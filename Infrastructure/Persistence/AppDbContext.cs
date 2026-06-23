using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<NotificationSettings> NotificationSettings => Set<NotificationSettings>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BillingCycle> BillingCycles => Set<BillingCycle>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Email).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(x => x.Timezone).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasMany(x => x.Subscriptions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.NotificationSettings)
                .WithOne(x => x.User)
                .HasForeignKey<NotificationSettings>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.ExpiresAt).IsRequired();
            entity.Property(x => x.ReplacedByTokenHash).HasMaxLength(128);
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NotificationSettings>(entity =>
        {
            entity.ToTable("NotificationSettings");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.UserId).IsUnique();
            entity.Property(x => x.Enabled).IsRequired();
            entity.Property(x => x.ReminderDaysBefore).IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Icon).HasMaxLength(80).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<BillingCycle>(entity =>
        {
            entity.ToTable("BillingCycles");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.IntervalTypeValue).HasConversion<int>().IsRequired();
            entity.Property(x => x.IntervalValue).IsRequired();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("Subscriptions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Provider).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Cost).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            entity.Property(x => x.Notes).HasMaxLength(1000);
            entity.HasOne(x => x.Category)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.BillingCycle)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.BillingCycleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(x => x.Notifications)
                .WithOne(x => x.Subscription)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).HasConversion<int>().IsRequired();
            entity.Property(x => x.Message).HasMaxLength(500).IsRequired();
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var settingsId = Guid.Parse("11111111-1111-1111-1111-111111111112");
        var streamingId = Guid.Parse("22222222-2222-2222-2222-222222222221");
        var softwareId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var utilitiesId = Guid.Parse("22222222-2222-2222-2222-222222222223");
        var monthlyId = Guid.Parse("33333333-3333-3333-3333-333333333331");
        var yearlyId = Guid.Parse("33333333-3333-3333-3333-333333333332");
        var weeklyId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var netflixId = Guid.Parse("44444444-4444-4444-4444-444444444441");
        var githubId = Guid.Parse("44444444-4444-4444-4444-444444444442");
        var notificationId = Guid.Parse("55555555-5555-5555-5555-555555555551");

        modelBuilder.Entity<User>().HasData(new
        {
            Id = userId,
            Email = "demo@subscription.local",
            Name = "Demo User",
            PasswordHash = "demo-password-hash",
            Timezone = "Europe/Berlin"
        });

        modelBuilder.Entity<NotificationSettings>().HasData(new
        {
            Id = settingsId,
            UserId = userId,
            Enabled = true,
            ReminderDaysBefore = 7
        });

        modelBuilder.Entity<Category>().HasData(
            new { Id = streamingId, Name = "Streaming", Icon = "tv" },
            new { Id = softwareId, Name = "Software", Icon = "code" },
            new { Id = utilitiesId, Name = "Utilities", Icon = "bolt" });

        modelBuilder.Entity<BillingCycle>().HasData(
            new { Id = monthlyId, IntervalTypeValue = IntervalType.Month, IntervalValue = 1 },
            new { Id = yearlyId, IntervalTypeValue = IntervalType.Year, IntervalValue = 1 },
            new { Id = weeklyId, IntervalTypeValue = IntervalType.Week, IntervalValue = 1 });

        modelBuilder.Entity<Subscription>().HasData(
            new
            {
                Id = netflixId,
                UserId = userId,
                Name = "Netflix",
                Provider = "Netflix",
                CategoryId = streamingId,
                Cost = 12.99m,
                Currency = "EUR",
                BillingCycleId = monthlyId,
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = (DateTime?)null,
                CancellationDeadline = new DateTime(2026, 12, 24, 0, 0, 0, DateTimeKind.Utc),
                AutoRenew = true,
                Notes = "Demo-Abo",
                IsActive = true
            },
            new
            {
                Id = githubId,
                UserId = userId,
                Name = "GitHub Pro",
                Provider = "GitHub",
                CategoryId = softwareId,
                Cost = 48.00m,
                Currency = "EUR",
                BillingCycleId = yearlyId,
                StartDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = (DateTime?)null,
                CancellationDeadline = new DateTime(2027, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                AutoRenew = true,
                Notes = "Entwickler-Tools",
                IsActive = true
            });

        modelBuilder.Entity<Notification>().HasData(new
        {
            Id = notificationId,
            SubscriptionId = netflixId,
            Type = NotificationType.PaymentReminder,
            Message = "Netflix Zahlung steht bald an.",
            ScheduledFor = new DateTime(2026, 2, 24, 9, 0, 0, DateTimeKind.Utc)
        });
    }
}
