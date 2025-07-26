using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.Account.Create;

public class CreateAccountResponseErrors : OperationResponseStandardErrors
{
    public bool AccountWithSameNameExists { get; set; }
    
    public bool AccountWithSameAddressExists { get; set; }
}