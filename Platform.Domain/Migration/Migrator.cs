using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Platform.Domain.Migration;

public class Migrator
{
    private readonly IServiceProvider _serviceProvider;

    public Migrator(Action<IMigrationRunnerBuilder> configure)
    {
        _serviceProvider = new ServiceCollection()
            .AddFluentMigratorCore()
            .Configure<RunnerOptions>(options =>
            {
                options.Task = "migrate";
                options.Version = 0;
                options.Steps = 1;
            })
            .ConfigureRunner(configure)
            .AddLogging(builder => builder.AddFluentMigratorConsole())
            .BuildServiceProvider();
    }

    public void MigrateUp()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        
        var migrationRunner = scope.ServiceProvider.GetService<IMigrationRunner>();
        
        migrationRunner!.MigrateUp();
    }
}