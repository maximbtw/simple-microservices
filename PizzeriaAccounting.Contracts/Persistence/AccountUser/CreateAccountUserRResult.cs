using Platform.Core.Persistence;

namespace PizzeriaAccounting.Contracts.Persistence.AccountUser;

public class CreateAccountUserRResult :  ICreateResult<AccountUserDto>
{
    public bool Ok => !this.UserWithSameEmailExists && this.CreatedEntity != null;
    
    public AccountUserDto? CreatedEntity { get; set; }
    
    public bool UserWithSameEmailExists { get; set; }
}