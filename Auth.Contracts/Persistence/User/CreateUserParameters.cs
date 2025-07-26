using Platform.Core.Persistence;

namespace Auth.Contracts.Persistence.User;

public class CreateUserParameters : ICreateParameters<UserDto>
{
    public UserDto Entity { get; set; } = new();
}