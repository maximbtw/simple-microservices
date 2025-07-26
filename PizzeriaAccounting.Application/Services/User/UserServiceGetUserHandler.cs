using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Contracts.AccountUser.GetUser;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Services.User;

public class UserServiceGetUserHandler
{
    private readonly IAccountUserRepository _accountUserRepository;
    private readonly IDbScopeProvider _dbScopeProvider;
    
    public UserServiceGetUserHandler(IAccountUserRepository accountUserRepository, IDbScopeProvider dbScopeProvider)
    {
        _accountUserRepository = accountUserRepository;
        _dbScopeProvider = dbScopeProvider;
    }
    
    public async Task<GetUserResponse> GetUser(GetUserRequest request)
    {
        var response = new GetUserResponse();

        await using OperationReaderScope scope = _dbScopeProvider.GetReaderScope();
        
        AccountUserDto? accountUser = await _accountUserRepository.GetOrNullByUserId(request.UserId, scope);
        if (accountUser == null)
        {
            return response;
        }
        
        response.AccountUser = accountUser;
        response.Ok = true;
        
        return response;
    }
}