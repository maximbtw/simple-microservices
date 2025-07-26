namespace Catalog.Contracts.Ingredient.CreateIngredient;

public class CreateIngredientModel
{
    public int ImageId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}