using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.Account;

public class UpdateAccountParameters : IUpdateParameters<AccountDto>
{
    public AccountDto Entity { get; set; } = new();
}