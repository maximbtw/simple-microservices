using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.Account.GetAccounts;

public class GetAccountsResponseErrors : OperationResponseStandardErrors
{
    public bool AccountNotFound { get; set; }
}