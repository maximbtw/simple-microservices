using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Domain.Ingredient;
using Microsoft.EntityFrameworkCore;
using Platform.Core.Persistence;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Catalog.Application.Services.Pizza;

internal class PizzaServiceGetOptionsHandler
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public PizzaServiceGetOptionsHandler(
        IPizzaRepository pizzaRepository,
        IIngredientRepository ingredientRepository,
        IDbScopeProvider scopeProvider)
    {
        _pizzaRepository = pizzaRepository;
        _ingredientRepository = ingredientRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<GetPizzaOptionsResponse> GetPizzaOptions(GetPizzaOptionsRequest request)
    {
        var response = new GetPizzaOptionsResponse();

        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();
        
        IMapper<IngredientOrm, IngredientDto> mapper = _ingredientRepository.GetMapper();

        List<IngredientOrm> ingredientOrms = await _ingredientRepository
            .QueryAll(scope)
            .Where(x => x.PizzeriaAccountId == request.PizzeriaAccountId)
            .ToListAsync();

        PizzaDto? model = null;
        if (request.PizzaId != null)
        {
            model = await _pizzaRepository.GetOrNull((int)request.PizzaId, scope);
            if (model == null)
            {
                response.Errors.PizzaNotFound = true;
                return response;
            }

            if (model.PizzeriaAccountId != request.PizzeriaAccountId)
            {
                response.Errors.PizzaNotFound = true;
                return response;
            }

            response.Pizza = model;
            response.Ingredients = ingredientOrms
                .Where(x => model.IngredientIds.Contains(x.Id))
                .Select(mapper.MapToDto)
                .ToList();
        }

        response.AvailableIngredients = ingredientOrms
            .Where(x => model == null || !model.IngredientIds.Contains(x.Id))
            .Select(mapper.MapToDto)
            .ToList();

        response.Ok = true;

        return response;
    }
}