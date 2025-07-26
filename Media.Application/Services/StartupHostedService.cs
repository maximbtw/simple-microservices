using System.Reflection;
using FluentMigrator.Runner;
using Media.Application.Infrastructure.Configuration;
using Media.Domain.Migrations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using Platform.Domain.Migration;

namespace Media.Application.Services;

public class StartupHostedService(IOptions<DatabaseOptions> databaseOptions) : IHostedService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.Info("Media is starting...");

        MigrateDatabase();

        Logger.Info("Media started.");
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void MigrateDatabase()
    {
        Assembly assemblyWithMigration = typeof(M0000_CreateDatabase).Assembly;

        var migrator = new Migrator(runnerBuilder => runnerBuilder.AddPostgres15_0()
            .WithGlobalConnectionString(databaseOptions.Value.ConnectionString)
            .ScanIn(assemblyWithMigration)
            .For.Migrations());

        migrator.MigrateUp();
    }
}