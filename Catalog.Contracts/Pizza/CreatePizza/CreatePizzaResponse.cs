using Catalog.Contracts.Persistence.Pizza;
using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.CreatePizza;

public class CreatePizzaResponse : OperationResponseBase
{
    public PizzaDto Pizza { get; set; } = null!;
}