using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain.EF;

namespace Catalog.Domain.Ingredient;

[Table("Ingredients")]
public class IngredientOrm : IOrm
{
    public int Id { get; set; }
    public int Version { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int PizzeriaAccountId { get; set; }
    public int ImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
}