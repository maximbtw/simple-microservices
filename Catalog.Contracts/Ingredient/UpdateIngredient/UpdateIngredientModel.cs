namespace Catalog.Contracts.Ingredient.UpdateIngredient;

public class UpdateIngredientModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    
    public decimal Price { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
}