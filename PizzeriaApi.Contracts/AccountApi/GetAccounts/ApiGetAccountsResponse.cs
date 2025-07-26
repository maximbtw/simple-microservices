using PizzeriaApi.Contracts.Shared;

namespace PizzeriaApi.Contracts.AccountApi.GetAccounts;

public class ApiGetAccountsResponse : ApiResponseBase
{
    public List<Account> Accounts { get; set; } = new();
}