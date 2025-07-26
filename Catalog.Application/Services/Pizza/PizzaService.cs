using Catalog.Application.Persistence.Ingredient;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza;
using Catalog.Contracts.Pizza.CreatePizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Contracts.Pizza.UpdatePizza;
using Catalog.Domain.Ingredient;
using Platform.Domain.EF;

namespace Catalog.Application.Services.Pizza;

internal class PizzaService : IPizzaService
{
    private readonly PizzaServiceGetPizzasHandler _getPizzasHandler;
    private readonly PizzaServiceCreateHandler _createHandler;
    private readonly PizzaServiceGetOptionsHandler _getOptionsHandler;
    private readonly PizzaServiceUpdateHandler _updateHandler;

    public PizzaService(
        IPizzaRepository pizzaRepository,
        IIngredientRepository ingredientRepository,
        IDbScopeProvider scopeProvide)
    {
        _getPizzasHandler = new PizzaServiceGetPizzasHandler(pizzaRepository, scopeProvide);
        _createHandler = new PizzaServiceCreateHandler(pizzaRepository, scopeProvide);
        _getOptionsHandler = new PizzaServiceGetOptionsHandler(pizzaRepository, ingredientRepository, scopeProvide);
        _updateHandler = new PizzaServiceUpdateHandler(pizzaRepository, scopeProvide);
    }

    public async Task<GetPizzasResponse> GetPizzas(GetPizzasRequest request) =>
        await _getPizzasHandler.GetPizzas(request);

    public async Task<CreatePizzaResponse> CreatePizza(CreatePizzaRequest request) =>
        await _createHandler.CreatePizza(request);

    public async Task<GetPizzaOptionsResponse> GetPizzaOptions(GetPizzaOptionsRequest request) =>
        await _getOptionsHandler.GetPizzaOptions(request);

    public async Task<UpdatePizzaResponse> UpdatePizza(UpdatePizzaRequest request) =>
        await _updateHandler.UpdatePizza(request);
}