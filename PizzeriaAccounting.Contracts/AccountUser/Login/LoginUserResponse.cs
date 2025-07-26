using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.AccountUser.Login;

public class LoginUserResponse : OperationResponseBase<LoginUserResponseErrors>
{
    public AccountUserDto User { get; set; } = null!;
    
    public string Login  { get; set; } = null!;
    
    public string Token { get; set; } = null!;
}