using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.IngredientApi.GetCreateOptions;

public class ApiGetIngredientCreateOptionsResponse : ApiResponseBase
{
    public Ingredient Ingredient { get; set; } = null!;
}