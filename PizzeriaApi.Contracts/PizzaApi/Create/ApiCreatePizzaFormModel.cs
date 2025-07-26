using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaApi.Contracts.Shared;
using Utilities.CustomAttributes;

namespace PizzeriaApi.Contracts.PizzaApi.Create;

public class ApiCreatePizzaFormModel : ApiRequestBase
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public IFormFile ImageFile { get; set; } = null!;

    [Required]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    [Required]
    [NotEmpty]
    [FromForm]
    public string IngredientIdsJson { get; set; } = string.Empty;

    [Required]
    [NotEmpty]
    [FromForm(Name = "prices")]
    public string PricesJson { get; set; } = string.Empty;
    
    [NotMapped]
    public List<int> IngredientIds => JsonConvert.DeserializeObject<List<int>>(this.IngredientIdsJson) ?? [];

    [NotMapped]
    public List<PizzaPrice> Prices => JsonConvert.DeserializeObject<List<PizzaPrice>>(this.PricesJson) ?? [];
}