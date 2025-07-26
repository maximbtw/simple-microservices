using PizzeriaApi.Contracts.IngredientApi.Create;
using PizzeriaApi.Contracts.IngredientApi.GetCreateOptions;
using PizzeriaApi.Contracts.IngredientApi.GetIngredients;
using PizzeriaApi.Contracts.IngredientApi.GetUpdateOptions;
using PizzeriaApi.Contracts.IngredientApi.Update;

namespace PizzeriaApi.Contracts.IngredientApi;

public interface IApiIngredientService
{
    Task<ApiGetIngredientsResponse> GetIngredients();

    Task<ApiGetIngredientCreateOptionsResponse> GetCreateOptions(int? copyId);
    
    Task<ApiGetIngredientUpdateOptionsResponse> GetUpdateOptions(int id);
    
    Task<ApiCreateIngredientResponse> Create(ApiCreateIngredientFormModel model);
    
    Task<ApiUpdateIngredientResponse> Update(ApiUpdateIngredientFormModel model);
}