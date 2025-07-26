using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Activate;
using PizzeriaAccounting.Contracts.AccountUser.Deactivate;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.AccountUser.Register;
using Platform.Client.Http;

namespace PizzeriaAccounting.Client;

public class UserServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IUserService
{
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request) =>
        await PostAsync<RegisterUserRequest, RegisterUserResponse>(request);

    public async Task<LoginUserResponse> Login(LoginUserRequest request) =>
        await PostAsync<LoginUserRequest, LoginUserResponse>(request);

    public async Task<GetUserResponse> GetUser(GetUserRequest request) =>
        await PostAsync<GetUserRequest, GetUserResponse>(request);

    public Task<ActivateUserResponse> Activate(ActivateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<DeactivateUserResponse> Deactivate(DeactivateUserRequest request)
    {
        throw new NotImplementedException();
    }
}