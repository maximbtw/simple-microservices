using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Persistence.Pizza;
using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.UpdatePizza;

public class UpdatePizzaResponse : OperationResponseBase<UpdatePizzaResponseErrors>
{
    public PizzaDto Pizza { get; set; } = null!;
}