namespace PizzeriaApi.Contracts.Shared;

public record Ingredient(
    int Id,
    string Name,
    string ImageUrl,
    int ImageId,
    decimal Price,
    bool IsActive)
{
    public static Ingredient CreateEmpty() => new(
        Id: 0, 
        Name: string.Empty, 
        ImageUrl: string.Empty, 
        ImageId: 0,
        Price: 0, 
        IsActive: true);
}