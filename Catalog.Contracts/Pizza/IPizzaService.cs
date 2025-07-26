using Catalog.Contracts.Pizza.CreatePizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Contracts.Pizza.UpdatePizza;

namespace Catalog.Contracts.Pizza;

public interface IPizzaService
{
    Task<GetPizzasResponse> GetPizzas(GetPizzasRequest request);
    
    Task<GetPizzaOptionsResponse> GetPizzaOptions(GetPizzaOptionsRequest request);
    
    Task<CreatePizzaResponse> CreatePizza(CreatePizzaRequest request);

    Task<UpdatePizzaResponse> UpdatePizza(UpdatePizzaRequest request);
}