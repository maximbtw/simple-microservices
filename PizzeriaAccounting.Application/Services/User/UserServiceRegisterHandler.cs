using Auth.Contracts.Auth;
using Auth.Contracts.Auth.Register;
using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.AccountUser.Register;
using PizzeriaAccounting.Contracts.Persistence.Account;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Services.User;

internal class UserServiceRegisterHandler
{
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthService _authService;
    private readonly IDbScopeProvider _dbScopeProvider;

    public UserServiceRegisterHandler(
        IAccountUserRepository accountUserRepository,
        IAccountRepository accountRepository,
        IAuthService authService,
        IDbScopeProvider dbScopeProvider)
    {
        _accountUserRepository = accountUserRepository;
        _accountRepository = accountRepository;
        _authService = authService;
        _dbScopeProvider = dbScopeProvider;
    }

    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        var response = new RegisterUserResponse();

        var registerRequest = new RegisterRequest
        {
            Login = request.Login,
            Password = request.Password,
            Role = UserRole.PizzeriaAccountUser
        };

        RegisterResponse registerResponse = await _authService.Register(registerRequest);
        if (!registerResponse.Ok)
        {
            response.Errors.UserWithSameLoginExists = registerResponse.Errors.LoginExists;
            return response;
        }

        await using OperationModificationScope scope = _dbScopeProvider.GetModificationScope();

        AccountDto? account = await _accountRepository.GetOrNull(request.AccountId, scope);
        if (account == null)
        {
            response.Errors.AccountNotActive = true;
            return response;
        }

        if (!account.IsActive)
        {
            response.Errors.AccountNotActive = true;
            return response;
        }

        var createParameters = new CreateAccountUserParameters
        {
            Entity = new AccountUserDto
            {
                IsActive = true, // TODO: временно
                UserId = registerResponse.User.Id,
                Email = request.Email,
                AccountId = account.Id
            }
        };

        CreateAccountUserRResult createResult = await _accountUserRepository.Create(createParameters, scope);

        if (!createResult.Ok)
        {
            response.Errors.UserWithSameEmailExists = createResult.UserWithSameEmailExists;
            return response;
        }

        await scope.CommitChangesIfSucceededAsync(createResult.Ok);
        response.Ok = true;

        return response;
    }
}