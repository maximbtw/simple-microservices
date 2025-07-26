using System.ComponentModel.DataAnnotations.Schema;
using Platform.Domain;
using Platform.Domain.EF;

namespace Auth.Domain.User;

[Table("Users")]
public class UserOrm : IOrm
{
    public int Id { get; set; }
    
    public int Version { get; set; }

    public string Login { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = null!;
    
    public UserRole Role { get; set; }
}