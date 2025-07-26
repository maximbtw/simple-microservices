using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.PizzaApi.Update;

public class ApiUpdatePizzaResponse : ApiResponseBase
{
    public Pizza Pizza { get; set; } = null!;
}