namespace Catalog.Contracts.Ingredient.GetIngredientOptions;

public class GetIngredientOptionsRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public int? IngredientId { get; set; }
}