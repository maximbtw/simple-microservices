using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain.EF;

namespace Catalog.Domain.Pizza;

[Table("Pizzas")]
public class PizzaOrm : IOrm
{
    public int Id { get; set; }
    public int Version { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int PizzeriaAccountId { get; set; }
    public List<PizzaIngredientOrm> Ingredients { get; set; } = new();
    public List<PizzaPriceOrm> Prices { get; set; } = new();
}