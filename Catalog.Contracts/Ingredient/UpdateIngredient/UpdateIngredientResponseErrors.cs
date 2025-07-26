using Platform.Core.Operations;

namespace Catalog.Contracts.Ingredient.UpdateIngredient;

public class UpdateIngredientResponseErrors : OperationResponseStandardErrors
{
    public bool IngredientNotFound { get; set; }
    
    public bool VersionConflict { get; set; }
}