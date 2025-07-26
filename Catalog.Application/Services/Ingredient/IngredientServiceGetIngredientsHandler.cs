using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Domain.Ingredient;
using Microsoft.EntityFrameworkCore;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Ingredient;

internal class IngredientServiceGetIngredientsHandler
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public IngredientServiceGetIngredientsHandler(
        IIngredientRepository ingredientRepository,
        IDbScopeProvider scopeProvider)
    {
        _ingredientRepository = ingredientRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<GetIngredientsResponse> GetIngredients(GetIngredientsRequest request)
    {
        var response = new GetIngredientsResponse();
        
        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();

        IMapper<IngredientOrm, IngredientDto> mapper = _ingredientRepository.GetMapper();

        List<IngredientOrm> orms = await GetMatchedQueryable(_ingredientRepository.QueryAll(scope), request)
            .Skip(request.Skip ?? 0)
            .Take(request.Take ?? int.MaxValue).ToListAsync();

        response.Ok = true;
        response.Ingredients = orms.ConvertAll(mapper.MapToDto);
        
        return response;
    }

    private IQueryable<IngredientOrm> GetMatchedQueryable(
        IQueryable<IngredientOrm> searchQuery,
        GetIngredientsRequest request)
    {
        if (request.IngredientId != null)
        {
            searchQuery = searchQuery.Where(x => x.Id == request.IngredientId);
        }

        return searchQuery;
    }
}