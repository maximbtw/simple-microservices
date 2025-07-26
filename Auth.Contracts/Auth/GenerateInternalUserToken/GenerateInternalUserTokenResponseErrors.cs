using Platform.Core.Operations;

namespace Auth.Contracts.Auth.GenerateInternalUserToken;

public class GenerateInternalUserTokenResponseErrors : OperationResponseStandardErrors
{
    public bool InvalidToken { get; set; }
    
    public bool UserNotFound { get; set; }
}