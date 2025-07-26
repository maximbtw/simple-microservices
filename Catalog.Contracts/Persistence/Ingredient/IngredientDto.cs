using Platform.Core.Persistence;

namespace Catalog.Contracts.Persistence.Ingredient;

public class IngredientDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public int ImageId { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public bool IsActive { get; set; }
    
    public int PizzeriaAccountId { get; set; } 
    
    public decimal Price { get; set; }
}