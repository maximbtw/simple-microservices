using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Core.Persistence;

namespace Catalog.Application.Persistence.Ingredient;

internal class IngredientRepository : EntityRepositoryBase<
        IngredientOrm,
        IngredientDto,
        EntityCreateParameters<IngredientDto>,
        EntityCreateResult<IngredientDto>,
        EntityUpdateParameters<IngredientDto>,
        EntityUpdateResult<IngredientDto>,
        EntityDeleteParameters,
        EntityDeleteResult>,
    IIngredientRepository
{
    public override IngredientMapper GetMapper() => new();
}