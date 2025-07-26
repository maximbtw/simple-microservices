using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.Account.Create;
using PizzeriaAccounting.Contracts.Account.GetAccounts;
using Platform.Client.Http;

namespace PizzeriaAccounting.Client;

public class AccountServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IAccountService
{
    public async Task<GetAccountsResponse> GetAccounts(GetAccountsRequest request) =>
        await PostAsync<GetAccountsRequest, GetAccountsResponse>(request);

    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request) =>
        await PostAsync<CreateAccountRequest, CreateAccountResponse>(request);
}