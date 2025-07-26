using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Deactivate;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Application.Services.User;

internal class UserServiceDeactivateHandler
{
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly IDbScopeProvider _dbScopeProvider;
    
    public UserServiceDeactivateHandler(IAccountUserRepository accountUserRepository, IDbScopeProvider dbScopeProvider)
    {
        _accountUserRepository = accountUserRepository;
        _dbScopeProvider = dbScopeProvider;
    }
    
    public Task<DeactivateUserResponse> Deactivate(DeactivateUserRequest request)
    {
        throw new NotImplementedException();
    }
}