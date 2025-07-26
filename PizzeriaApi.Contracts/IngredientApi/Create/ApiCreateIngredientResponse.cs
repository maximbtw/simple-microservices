using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.IngredientApi.Create;

public class ApiCreateIngredientResponse : ApiResponseBase
{
    public Ingredient Ingredient { get; set; } = null!;
}