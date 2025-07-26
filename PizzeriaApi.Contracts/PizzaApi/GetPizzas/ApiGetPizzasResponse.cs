namespace PizzeriaApi.Contracts.PizzaApi.GetPizzas;

public class ApiGetPizzasResponse : ApiResponseBase
{
    public List<ApiGetPizzasResponseItem> Items { get; set; } = new();
}