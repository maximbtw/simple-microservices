using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Infrastructure.Configuration;

public class DatabaseOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}