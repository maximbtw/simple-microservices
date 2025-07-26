namespace PizzeriaApi.Contracts.PizzaApi.GetPizzas;

public record ApiGetPizzasResponseItem(
    int Id, 
    string Name, 
    string ImageUrl, 
    string Description, 
    bool IsActive, 
    decimal MinPrice, 
    decimal MaxPrice);