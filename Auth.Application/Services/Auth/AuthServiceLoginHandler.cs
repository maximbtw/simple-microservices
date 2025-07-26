using System.Collections.Frozen;
using Auth.Application.Persistence.User;
using Auth.Contracts;
using Auth.Contracts.Auth.Login;
using Auth.Contracts.Persistence.User;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Auth.Application.Services.Auth;

internal class AuthServiceLoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IDbScopeProvider _scopeProvider;
    
    public AuthServiceLoginHandler(
        IUserRepository userRepository, 
        ITokenProvider tokenProvider, 
        IDbScopeProvider scopeProvider)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _scopeProvider = scopeProvider;
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var response = new LoginResponse();
        
        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();
        
        UserDto? user = await _userRepository.GetOrNull(request.Login, request.Password, scope);
        if (user == null)
        {
            response.Errors.InvalidPasswordOrLogin = true;

            return response;
        }
        
        response.Token = _tokenProvider.GenerateToken(user);
        response.User = user;
        response.Ok = true;
        
        return response;
    }
}