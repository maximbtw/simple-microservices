using System.ComponentModel.DataAnnotations;

namespace Platform.Core.DistributedCache.Configuration;

public class DistributedCacheRedisSettings
{
    [Required]
    public string Url { get; set; } = string.Empty;
}