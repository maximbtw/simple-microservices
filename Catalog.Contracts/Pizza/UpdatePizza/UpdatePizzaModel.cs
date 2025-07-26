using Catalog.Contracts.Persistence.Pizza;

namespace Catalog.Contracts.Pizza.UpdatePizza;

public class UpdatePizzaModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; }= string.Empty;
    
    public string ImageUrl { get; set; }= string.Empty;

    public bool IsActive { get; set; }

    public List<int> IngredientIds { get; set; } = new();
    
    public List<PizzaPriceDto> Prices { get; set; } = new();
}