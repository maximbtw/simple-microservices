using PizzeriaAccounting.Contracts.Persistence.Account;
using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.Account.GetAccounts;

public class GetAccountsResponse : OperationResponseBase<GetAccountsResponseErrors>
{
    public List<AccountDto> Accounts { get; set; } = new();
}