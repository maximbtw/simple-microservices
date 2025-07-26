using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza.UpdatePizza;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Utilities;

namespace Catalog.Application.Services.Pizza;

internal class PizzaServiceUpdateHandler
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public PizzaServiceUpdateHandler(
        IPizzaRepository pizzaRepository,
        IDbScopeProvider scopeProvider)
    {
        _pizzaRepository = pizzaRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<UpdatePizzaResponse> UpdatePizza(UpdatePizzaRequest request)
    {
        var response = new UpdatePizzaResponse();
        
        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        PizzaDto? currentModel = await _pizzaRepository.GetOrNull(request.UpdateModel.Id, scope);
        if (currentModel == null)
        {
            response.Errors.PizzaNotFound = true;
            return response;
        }

        EntityUpdateResult<PizzaDto> updateResult = await UpdatePizza(currentModel, request, scope);
        if (updateResult.Ok)
        {
            await scope.CommitChangesIfSucceededAsync(updateResult.Ok);

            response.Pizza = updateResult.UpdatedEntity!;
            response.Ok = true;
        }

        response.Errors.VersionConflict = updateResult.VersionConflict;

        return response;
    }
    
    private async Task<EntityUpdateResult<PizzaDto>> UpdatePizza(
        PizzaDto currentModel,
        UpdatePizzaRequest request,
        OperationModificationScope scope)
    {
        PizzaDto modifiedModel = CloneHelper.CloneDeep(currentModel, copyIds: true);
        
        modifiedModel.Name = request.UpdateModel.Name;
        modifiedModel.Description = request.UpdateModel.Description;
        modifiedModel.IsActive = request.UpdateModel.IsActive;
        modifiedModel.IngredientIds = request.UpdateModel.IngredientIds;
        modifiedModel.Prices = request.UpdateModel.Prices;
        modifiedModel.ImageUrl = request.UpdateModel.ImageUrl;
        
        var parameters = new EntityUpdateParameters<PizzaDto>
        {
            Entity = modifiedModel,
        };

        return await _pizzaRepository.Update(parameters, scope);
    }
}