using Platform.Core.Operations;

namespace Auth.Contracts.Auth.Login;

public class LoginResponseErrors : OperationResponseStandardErrors
{
    public bool InvalidPasswordOrLogin { get; set; }
}