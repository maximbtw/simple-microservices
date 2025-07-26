using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PizzeriaApi.Contracts.Shared;
using Utilities.CustomAttributes;

namespace PizzeriaApi.Contracts.IngredientApi.Create;

public class ApiCreateIngredientFormModel : ApiRequestBase
{
    [Required] 
    public IFormFile ImageFile { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public bool IsActive { get; set; }
}