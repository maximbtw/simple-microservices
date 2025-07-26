using Catalog.Contracts.Persistence.Ingredient;
using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.GetIngredientOptions;

public class GetIngredientOptionsResponse : OperationResponseBase<GetIngredientOptionsResponseErrors>
{
    public IngredientDto Ingredient { get; set; } = null!;
}