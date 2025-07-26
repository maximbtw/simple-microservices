namespace Catalog.Contracts.Ingredient.CreateIngredient;

public class CreateIngredientRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public CreateIngredientModel CreateModel { get; set; } = new();
}