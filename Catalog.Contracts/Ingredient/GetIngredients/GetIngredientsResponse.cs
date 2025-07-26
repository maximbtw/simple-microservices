using Catalog.Contracts.Persistence.Ingredient;
using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.GetIngredients;

public class GetIngredientsResponse : OperationResponseBase
{
    public List<IngredientDto> Ingredients { get; set; } = new();
}