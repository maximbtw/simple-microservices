namespace Auth.Application.Infrastructure.Configuration;

public class JwtOptions
{
    public string JwtSecretKey { get; set; } = string.Empty;
    
    public int ExpirationHours { get; set; }
}