using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza.CreatePizza;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Pizza;

internal class PizzaServiceCreateHandler
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public PizzaServiceCreateHandler(
        IPizzaRepository pizzaRepository,
        IDbScopeProvider scopeProvider)
    {
        _pizzaRepository = pizzaRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<CreatePizzaResponse> CreatePizza(CreatePizzaRequest request)
    {
        var response = new CreatePizzaResponse();
        
        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        EntityCreateResult<PizzaDto> createPizzaResponse = await CreatePizza(request, scope);

        if (createPizzaResponse.Ok)
        {
            await scope.CommitChangesIfSucceededAsync(createPizzaResponse.Ok);

            response.Ok = true;
            response.Pizza = createPizzaResponse.CreatedEntity!;
        }

        return response;
    }

    private async Task<EntityCreateResult<PizzaDto>> CreatePizza(
        CreatePizzaRequest request,
        OperationModificationScope scope)
    {
        var parameters = new EntityCreateParameters<PizzaDto>
        {
            Entity = new PizzaDto
            {
                Name = request.CreateModel.Name,
                Description = request.CreateModel.Description,
                ImageId = request.CreateModel.ImageId,
                ImageUrl = request.CreateModel.ImageUrl,
                IsActive = request.CreateModel.IsActive,
                IngredientIds = request.CreateModel.IngredientIds,
                Prices = request.CreateModel.Prices,
                PizzeriaAccountId = request.PizzeriaAccountId,
            }
        };
        
        return await _pizzaRepository.Create(parameters, scope);
    }
}