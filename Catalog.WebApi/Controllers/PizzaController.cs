using Catalog.Contracts.Pizza;
using Catalog.Contracts.Pizza.CreatePizza;
using Catalog.Contracts.Pizza.GetPizzaOptions;
using Catalog.Contracts.Pizza.GetPizzas;
using Catalog.Contracts.Pizza.UpdatePizza;
using Microsoft.AspNetCore.Mvc;
using Platform.Domain;
using Platform.WebApi;

namespace Catalog.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(IPizzaService service) : ControllerBase, IPizzaService
{
    [HttpPost("[action]")]
    public async Task<GetPizzasResponse> GetPizzas(GetPizzasRequest request) =>
        await service.GetPizzas(request);
    
    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<GetPizzaOptionsResponse> GetPizzaOptions(GetPizzaOptionsRequest request) =>
        await service.GetPizzaOptions(request);

    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<CreatePizzaResponse> CreatePizza(CreatePizzaRequest request) =>
        await service.CreatePizza(request);

    [RequireRoles(UserRole.PizzeriaAccountUser)]
    [HttpPost("[action]")]
    public async Task<UpdatePizzaResponse> UpdatePizza(UpdatePizzaRequest request) =>
        await service.UpdatePizza(request);
}