using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.Infrastructure.Configuration;

public class DatabaseOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}