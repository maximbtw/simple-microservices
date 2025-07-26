using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.IngredientApi.Update;

public class ApiUpdateIngredientResponse : ApiResponseBase
{
    public Ingredient Ingredient { get; set; } = null!;
}