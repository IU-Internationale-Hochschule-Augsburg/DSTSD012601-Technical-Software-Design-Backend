using Microsoft.EntityFrameworkCore;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Application.Services;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;
using Subscription_Control_Backend.Infrastructure.Repositories;
using Subscription_Control_Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

const string corsPolicy = "_subscriptionControlCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:8011", "http://localhost:3000", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' wurde nicht gefunden.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddHostedService<DatabaseStartupService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBillingCycleService, BillingCycleService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationSettingsService, NotificationSettingsService>();
builder.Services.AddScoped<IExampleService, ExampleService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
