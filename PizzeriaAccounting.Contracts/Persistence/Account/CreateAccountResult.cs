using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.Account;

public class CreateAccountResult : ICreateResult<AccountDto>
{
    public bool Ok => !this.SameAddressExists && !this.SameNameExists && this.CreatedEntity != null;
    
    public AccountDto? CreatedEntity { get; set; } = null!;
    
    public bool SameNameExists { get; set; }
    
    public bool SameAddressExists { get; set; }
}