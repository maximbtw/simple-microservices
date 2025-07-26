using PizzeriaAccounting.Contracts.Account.Create;
using PizzeriaAccounting.Contracts.Account.GetAccounts;

namespace PizzeriaAccounting.Contracts.Account;

public interface IAccountService
{
    Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request);
    
    Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);
}