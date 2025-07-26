using Catalog.Contracts.Pizza;
using Catalog.Contracts.Pizza.CreatePizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Contracts.Pizza.UpdatePizza;
using Platform.Client.Http;

namespace Catalog.Client;

public class PizzaServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IPizzaService
{
    public async Task<GetPizzasResponse> GetPizzas(GetPizzasRequest request) =>
        await PostAsync<GetPizzasRequest, GetPizzasResponse>(request);

    public async Task<GetPizzaOptionsResponse> GetPizzaOptions(GetPizzaOptionsRequest request) =>
        await PostAsync<GetPizzaOptionsRequest, GetPizzaOptionsResponse>(request);

    public async Task<CreatePizzaResponse> CreatePizza(CreatePizzaRequest request) =>
        await PostAsync<CreatePizzaRequest, CreatePizzaResponse>(request);

    public async Task<UpdatePizzaResponse> UpdatePizza(UpdatePizzaRequest request) =>
        await PostAsync<UpdatePizzaRequest, UpdatePizzaResponse>(request);
}