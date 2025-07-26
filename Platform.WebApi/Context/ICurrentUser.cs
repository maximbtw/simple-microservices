using Platform.Domain;

namespace Platform.WebApi.Context;

public interface ICurrentUser
{
    public int Id { get; set; }
    
    public string Login { get; set; }
    
    public UserRole Role { get; set; }
}