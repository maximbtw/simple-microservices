using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Ingredient.UpdateIngredient;
using Platform.Client.Http;

namespace Catalog.Client;

public class IngredientServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IIngredientService
{
    public async Task<GetIngredientsResponse> GetIngredients(GetIngredientsRequest request) =>
        await PostAsync<GetIngredientsRequest, GetIngredientsResponse>(request);

    public async Task<GetIngredientOptionsResponse> GetIngredientOptions(GetIngredientOptionsRequest request) =>
        await PostAsync<GetIngredientOptionsRequest, GetIngredientOptionsResponse>(request);

    public async Task<CreateIngredientResponse> CreateIngredient(CreateIngredientRequest request) =>
        await PostAsync<CreateIngredientRequest, CreateIngredientResponse>(request);

    public async Task<UpdateIngredientResponse> UpdateIngredient(UpdateIngredientRequest request) =>
        await PostAsync<UpdateIngredientRequest, UpdateIngredientResponse>(request);
}