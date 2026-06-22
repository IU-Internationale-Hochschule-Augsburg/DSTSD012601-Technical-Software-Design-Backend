using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Infrastructure.Persistence;

namespace Subscription_Control_Backend.Infrastructure.Services;

public class DatabaseStartupService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DatabaseStartupService> _logger;

    public DatabaseStartupService(
        IServiceScopeFactory scopeFactory,
        ILogger<DatabaseStartupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Es gibt (noch) keine EF-Migrations: Schema und Seed-Daten werden direkt erstellt.
        // Sobald Migrations eingeführt werden, hier auf MigrateAsync umstellen.
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        _logger.LogInformation("Database schema and seed data are ready.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}