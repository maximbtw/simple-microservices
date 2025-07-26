using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Ingredient.UpdateIngredient;
using Media.Contracts.Image;
using Media.Contracts.Image.Delete;
using Media.Contracts.Image.Update;
using Media.Contracts.Image.Upload;
using Media.Contracts.Persistence.Image;
using PizzeriaApi.Contracts;
using PizzeriaApi.Contracts.IngredientApi;
using PizzeriaApi.Contracts.IngredientApi.Create;
using PizzeriaApi.Contracts.IngredientApi.GetCreateOptions;
using PizzeriaApi.Contracts.IngredientApi.GetIngredients;
using PizzeriaApi.Contracts.IngredientApi.GetUpdateOptions;
using PizzeriaApi.Contracts.IngredientApi.Update;
using PizzeriaApi.Contracts.Providers;
using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Application.Services;

internal class ApiIngredientService : IApiIngredientService
{
    private readonly IIngredientService _ingredientService;
    private readonly IImageService _imageService;
    private readonly IAccountUserProvider _accountUserProvider;

    public ApiIngredientService(
        IIngredientService ingredientService, 
        IImageService imageService,
        IAccountUserProvider accountUserProvider)
    {
        _ingredientService = ingredientService;
        _imageService = imageService;
        _accountUserProvider = accountUserProvider;
    }

    public async Task<ApiGetIngredientsResponse> GetIngredients()
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetIngredientsRequest
        {
            PizzeriaAccountId = user.AccountId
        };
        
        return await OperationInvoker.InvokeServiceAsync<GetIngredientsResponse, ApiGetIngredientsResponse>(
            () => _ingredientService.GetIngredients(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Items = s.Ingredients.ConvertAll(x => new ApiGetIngredientsResponseItem(
                    x.Id,
                    x.Name,
                    x.ImageUrl,
                    x.IsActive,
                    x.Price));
            });
    }

    public async Task<ApiGetIngredientCreateOptionsResponse> GetCreateOptions(int? copyId)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetIngredientOptionsRequest
        {
            PizzeriaAccountId = user.AccountId,
            IngredientId = copyId
        };

        return await OperationInvoker.InvokeServiceAsync<
            GetIngredientOptionsResponse,
            GetIngredientOptionsResponseErrors,
            ApiGetIngredientCreateOptionsResponse>(
            () => _ingredientService.GetIngredientOptions(request),
            onSuccess: (s, apiResponse) =>
            {
                if (copyId.HasValue)
                {
                    apiResponse.Ingredient = new Ingredient(
                        Id: 0,
                        s.Ingredient.Name,
                        ImageUrl: string.Empty,
                        ImageId: 0,
                        s.Ingredient.Price,
                        s.Ingredient.IsActive);
                }
                else
                {
                    apiResponse.Ingredient = Ingredient.CreateEmpty();
                }
            },
            onFailed: (s, apiResponse) =>
            {
                apiResponse.AddErrorIf(s.Errors.IngredientNotFound, ErrorCode.NotFound);
            });
    }

    public async Task<ApiGetIngredientUpdateOptionsResponse> GetUpdateOptions(int id)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        var request = new GetIngredientOptionsRequest
        {
            PizzeriaAccountId = user.AccountId,
            IngredientId = id
        };

        return await OperationInvoker.InvokeServiceAsync<
            GetIngredientOptionsResponse,
            GetIngredientOptionsResponseErrors,
            ApiGetIngredientUpdateOptionsResponse>(
            () => _ingredientService.GetIngredientOptions(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Ingredient = new Ingredient(
                    s.Ingredient.Id,
                    s.Ingredient.Name,
                    s.Ingredient.ImageUrl,
                    s.Ingredient.ImageId,
                    s.Ingredient.Price,
                    s.Ingredient.IsActive);
            },
            onFailed: (s, apiResponse) =>
            {
                apiResponse.AddErrorIf(s.Errors.IngredientNotFound, ErrorCode.NotFound);
            });
    }

    public async Task<ApiCreateIngredientResponse> Create(ApiCreateIngredientFormModel model)
    {
        AccountUserModel user = await _accountUserProvider.GetCurrentAccountUser();
        UploadImageResponse uploadResponse = await ImageInvoker.UploadImage(_imageService, model.ImageFile, ImageFolder.Ingredients);
        if (!uploadResponse.Ok)
        {
            var response = new ApiCreateIngredientResponse();
            OperationInvoker.MapStandardErrors(response, uploadResponse.Errors);
            return response;
        }

        var request = new CreateIngredientRequest
        {
            PizzeriaAccountId = user.AccountId,
            CreateModel = new CreateIngredientModel
            {
                Name = model.Name,
                Price = model.Price,
                IsActive = model.IsActive,
                ImageId = uploadResponse.Image.Id,
                ImageUrl = uploadResponse.Image.Url
            }
        };

        ApiCreateIngredientResponse createResult = await OperationInvoker.InvokeServiceAsync<
            CreateIngredientResponse,
            ApiCreateIngredientResponse>(
            () => _ingredientService.CreateIngredient(request),
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Ingredient = new Ingredient(
                    s.Ingredient.Id,
                    s.Ingredient.Name,
                    s.Ingredient.ImageUrl,
                    s.Ingredient.ImageId,
                    s.Ingredient.Price,
                    s.Ingredient.IsActive);
            });

        if (!createResult.Ok)
        {
            await OperationInvoker.InvokeServiceAsync<DeleteImageResponse, ApiCreateIngredientResponse>(
                () => ImageInvoker.DeleteImage(_imageService, uploadResponse.Image.Id));
        }

        return createResult;
    }

    public async Task<ApiUpdateIngredientResponse> Update(ApiUpdateIngredientFormModel model)
    {
        string? imageUrl = model.ImageUrl;
        if (model.ImageFile != null)
        {
            UpdateImageResponse updateImageResponse =
                await ImageInvoker.UpdateImage(_imageService, model.ImageFile, model.ImageId);

            if (!updateImageResponse.Ok)
            {
                var response = new ApiUpdateIngredientResponse();
                OperationInvoker.MapStandardErrors(response, updateImageResponse.Errors);

                return response;
            }

            imageUrl = updateImageResponse.ImageUrl;
        }

        var request = new UpdateIngredientRequest
        {
            UpdateModel = new UpdateIngredientModel
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                IsActive = model.IsActive,
                ImageUrl = imageUrl!
            }
        };

        return await OperationInvoker.InvokeServiceAsync<
            UpdateIngredientResponse,
            UpdateIngredientResponseErrors,
            ApiUpdateIngredientResponse>(
            () => _ingredientService.UpdateIngredient(request),
            onFailed: (s, apiResponse) =>
            {
                apiResponse
                    .AddErrorIf(s.Errors.IngredientNotFound, ErrorCode.NotFound)
                    .AddErrorIf(s.Errors.VersionConflict, ErrorCode.Conflict, IngredientUpdateErrorType.VersionConflict.ToString());
            },
            onSuccess: (s, apiResponse) =>
            {
                apiResponse.Ingredient = new Ingredient(
                    s.Ingredient.Id,
                    s.Ingredient.Name,
                    s.Ingredient.ImageUrl,
                    s.Ingredient.ImageId,
                    s.Ingredient.Price,
                    s.Ingredient.IsActive);
            });
    }
}