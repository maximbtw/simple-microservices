namespace PizzeriaApi.Contracts.IngredientApi.GetIngredients;

public class ApiGetIngredientsResponse : ApiResponseBase
{
    public List<ApiGetIngredientsResponseItem> Items { get; set; } = new();
}