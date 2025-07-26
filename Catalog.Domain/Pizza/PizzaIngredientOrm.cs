using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Ingredient;

namespace Catalog.Domain.Pizza;

[Table("PizzaIngredients")]
public class PizzaIngredientOrm
{
    public int PizzaId { get; set; }
    public int IngredientId { get; set; }
    
    public IngredientOrm Ingredient { get; set; } = null!;
}