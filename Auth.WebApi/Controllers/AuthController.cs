using System.Text.Json;
using Auth.Contracts.Auth;
using Auth.Contracts.Auth.GenerateInternalUserToken;
using Auth.Contracts.Auth.Login;
using Auth.Contracts.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService service) : ControllerBase, IAuthService
{
    [HttpPost]
    public IActionResult Test([FromBody] JsonElement data)
    {
        // data — это весь JSON из тела запроса
        Console.WriteLine(data.ToString());
        return Ok();
    }
    
    [HttpPost("[action]")]
    public async Task<RegisterResponse> Register(RegisterRequest request) => await service.Register(request);

    [HttpPost("[action]")]
    public async Task<LoginResponse> Login(LoginRequest request) => await service.Login(request);
    
    #if DEBUG
    [HttpPost("[action]")]
    public async Task<GenerateInternalUserTokenResponse> GenerateInternalUserToken(
        GenerateInternalUserTokenRequest request) =>
        await service.GenerateInternalUserToken(request);
    #endif
}