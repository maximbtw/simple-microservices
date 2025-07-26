using Auth.Contracts.Auth.GenerateInternalUserToken;
using Auth.Contracts.Auth.Login;
using Auth.Contracts.Auth.Register;

namespace Auth.Contracts.Auth;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest request);
    
    Task<LoginResponse> Login(LoginRequest request);

#if DEBUG
    Task<GenerateInternalUserTokenResponse> GenerateInternalUserToken(GenerateInternalUserTokenRequest request);
#endif
}