using Auth.Contracts.Persistence.User;
using Auth.Domain.User;
using Platform.Core.Persistence;

namespace Auth.Application.Persistence.User;

internal class UserMapper : IMapper<UserOrm, UserDto>
{
    public UserDto MapToDto(UserOrm orm) => new()
    {
        Id = orm.Id,
        Version = orm.Version,
        Login = orm.Login,
        Role = orm.Role
    };

    public void UpdateOrmFromDto(UserOrm orm, UserDto dto)
    {
        orm.Id = dto.Id;
        orm.Version = dto.Version;
        orm.PasswordHash = dto.PasswordHash!;
        orm.Login = dto.Login;
        orm.Role = dto.Role;
    }
}