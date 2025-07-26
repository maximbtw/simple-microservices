using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.GetIngredientOptions;

public class GetIngredientOptionsResponseErrors : OperationResponseStandardErrors
{
    public bool IngredientNotFound { get; set; }
}