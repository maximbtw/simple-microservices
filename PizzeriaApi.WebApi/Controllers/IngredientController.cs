using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Contracts.IngredientApi;
using PizzeriaApi.Contracts.IngredientApi.Create;
using PizzeriaApi.Contracts.IngredientApi.Update;

namespace PizzeriaApi.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("ingredients")]
public class IngredientController(IApiIngredientService service) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get() => await InvokeAsync(service.GetIngredients);

    [HttpGet("create-options")]
    public async Task<IActionResult> GetCreateOptions([FromQuery] int? copyFromId) =>
        await InvokeAsync(() => service.GetCreateOptions(copyFromId));

    [HttpGet("{id:int}/update-options")]
    public async Task<IActionResult> GetUpdateOptions([FromRoute] int id) =>
        await InvokeAsync(() => service.GetUpdateOptions(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApiCreateIngredientFormModel model) =>
        await InvokeAsync(model, service.Create);

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ApiUpdateIngredientFormModel model)
    {
        model.Id = id;
        return await InvokeAsync(model, service.Update);
    }
}