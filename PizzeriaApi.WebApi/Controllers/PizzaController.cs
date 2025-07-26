using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Contracts.PizzaApi;
using PizzeriaApi.Contracts.PizzaApi.Create;
using PizzeriaApi.Contracts.PizzaApi.Update;

namespace PizzeriaApi.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("pizzas")]
public class PizzaController(IApiPizzaService service) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get() => await InvokeAsync(service.GetPizzas);
    
    [HttpGet("create-options")]
    public async Task<IActionResult> GetCreateOptions([FromQuery] int? copyFromId) =>
        await InvokeAsync(() => service.GetCreateOptions(copyFromId));
    
    [HttpGet("{id:int}/update-options")]
    public async Task<IActionResult> GetUpdateOptions([FromRoute] int id) =>
        await InvokeAsync(() => service.GetUpdateOptions(id));
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApiCreatePizzaFormModel model) =>
        await InvokeAsync(model, service.Create);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ApiUpdatePizzaFormModel model)
    {
        model.Id = id;
        return await InvokeAsync(model, service.Update);
    }
}