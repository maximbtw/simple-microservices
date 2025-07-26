namespace PizzeriaApi.Contracts.Shared;

public record Pizza(
    int Id,
    string Name,
    string ImageUrl,
    int ImageId,
    string Description,
    bool IsActive,
    List<PizzaPrice> Prices,
    List<PizzaIngredient> Ingredients)
{
    public static Pizza CreateEmpty() => new(
        Id: 0,
        Name: string.Empty,
        ImageUrl: string.Empty,
        ImageId: 0,
        Description: string.Empty,
        IsActive: true,
        Prices: [],
        Ingredients: []);
}
