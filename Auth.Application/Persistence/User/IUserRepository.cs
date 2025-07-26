using Auth.Contracts.Persistence.User;
using Platform.Domain.EF.Transactions;

namespace Auth.Application.Persistence.User;

public interface IUserRepository
{
    Task<CreateUserResult> Create(CreateUserParameters parameters, OperationModificationScope scope);
    
    Task<UserDto?> GetOrNull(string login, string password, OperationScopeBase scope);
    
    Task<UserDto?> GetOrNull(int id, OperationScopeBase scope);
}