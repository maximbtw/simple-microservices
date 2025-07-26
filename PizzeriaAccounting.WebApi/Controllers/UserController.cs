using Microsoft.AspNetCore.Mvc;
using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Activate;
using PizzeriaAccounting.Contracts.AccountUser.Deactivate;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.AccountUser.Register;
using Platform.Domain;
using Platform.WebApi;

namespace PizzeriaAccounting.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService service) : ControllerBase, IUserService
{
    [HttpPost("[action]")]
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request) => await service.Register(request);

    [HttpPost("[action]")]
    public async Task<LoginUserResponse> Login(LoginUserRequest request) => await service.Login(request);
    
    [HttpPost("[action]")]
    public async Task<GetUserResponse> GetUser(GetUserRequest request) => await service.GetUser(request);

    [RequireRoles(UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<ActivateUserResponse> Activate(ActivateUserRequest request) => await service.Activate(request);

    [RequireRoles(UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<DeactivateUserResponse> Deactivate(DeactivateUserRequest request) =>
        await service.Deactivate(request);
}