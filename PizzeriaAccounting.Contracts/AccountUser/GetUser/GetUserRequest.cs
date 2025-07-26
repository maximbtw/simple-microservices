using Platform.Core.DistributedCache;

namespace PizzeriaAccounting.Contracts.AccountUser.GetUser;

public class GetUserRequest : IDistributedCacheKey
{
    public int UserId { get; set; }
    
    public TimeSpan CachedValueRelevanceInterval { get; set; }
}