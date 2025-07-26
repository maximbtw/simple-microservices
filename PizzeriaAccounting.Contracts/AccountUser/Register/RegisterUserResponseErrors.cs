using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.AccountUser.Register;

public class RegisterUserResponseErrors : OperationResponseStandardErrors
{
    public bool UserWithSameLoginExists { get; set; }
    
    public bool UserWithSameEmailExists { get; set; }
    
    public bool AccountNotFound { get; set; }
    
    public bool AccountNotActive { get; set; }
}