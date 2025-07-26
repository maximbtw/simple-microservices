using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.AccountUser.Login;

public class LoginUserResponseErrors : OperationResponseStandardErrors
{
    public bool InvalidPasswordOrLogin { get; set; }
    
    public bool UserNotActive { get; set; }
}