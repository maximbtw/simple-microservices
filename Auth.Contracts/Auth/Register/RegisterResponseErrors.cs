using Platform.Core.Operations;

namespace Auth.Contracts.Auth.Register;

public class RegisterResponseErrors : OperationResponseStandardErrors
{
    public bool LoginExists { get; set; }
}