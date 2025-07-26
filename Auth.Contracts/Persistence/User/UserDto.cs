using Platform.Core.Persistence;
using Platform.Domain;

namespace Auth.Contracts.Persistence.User;

public class UserDto : IEntityDto
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Login { get; set; } = string.Empty;

    public byte[]? PasswordHash { get; set; }
    
    public UserRole Role { get; set; }
}