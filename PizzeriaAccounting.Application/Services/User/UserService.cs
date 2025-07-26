using Auth.Contracts.Auth;
using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Activate;
using PizzeriaAccounting.Contracts.AccountUser.Deactivate;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaAccounting.Contracts.AccountUser.Login;
using PizzeriaAccounting.Contracts.AccountUser.Register;
using PizzeriaAccounting.Contracts.Persistence.Account;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Application.Services.User;

public class UserService : IUserService
{
    private readonly UserServiceRegisterHandler _registerHandler;
    private readonly UserServiceLoginHandler _loginHandler;
    private readonly UserServiceGetUserHandler _getUserHandler;
    private readonly UserServiceActivateHandler _activateHandler;
    private readonly UserServiceDeactivateHandler _deactivateHandler;

    public UserService(
        IAccountUserRepository accountUserRepository,
        IAccountRepository accountRepository,
        IAuthService authService,
        IDbScopeProvider dbScopeProvider)
    {
        _registerHandler = new UserServiceRegisterHandler(
            accountUserRepository, 
            accountRepository, 
            authService, 
            dbScopeProvider);

        _loginHandler = new UserServiceLoginHandler(accountUserRepository, authService, dbScopeProvider);
        _getUserHandler = new UserServiceGetUserHandler(accountUserRepository, dbScopeProvider);

        _activateHandler = new UserServiceActivateHandler(accountUserRepository, dbScopeProvider);
        _deactivateHandler = new UserServiceDeactivateHandler(accountUserRepository, dbScopeProvider);
    }

    public async Task<RegisterUserResponse> Register(RegisterUserRequest request) =>
        await _registerHandler.Register(request);

    public async Task<LoginUserResponse> Login(LoginUserRequest request) => await _loginHandler.Login(request);
    
    public async Task<GetUserResponse> GetUser(GetUserRequest request) => await _getUserHandler.GetUser(request);

    public async Task<ActivateUserResponse> Activate(ActivateUserRequest request) =>
        await _activateHandler.Activate(request);

    public async Task<DeactivateUserResponse> Deactivate(DeactivateUserRequest request) =>
        await _deactivateHandler.Deactivate(request);
}