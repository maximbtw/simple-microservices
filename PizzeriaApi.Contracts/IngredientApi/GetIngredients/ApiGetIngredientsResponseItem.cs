namespace PizzeriaApi.Contracts.IngredientApi.GetIngredients;

public record ApiGetIngredientsResponseItem(int Id, string Name, string ImageUrl, bool IsActive, decimal Price);