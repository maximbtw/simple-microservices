using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Domain.Pizza;

[Table("PizzaPrices")]
public class PizzaPriceOrm
{
    public int PizzaId { get; set; }
    public decimal Price { get; set; }
    public int Size { get; set; }
}