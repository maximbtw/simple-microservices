using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.GetPizzaOptions;

public class GetPizzaOptionsResponseErrors : OperationResponseStandardErrors
{
    public bool PizzaNotFound { get; set; }
}