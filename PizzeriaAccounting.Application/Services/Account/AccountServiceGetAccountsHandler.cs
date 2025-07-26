using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.GetAccounts;
using PizzeriaAccounting.Contracts.Persistence.Account;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Services.Account;

internal class AccountServiceGetAccountsHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly IDbScopeProvider _scopeProvider;

    public AccountServiceGetAccountsHandler(
        IAccountRepository accountRepository,
        IDbScopeProvider scopeProvider)
    {
        _accountRepository = accountRepository;
        _scopeProvider = scopeProvider;
    }

    public async Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request)
    {
        var response = new GetAccountsResponse();

        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();

        if (request.AccountId == null)
        {
            List<AccountDto> accounts = await _accountRepository.GetAll(scope);

            response.Accounts = accounts;
            response.Ok = true;
            return response;
        }

        AccountDto? account = await _accountRepository.GetOrNull((int)request.AccountId, scope);
        if (account == null)
        {
            response.Errors.AccountNotFound = true;
            return response;
        }
        
        response.Accounts = [account];
        response.Ok = true;

        return response;
    }
    
}