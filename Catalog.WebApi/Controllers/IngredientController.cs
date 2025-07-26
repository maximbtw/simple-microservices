using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Ingredient.CreateIngredient;
using Catalog.Contracts.Ingredient.GetIngredientOptions;
using Catalog.Contracts.Ingredient.GetIngredients;
using Catalog.Contracts.Ingredient.UpdateIngredient;
using Microsoft.AspNetCore.Mvc;
using Platform.Domain;
using Platform.WebApi;

namespace Catalog.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientController(IIngredientService service) : ControllerBase, IIngredientService
{
    [HttpPost("[action]")]
    public async Task<GetIngredientsResponse> GetIngredients(GetIngredientsRequest request) =>
        await service.GetIngredients(request);
    
    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<GetIngredientOptionsResponse> GetIngredientOptions(GetIngredientOptionsRequest request) =>
        await service.GetIngredientOptions(request);

    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<CreateIngredientResponse> CreateIngredient(CreateIngredientRequest request) =>
        await service.CreateIngredient(request);

    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<UpdateIngredientResponse> UpdateIngredient(UpdateIngredientRequest request) =>
        await service.UpdateIngredient(request);
}