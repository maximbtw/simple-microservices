using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Contracts.AuthApi;
using PizzeriaApi.Contracts.AuthApi.Login;
using PizzeriaApi.Contracts.AuthApi.Register;

namespace PizzeriaApi.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IApiAuthService service) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(ApiRegisterRequest request) =>
        await InvokeAsync(request, service.Register);

    [HttpPost("login")]
    public async Task<IActionResult> Login(ApiLoginRequest request) => await InvokeAsync(request, service.Login);
}