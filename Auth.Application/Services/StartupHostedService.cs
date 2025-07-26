using System.Diagnostics;
using System.Reflection;
using Auth.Application.Infrastructure.Configuration;
using Auth.Domain.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using Platform.Domain.Migration;

namespace Auth.Application.Services;

public class StartupHostedService(IOptions<DatabaseOptions> databaseOptions) : IHostedService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.Info("Auth is starting...");
        var sw = Stopwatch.StartNew();
        
        MigrateDatabase();

        sw.Stop();
        Logger.Info("Auth started.");

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