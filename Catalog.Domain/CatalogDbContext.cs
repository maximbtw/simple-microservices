using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;

namespace Catalog.Domain;

public class CatalogDbContext : DbContextBase
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IDbEventObserver observer) 
        : base(options, observer)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}