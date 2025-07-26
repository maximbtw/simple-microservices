using Microsoft.EntityFrameworkCore;

namespace Platform.Domain.EF;

public abstract class DbContextBase : DbContext
{
    private readonly IDbEventObserver _observer;

    protected DbContextBase(DbContextOptions options, IDbEventObserver observer) 
        : base(options)
    {
        _observer = observer;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseLoggerFactory(DbContextLoggerProvider.GetFactory(_observer))
            .EnableSensitiveDataLogging();
    }
}