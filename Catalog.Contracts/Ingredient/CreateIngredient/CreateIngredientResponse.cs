using Catalog.Contracts.Persistence.Ingredient;
using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.CreateIngredient;

public class CreateIngredientResponse : OperationResponseBase
{
    public IngredientDto Ingredient { get; set; } = null!;
}