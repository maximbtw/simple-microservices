using Platform.Core.Persistence;

namespace Auth.Contracts.Persistence.User;

public class CreateUserResult : ICreateResult<UserDto>
{
    public bool Ok => !this.UserWithSameLoginExists && this.CreatedEntity != null;
    
    public UserDto? CreatedEntity { get; set; }
    
    public bool UserWithSameLoginExists { get; set; }
}