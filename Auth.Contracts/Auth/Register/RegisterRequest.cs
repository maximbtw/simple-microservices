using Platform.Domain;

namespace Auth.Contracts.Auth.Register;

public class RegisterRequest
{
    public string Login { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
}