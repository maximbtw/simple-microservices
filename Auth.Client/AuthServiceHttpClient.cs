using Auth.Contracts.Auth;
using Auth.Contracts.Auth.GenerateInternalUserToken;
using Auth.Contracts.Auth.Login;
using Auth.Contracts.Auth.Register;
using Platform.Client.Http;

namespace Auth.Client;

public class AuthServiceHttpClient(HttpClient httpClient, IHttpClientObserver? observer = null)
    : HttpClientBase(httpClient, null, observer), IAuthService
{
    public async Task<RegisterResponse> Register(RegisterRequest request) =>
        await PostAsync<RegisterRequest, RegisterResponse>(request);

    public async Task<LoginResponse> Login(LoginRequest request) =>
        await PostAsync<LoginRequest, LoginResponse>(request);

    public async Task<GenerateInternalUserTokenResponse> GenerateInternalUserToken(GenerateInternalUserTokenRequest request) =>
        await PostAsync<GenerateInternalUserTokenRequest, GenerateInternalUserTokenResponse>(request);
}