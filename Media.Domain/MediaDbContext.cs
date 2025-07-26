using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;

namespace Media.Domain;

public class MediaDbContext : DbContextBase
{
    public MediaDbContext(DbContextOptions<MediaDbContext> options, IDbEventObserver observer) 
        : base(options, observer)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}