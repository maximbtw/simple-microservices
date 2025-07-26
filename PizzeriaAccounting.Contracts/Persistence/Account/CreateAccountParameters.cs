using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.Account;

public class CreateAccountParameters : ICreateParameters<AccountDto>
{
    public AccountDto Entity { get; set; } = new();
}