using Auth.Application.Persistence.User;
using Auth.Contracts;
using Auth.Contracts.Auth.GenerateInternalUserToken;
using Auth.Contracts.Persistence.User;
using Platform.Domain;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Platform.WebApi.Context;
using Utilities;

namespace Auth.Application.Services.Auth;

public class AuthServiceGenerateInternalUserTokenHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IDbScopeProvider _scopeProvider;
    private readonly ITokenProvider _tokenProvider;

    public AuthServiceGenerateInternalUserTokenHandler(
        IUserRepository userRepository,
        IDbScopeProvider scopeProvider,
        ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _scopeProvider = scopeProvider;
        _tokenProvider = tokenProvider;
    }

    public async Task<GenerateInternalUserTokenResponse> GenerateInternalUserToken(
        GenerateInternalUserTokenRequest request)
    {
        var response = new GenerateInternalUserTokenResponse();

        bool gotUserGuid = _tokenProvider.TryGetUserIdFromToken(request.JwtToken, out int userId);
        if (!gotUserGuid)
        {
            response.Errors.InvalidToken = true;
            return response;
        }

        await using OperationReaderScope scope = _scopeProvider.GetReaderScope();

        UserDto? user = await _userRepository.GetOrNull(userId, scope);
        if (user == null)
        {
            response.Errors.UserNotFound = true;
            return response;
        }

        var currentUser = new CurrentUser
        {
            Login = user.Login,
            Role = user.Role
        };

        response.InternalToken = SerializationHelper.SerializeToBase64(currentUser);
        response.Ok = true;

        return response;
    }
    
    private class CurrentUser : ICurrentUser
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;
        
        public UserRole Role { get; set; }
    }
}