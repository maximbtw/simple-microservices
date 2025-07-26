using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.GetAccounts;
using PizzeriaApi.Contracts.AccountApi;
using PizzeriaApi.Contracts.AccountApi.GetAccounts;
using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Application.Services;

internal class ApiAccountService : IApiAccountService
{
    private readonly IAccountService _accountService;

    public ApiAccountService(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<ApiGetAccountsResponse> GetAccounts()
    {
        return await OperationInvoker
            .InvokeServiceAsync<GetAccountsResponse, GetAccountsResponseErrors, ApiGetAccountsResponse>(
                () => _accountService.GetAccounts(new GetAccountsRequest()),
                onSuccess: (s, a) =>
                {
                    a.Accounts = s.Accounts.ConvertAll(x =>
                        new Account(x.Id, x.Name, x.Address, x.Phone, x.Email, x.IsActive));
                });
    }
}