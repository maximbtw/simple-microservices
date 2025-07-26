namespace Catalog.Contracts.Pizza.CreatePizza;

public class CreatePizzaRequest
{
    public int PizzeriaAccountId { get; set; }
    
    public CreatePizzaModel CreateModel { get; set; } = new();
}