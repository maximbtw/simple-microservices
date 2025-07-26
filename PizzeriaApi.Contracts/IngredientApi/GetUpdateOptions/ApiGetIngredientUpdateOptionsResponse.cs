using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.IngredientApi.GetUpdateOptions;

public class ApiGetIngredientUpdateOptionsResponse : ApiResponseBase
{
    public Ingredient Ingredient { get; set; } = null!;
}