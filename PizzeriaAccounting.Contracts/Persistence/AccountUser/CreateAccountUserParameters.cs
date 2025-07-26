using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.AccountUser;

public class CreateAccountUserParameters : ICreateParameters<AccountUserDto>
{
    public AccountUserDto Entity { get; set; } = new();
}