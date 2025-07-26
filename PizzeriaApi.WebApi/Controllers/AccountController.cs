using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Contracts.AccountApi;

namespace PizzeriaApi.WebApi.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController(IApiAccountService service) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAccounts() => await InvokeAsync(service.GetAccounts); 
}