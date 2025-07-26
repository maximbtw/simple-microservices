using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.PizzaApi.Create;

public class ApiCreatePizzaResponse : ApiResponseBase
{
    public Pizza Pizza { get; set; } = null!;
}