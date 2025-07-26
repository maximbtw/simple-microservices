using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient.UpdateIngredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Utilities;

namespace Catalog.Application.Services.Ingredient;

internal class IngredientServiceUpdateHandler
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public IngredientServiceUpdateHandler(
        IIngredientRepository ingredientRepository,
        IDbScopeProvider scopeProvider)
    {
        _ingredientRepository = ingredientRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<UpdateIngredientResponse> UpdateIngredient(UpdateIngredientRequest request)
    {
        var response = new UpdateIngredientResponse();

        await using OperationModificationScope scope = _scopeProvider.GetModificationScope();

        IngredientDto? currentModel = await _ingredientRepository.GetOrNull(request.UpdateModel.Id, scope);
        if (currentModel == null)
        {
            response.Errors.IngredientNotFound = true;
            return response;
        }

        EntityUpdateResult<IngredientDto> updateResult = await UpdateIngredient(currentModel, request, scope);

        await scope.CommitChangesIfSucceededAsync(updateResult.Ok);

        response.Errors.VersionConflict = updateResult.VersionConflict;
        response.Ingredient = updateResult.UpdatedEntity ?? null!;
        response.Ok = updateResult.Ok;

        return response;
    }

    private async Task<EntityUpdateResult<IngredientDto>> UpdateIngredient(
        IngredientDto currentModel,
        UpdateIngredientRequest request,
        OperationModificationScope scope)
    {
        IngredientDto modifiedModel = CloneHelper.CloneDeep(currentModel, copyIds: true);

        modifiedModel.Name = request.UpdateModel.Name;
        modifiedModel.IsActive = request.UpdateModel.IsActive;
        modifiedModel.Price = request.UpdateModel.Price;
        modifiedModel.ImageUrl = request.UpdateModel.ImageUrl;

        var parameters = new EntityUpdateParameters<IngredientDto>
        {
            Entity = modifiedModel,
        };

        return await _ingredientRepository.Update(parameters, scope);
    }
}