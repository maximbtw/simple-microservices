namespace Catalog.Contracts.Ingredient.GetIngredients;

public class GetIngredientsRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public int? IngredientId { get; set; }
    
    public int? Take { get; set; }
    
    public int? Skip { get; set; }
}