namespace Catalog.Contracts.Pizza.GetPizzas;

public class GetPizzasRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public int? PizzaId { get; set; }
    
    public int? Take { get; set; }
    
    public int? Skip { get; set; }
}