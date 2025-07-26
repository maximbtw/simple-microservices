using Platform.Core.Operations;

namespace Catalog.Contracts.Pizza.UpdatePizza;

public class UpdatePizzaResponseErrors : OperationResponseStandardErrors
{
    public bool PizzaNotFound { get; set; }
    
    public bool VersionConflict { get; set; }
}