namespace Catalog.Contracts.Pizza.GetPizzaOptions;

public class GetPizzaOptionsRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public int? PizzaId { get; set; }
}