using PizzeriaApi.Contracts.PizzaApi.Create;
using PizzeriaApi.Contracts.PizzaApi.GetOptions;
using PizzeriaApi.Contracts.PizzaApi.GetPizzas;
using PizzeriaApi.Contracts.PizzaApi.Update;

namespace PizzeriaApi.Contracts.PizzaApi;

public interface IApiPizzaService
{
    Task<ApiGetPizzasResponse> GetPizzas();

    Task<ApiGetPizzaCreateOptionsResponse> GetCreateOptions(int? copyId);

    Task<ApiGetPizzaUpdateOptionsResponse> GetUpdateOptions(int id);
    
    Task<ApiCreatePizzaResponse> Create(ApiCreatePizzaFormModel formModel);
    
    Task<ApiUpdatePizzaResponse> Update(ApiUpdatePizzaFormModel request);
}