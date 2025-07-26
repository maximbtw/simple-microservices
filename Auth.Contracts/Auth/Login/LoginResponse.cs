using Auth.Contracts.Persistence.User;
using Platform.Core.Operations;

namespace Auth.Contracts.Auth.Login;

public class LoginResponse : OperationResponseBase<LoginResponseErrors>
{
    public string Token { get; set; } = null!;

    public UserDto User { get; set; } = null!;
}