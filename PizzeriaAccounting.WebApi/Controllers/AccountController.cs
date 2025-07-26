using Microsoft.AspNetCore.Mvc;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.Create;
using PizzeriaAccounting.Contracts.Account.GetAccounts;
using Platform.Domain;
using Platform.WebApi;

namespace PizzeriaAccounting.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService service) : ControllerBase, IAccountService
{
    [HttpPost("[action]")]
    public async Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request) =>
        await service.GetAccounts(request);
    
    [RequireRoles(UserRole.Admin)]
    [HttpPost("[action]")]
    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request) =>
        await service.CreateAccount(request);
}