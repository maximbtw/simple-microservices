using Auth.Contracts.Auth;
using Auth.Contracts.Auth.Login;
using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Core.Operations;
using Platform.Domain;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Services.User;

internal class UserServiceLoginHandler
{
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly IAuthService _authService;
    private readonly IDbScopeProvider _dbScopeProvider;

    public UserServiceLoginHandler(
        IAccountUserRepository accountUserRepository,
        IAuthService authService,
        IDbScopeProvider dbScopeProvider)
    {
        _accountUserRepository = accountUserRepository;
        _authService = authService;
        _dbScopeProvider = dbScopeProvider;
    }

    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        var response = new LoginUserResponse();

        var loginRequest = new LoginRequest
        {
            Login = request.Login,
            Password = request.Password
        };

        LoginResponse loginResponse = await _authService.Login(loginRequest);
        if (!loginResponse.Ok)
        {
            response.Errors.InvalidPasswordOrLogin = loginResponse.Errors.InvalidPasswordOrLogin;
            return response;
        }

        if (loginResponse.User.Role != UserRole.PizzeriaAccountUser)
        {
            response.Errors.AccessDenied = true;
            response.Errors.AccessDeniedReason = AccessDeniedReason.OperationAccessDenied;
            
            return response;
        }

        await using OperationModificationScope scope = _dbScopeProvider.GetModificationScope();

        AccountUserDto? accountUser = await _accountUserRepository.GetOrNullByUserId(loginResponse.User.Id, scope);
        if (accountUser == null)
        {
            response.Errors.InvalidPasswordOrLogin = true;
            return response;
        }

        if (!accountUser.IsActive)
        {
            response.Errors.UserNotActive = true;
            return response;
        }
        
        response.Ok = true;
        response.User = accountUser;
        response.Login = loginResponse.User.Login;
        response.Token = loginResponse.Token;

        return response;
    }
}