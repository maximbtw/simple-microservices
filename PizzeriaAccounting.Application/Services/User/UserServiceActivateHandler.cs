using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.Activate;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Application.Services.User;

internal class UserServiceActivateHandler
{
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly IDbScopeProvider _dbScopeProvider;
    
    public UserServiceActivateHandler(IAccountUserRepository accountUserRepository, IDbScopeProvider dbScopeProvider)
    {
        _accountUserRepository = accountUserRepository;
        _dbScopeProvider = dbScopeProvider;
    }
    
    public Task<ActivateUserResponse> Activate(ActivateUserRequest request)
    {

        throw new NotImplementedException();
        /*var response = new ActivateUserResponse();

        await using OperationModificationScope scope = _dbScopeProvider.GetModificationScope();


        var parameters = new StandardUpdateDbEntityRequest<AccountUserDto>
        {

        }

        StandardUpdateDbEntityResponse<AccountUserDto> updateResult = await _dbAccountUserService.Update(parameters, scope);

        await scope.CommitChangesIfSucceeded(updateResult.Ok);

        response.Ok = updateResult.Ok;

        return response;*/
    }
}