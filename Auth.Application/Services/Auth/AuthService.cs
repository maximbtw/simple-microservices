using Auth.Application.Persistence.User;
using Auth.Contracts;
using Auth.Contracts.Auth;
using Auth.Contracts.Auth.GenerateInternalUserToken;
using Auth.Contracts.Auth.Login;
using Auth.Contracts.Auth.Register;
using Auth.Contracts.Persistence.User;
using Platform.Domain.EF;

namespace Auth.Application.Services.Auth;

internal class AuthService : IAuthService
{
    private readonly AuthServiceLoginHandler _loginHandler;
    private readonly AuthServiceRegisterHandler _registerHandler;
    private readonly AuthServiceGenerateInternalUserTokenHandler _generateInternalUserTokenHandler;
    
    public AuthService(        
        IUserRepository userRepository, 
        ITokenProvider tokenProvider, 
        IDbScopeProvider scopeProvider)
    {
        _loginHandler = new AuthServiceLoginHandler(userRepository, tokenProvider, scopeProvider);
        _registerHandler = new AuthServiceRegisterHandler(userRepository, scopeProvider);
        
        _generateInternalUserTokenHandler = new AuthServiceGenerateInternalUserTokenHandler(
            userRepository, 
            scopeProvider, 
            tokenProvider);
    }
    
    public async Task<RegisterResponse> Register(RegisterRequest request) => await _registerHandler.Register(request);

    public async Task<LoginResponse> Login(LoginRequest request) => await _loginHandler.Login(request);

    public async Task<GenerateInternalUserTokenResponse> GenerateInternalUserToken(GenerateInternalUserTokenRequest request) =>
        await _generateInternalUserTokenHandler.GenerateInternalUserToken(request);
}