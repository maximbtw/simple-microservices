using Auth.Contracts.Persistence.User;
using Platform.Core.Operations;

namespace Auth.Contracts.Auth.Register;

public class RegisterResponse : OperationResponseBase<RegisterResponseErrors>
{
    public UserDto User { get; set; } = null!;
}