using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;

namespace PizzeriaAccounting.Domain;

public class PizzeriaAccountingDbContext : DbContextBase
{
    public PizzeriaAccountingDbContext(DbContextOptions<PizzeriaAccountingDbContext> options, IDbEventObserver observer) 
        : base(options, observer)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}