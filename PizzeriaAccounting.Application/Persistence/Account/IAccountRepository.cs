using PizzeriaAccounting.Contracts.Persistence.Account;
using Platform.Domain.EF.Transactions;

namespace PizzeriaAccounting.Application.Persistence.Account;

public interface IAccountRepository
{
    Task<AccountDto?> GetOrNull(int id, OperationScopeBase scope);
    
    Task<CreateAccountResult> Create(CreateAccountParameters parameters, OperationModificationScope scope);
    
    Task<List<AccountDto>> GetAll(OperationReaderScope scope);
}