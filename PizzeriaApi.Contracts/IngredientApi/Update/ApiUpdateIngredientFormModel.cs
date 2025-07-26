using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Utilities.CustomAttributes;

namespace PizzeriaApi.Contracts.IngredientApi.Update;

public class ApiUpdateIngredientFormModel : ApiRequestBase
{
    [Positive] 
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Positive] 
    public decimal Price { get; set; }
    
    public bool IsActive { get; set; }
    
    public IFormFile? ImageFile { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [Positive] 
    public int ImageId { get; set; }
}