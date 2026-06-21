using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Constants;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Services;

public class DatabaseStartupService : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DatabaseStartupService> _logger;

    public DatabaseStartupService(
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory,
        ILogger<DatabaseStartupService> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await EnsureDatabaseExistsAsync(cancellationToken);

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        _logger.LogInformation("Database schema and seed data are ready.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task EnsureDatabaseExistsAsync(CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("MasterConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogWarning("MasterConnection wurde nicht gefunden. Datenbank-Erstellung wird übersprungen.");
            return;
        }

        const string databaseName = DatabaseConstants.DbName;
        var sql = $@"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}')
BEGIN
    CREATE DATABASE [{databaseName}]
END";

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation("Database check completed for {DatabaseName}.", databaseName);
    }
}
