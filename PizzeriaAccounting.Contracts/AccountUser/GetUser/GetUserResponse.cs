using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.Core.Operations;

namespace PizzeriaAccounting.Contracts.AccountUser.GetUser;

public class GetUserResponse : OperationResponseBase
{
    public AccountUserDto AccountUser { get; set; } = null!;
}