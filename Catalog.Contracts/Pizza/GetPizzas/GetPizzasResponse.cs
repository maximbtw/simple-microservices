using Catalog.Contracts.Persistence.Pizza;
using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.GetPizzas;

public class GetPizzasResponse : OperationResponseBase
{
    public List<PizzaDto> Pizzas { get; set; } = new();
}