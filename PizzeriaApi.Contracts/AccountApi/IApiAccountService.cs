using PizzeriaApi.Contracts.AccountApi.GetAccounts;

namespace PizzeriaApi.Contracts.AccountApi;

public interface IApiAccountService
{
    Task<ApiGetAccountsResponse> GetAccounts();
}