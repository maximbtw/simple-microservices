using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.Create;
using PizzeriaAccounting.Contracts.Account.GetAccounts;
using PizzeriaAccounting.Contracts.Persistence.Account;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Application.Services.Account;

public class AccountService : IAccountService
{
    private readonly AccountServiceGetAccountsHandler _getAccountsHandler;
    private readonly AccountServiceCreateAccountHandler _createAccountHandler;
    
    public AccountService(
        IAccountRepository accountRepository,
        IDbScopeProvider scopeProvider)
    {
        _getAccountsHandler = new AccountServiceGetAccountsHandler(accountRepository, scopeProvider);
        _createAccountHandler = new AccountServiceCreateAccountHandler(accountRepository, scopeProvider);
    }

    public async Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request) =>
        await _getAccountsHandler.GetAccounts(request);

    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request) =>
        await _createAccountHandler.CreateAccount(request);
}