using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza;
using Catalog.Contracts.Pizza.CreatePizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Contracts.Pizza.UpdatePizza;
using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Media.Contracts.Persistence.Image;
using PizzeriaApi.Contracts;
using PizzeriaApi.Contracts.PizzaApi;
using PizzeriaApi.Contracts.PizzaApi.Create;
using PizzeriaApi.Contracts.PizzaApi.GetOptions;
using PizzeriaApi.Contracts.PizzaApi.GetPizzas;
using PizzeriaApi.Contracts.PizzaApi.Update;
using PizzeriaApi.Contracts.Providers;
using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Application.Services;

internal class ApiPizzaService : IApiPizzaService
{
    private readonly IPizzaService _pizzaService;
    private readonly IImageService _imageService;
    private readonly IAccountUserProvider _accountUserProvider;

    public ApiPizzaService(
        IPizzaService pizzaService, 
        IImageService imageService, 
        IAccountUserProvider accountUserProvider)
    {
        _pizzaService = pizzaService;
        _imageService = imageService;
        _accountUserProvider = accountUserProvider;
    }
    
    public async Task<ApiGetPizzasResponse> GetPizzas()
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetPizzasRequest
        {
            PizzeriaAccountId = user.AccountId
        };
        
        return await OperationInvoker.InvokeServiceAsync<GetPizzasResponse, ApiGetPizzasResponse>(
            () => _pizzaService.GetPizzas(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Items = s.Pizzas.ConvertAll(x => new ApiGetPizzasResponseItem(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Description,
                    x.IsActive,
                    x.Prices.Min(z => z.Price),
                    x.Prices.Max(z => z.Price)));
            });
    }

    public async Task<ApiGetPizzaCreateOptionsResponse> GetCreateOptions(int? copyId)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetPizzaOptionsRequest
        {
            PizzeriaAccountId = user.AccountId,
            PizzaId = copyId
        };

        return await OperationInvoker.InvokeServiceAsync<
            GetPizzaOptionsResponse,
            GetPizzaOptionsResponseErrors,
            ApiGetPizzaCreateOptionsResponse>(
            () => _pizzaService.GetPizzaOptions(request),
            onSuccess: (s, apiResponse) =>
            {
                if (s.Pizza != null)
                {
                    apiResponse.Pizza = new Pizza(
                        Id: 0,
                        s.Pizza.Name,
                        ImageUrl: string.Empty,
                        ImageId: 0,
                        s.Pizza.Description,
                        s.Pizza.IsActive,
                        s.Pizza.Prices.ConvertAll(x => new PizzaPrice(x.Size, x.Price)),
                        s.Ingredients!.ConvertAll(x => new PizzaIngredient(x.Id, x.Name, x.ImageUrl, x.Price)));
                }
                else
                {
                    apiResponse.Pizza = Pizza.CreateEmpty();
                }

                apiResponse.AvailableIngredients = s.AvailableIngredients.ConvertAll(x => new PizzaIngredient(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Price));
            },
            onFailed: (s, apiResponse) =>
            {
                apiResponse.AddErrorIf(s.Errors.PizzaNotFound, ErrorCode.NotFound, "Pizza not found.");
            });
    }

    public async Task<ApiGetPizzaUpdateOptionsResponse> GetUpdateOptions(int id)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetPizzaOptionsRequest
        {
            PizzeriaAccountId = user.AccountId,
            PizzaId = id
        };

        return await OperationInvoker.InvokeServiceAsync<
            GetPizzaOptionsResponse,
            GetPizzaOptionsResponseErrors,
            ApiGetPizzaUpdateOptionsResponse>(
            () => _pizzaService.GetPizzaOptions(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Pizza = MapPizzaFromDto(
                    s.Pizza!,
                    s.Ingredients!.ConvertAll(x => new PizzaIngredient(x.Id, x.Name, x.ImageUrl, x.Price)));

                apiResponse.AvailableIngredients = s.AvailableIngredients.ConvertAll(x => new PizzaIngredient(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.Price));
            },
            onFailed: (s, apiResponse) =>
            {
                apiResponse.AddErrorIf(s.Errors.PizzaNotFound, ErrorCode.NotFound);
            });
    }

    public async Task<ApiCreatePizzaResponse> Create(ApiCreatePizzaFormModel model)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        UploadImageResponse uploadResult =
            await ImageInvoker.UploadImage(_imageService, model.ImageFile, ImageFolder.Ingredients);

        if (!uploadResult.Ok)
        {
            var response = new ApiCreatePizzaResponse();
            OperationInvoker.MapStandardErrors(response, uploadResult.Errors);

            return response;
        }

        var request = new CreatePizzaRequest
        {
            PizzeriaAccountId = user.AccountId,
            CreateModel = new CreatePizzaModel
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                IngredientIds = model.IngredientIds,
                Prices = model.Prices.ConvertAll(x => new PizzaPriceDto
                {
                    Price = x.Price,
                    Size = x.Size
                }),
                ImageId = uploadResult.Image.Id,
                ImageUrl = uploadResult.Image.Url
            }
        };

        ApiCreatePizzaResponse createResult = await OperationInvoker.InvokeServiceAsync<
            CreatePizzaResponse,
            ApiCreatePizzaResponse>(
            () => _pizzaService.CreatePizza(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Pizza = MapPizzaFromDto(s.Pizza, ingredients: []);
            });

        if (!createResult.Ok)
        {
            await OperationInvoker.InvokeServiceAsync<DeleteImageResponse, ApiCreatePizzaResponse>(
                () => ImageInvoker.DeleteImage(_imageService, uploadResult.Image.Id));
        }

        return createResult;
    }

    public async Task<ApiUpdatePizzaResponse> Update(ApiUpdatePizzaFormModel model)
    {
        string? imageUrl = model.ImageUrl;
        if (model.ImageFile != null)
        {
            UpdateImageResponse updateImageResponse =
                await ImageInvoker.UpdateImage(_imageService, model.ImageFile, model.ImageId);

            if (!updateImageResponse.Ok)
            {
                var response = new ApiUpdatePizzaResponse();
                OperationInvoker.MapStandardErrors(response, updateImageResponse.Errors);

                return response;
            }

            imageUrl = model.ImageUrl;
        }

        var request = new UpdatePizzaRequest
        {
            UpdateModel = new UpdatePizzaModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                IngredientIds = model.IngredientIds,
                Prices = model.Prices.ConvertAll(x => new PizzaPriceDto
                {
                    Price = x.Price,
                    Size = x.Size
                }),
                ImageUrl = imageUrl!
            }
        };

        return await OperationInvoker.InvokeServiceAsync<
            UpdatePizzaResponse,
            UpdatePizzaResponseErrors,
            ApiUpdatePizzaResponse>(
            () => _pizzaService.UpdatePizza(request),
            onFailed: (s, apiResponse) =>
            {
                apiResponse.AddErrorIf(s.Errors.PizzaNotFound, ErrorCode.NotFound)
                    .AddErrorIf(
                        s.Errors.VersionConflict, 
                        ErrorCode.Conflict, 
                        "Version conflict",
                        PizzaUpdateErrorType.VersionConflict);
            },
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Pizza = MapPizzaFromDto(s.Pizza, ingredients: []);
            });
    }

    private static Pizza MapPizzaFromDto(PizzaDto dto, List<PizzaIngredient> ingredients)
    {
        return new Pizza(
            dto.Id,
            dto.Name,
            dto.ImageUrl,
            dto.ImageId,
            dto.Description,
            dto.IsActive,
            dto.Prices.ConvertAll(x => new PizzaPrice(x.Size, x.Price)),
            ingredients);
    }
}