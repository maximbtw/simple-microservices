using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Ingredient.UpdateIngredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Domain.EF;

namespace Catalog.Application.Services.Ingredient;

public class IngredientService : IIngredientService
{
    private readonly IngredientServiceCreateHandler _createHandler;
    private readonly IngredientServiceGetIngredientsHandler _getIngredientsHandler;
    private readonly IngredientServiceGetOptionsHandler _getOptionsHandler;
    private readonly IngredientServiceUpdateHandler _updateHandler;
    
    public IngredientService(IIngredientRepository ingredientRepository, IDbScopeProvider scopeProvider)
    {
        _createHandler = new IngredientServiceCreateHandler(ingredientRepository, scopeProvider);
        _getIngredientsHandler = new IngredientServiceGetIngredientsHandler(ingredientRepository, scopeProvider);
        _getOptionsHandler = new IngredientServiceGetOptionsHandler(ingredientRepository, scopeProvider);
        _updateHandler = new IngredientServiceUpdateHandler(ingredientRepository, scopeProvider);
    }

    public async Task<GetIngredientsResponse> GetIngredients(GetIngredientsRequest request) =>
        await _getIngredientsHandler.GetIngredients(request);

    public async Task<GetIngredientOptionsResponse> GetIngredientOptions(GetIngredientOptionsRequest request) =>
        await _getOptionsHandler.GetIngredientOptions(request);

    public async Task<CreateIngredientResponse> CreateIngredient(CreateIngredientRequest request) =>
        await _createHandler.CreateIngredient(request);

    public async Task<UpdateIngredientResponse> UpdateIngredient(UpdateIngredientRequest request) =>
        await _updateHandler.UpdateIngredient(request);
}