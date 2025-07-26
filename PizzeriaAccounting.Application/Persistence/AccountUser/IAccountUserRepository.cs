using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Persistence.AccountUser;

public interface IAccountUserRepository
{
    Task<AccountUserDto?> GetOrNullByUserId(int userId, OperationScopeBase scope);
    
    Task<CreateAccountUserRResult> Create(CreateAccountUserParameters parameters, OperationModificationScope scope);
}