using Catalog.Contracts.Persistence.Ingredient;
using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.UpdateIngredient;

public class UpdateIngredientResponse : OperationResponseBase<UpdateIngredientResponseErrors>
{
    public IngredientDto Ingredient { get; set; } = null!;
}