using System.Reflection;
using Auth.Domain.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using PizzeriaAccounting.Application.Infrastructure.Configuration;
using Platform.Domain.Migration;

namespace PizzeriaAccounting.Application.Services;

public class StartupHostedService(IOptions<DatabaseOptions> databaseOptions) : IHostedService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.Info("PizzeriaAccounting is starting...");

        MigrateDatabase();

        Logger.Info("PizzeriaAccounting started.");
        
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