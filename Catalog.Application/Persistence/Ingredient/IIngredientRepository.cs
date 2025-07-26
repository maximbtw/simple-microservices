using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Core.Persistence;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Persistence.Ingredient;

public interface IIngredientRepository
{
    IQueryable<IngredientOrm> QueryAll(OperationScopeBase scope);
    
    Task<IngredientDto?> GetOrNull(int id, OperationScopeBase scope);

    Task<EntityCreateResult<IngredientDto>> Create(
        EntityCreateParameters<IngredientDto> parameters,
        OperationModificationScope scope);

    Task<EntityUpdateResult<IngredientDto>> Update(
        EntityUpdateParameters<IngredientDto> parameters,
        OperationModificationScope scope);

    IMapper<IngredientOrm, IngredientDto> GetMapper();
}