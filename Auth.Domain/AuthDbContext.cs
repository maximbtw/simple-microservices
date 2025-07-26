using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;

namespace Auth.Domain;

public class AuthDbContext : DbContextBase
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options, IDbEventObserver observer) 
        : base(options, observer)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}