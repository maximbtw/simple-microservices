using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Ingredient;

internal class IngredientServiceGetOptionsHandler
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public IngredientServiceGetOptionsHandler(IIngredientRepository ingredientRepository, IDbScopeProvider scopeProvider)
    {
        _ingredientRepository = ingredientRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<GetIngredientOptionsResponse> GetIngredientOptions(GetIngredientOptionsRequest request)
    {
        var response = new GetIngredientOptionsResponse();
        
        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();
        
        if (request.IngredientId != null)
        {
            IngredientDto? model = await _ingredientRepository.GetOrNull(request.IngredientId.Value, scope);
            if (model == null)
            {
                response.Errors.IngredientNotFound = true;

                return response;
            }

            if (model.PizzeriaAccountId != request.PizzeriaAccountId)
            {
                response.Errors.IngredientNotFound = true;
                
                return response;
            }

            response.Ingredient = model;
        }

        response.Ok = true;

        return response;
    }
}