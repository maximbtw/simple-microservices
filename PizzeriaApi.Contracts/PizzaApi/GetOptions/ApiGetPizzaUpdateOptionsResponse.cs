using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.PizzaApi.GetOptions;

public class ApiGetPizzaUpdateOptionsResponse : ApiResponseBase
{
    public Pizza Pizza { get; set; } = null!;

    public List<PizzaIngredient> AvailableIngredients { get; set; } = null!;
}