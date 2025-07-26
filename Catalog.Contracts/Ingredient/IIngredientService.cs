using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Ingredient.UpdateIngredient;

namespace Catalog.Contracts.Ingredient;

public interface IIngredientService
{
    Task<GetIngredientsResponse> GetIngredients(GetIngredientsRequest request);
    
    Task<GetIngredientOptionsResponse> GetIngredientOptions(GetIngredientOptionsRequest request);
    
    Task<CreateIngredientResponse> CreateIngredient(CreateIngredientRequest request);
    
    Task<UpdateIngredientResponse> UpdateIngredient(UpdateIngredientRequest request);
}