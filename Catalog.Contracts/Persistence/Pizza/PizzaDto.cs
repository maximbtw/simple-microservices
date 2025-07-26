using Platform.Core.Persistence;

namespace Catalog.Contracts.Persistence.Pizza;

public class PizzaDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public int ImageId { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int PizzeriaAccountId { get; set; } 

    public List<PizzaPriceDto> Prices { get; set; } = new();

    public List<int> IngredientIds { get; set; } = new();
}