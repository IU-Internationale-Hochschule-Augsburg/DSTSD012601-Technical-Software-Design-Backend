using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Subscription_Control_Backend.Api.OpenApi;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Application.Options;
using Subscription_Control_Backend.Application.Services;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Infrastructure.Persistence;
using Subscription_Control_Backend.Infrastructure.Repositories;
using Subscription_Control_Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiDocumentInfoTransformer>();
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddProblemDetails();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                 ?? throw new InvalidOperationException("Keine JWT-Konfiguration vorhanden (Abschnitt 'Jwt').");
if (string.IsNullOrWhiteSpace(jwtOptions.Key))
{
    throw new InvalidOperationException("Kein JWT-Signaturschlüssel konfiguriert (Jwt:Key bzw. Jwt__Key).");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

const string corsPolicy = "_subscriptionControlCors";
var corsOptions = builder.Configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>()
                  ?? new CorsOptions();
var allowedOrigins = corsOptions.AllowedOrigins
    .SelectMany(origin => origin.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .ToArray();
if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "Keine CORS-Origins konfiguriert (Cors:AllowedOrigins bzw. Cors__AllowedOrigins__0 / CORS_ALLOWED_ORIGINS).");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.
            WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var connectionString = BuildConnectionString(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHostedService<DatabaseStartupService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<INotificationSettingsRepository, NotificationSettingsRepository>();

builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBillingCycleService, BillingCycleService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationSettingsService, NotificationSettingsService>();
builder.Services.AddScoped<IExampleService, ExampleService>();

var app = builder.Build();

app.UseExceptionHandler();

// OpenAPI-Spezifikation unter /openapi/v1.json und Swagger-UI unter /swagger.
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Subscription Control API v1");
    options.DocumentTitle = "Subscription Control API";
});

app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

return;

static string BuildConnectionString(IConfiguration configuration)
{
    var host = configuration["DB_HOST"];
    if (!string.IsNullOrWhiteSpace(host))
    {
        return new Npgsql.NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = int.TryParse(configuration["DB_PORT"], out var port) ? port : 5432,
            Database = configuration["DB_NAME"],
            Username = configuration["DB_USER"],
            Password = configuration["DB_PASSWORD"]
        }.ConnectionString;
    }

    return configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException(
               "Keine Datenbankverbindung konfiguriert (weder DB_HOST-Umgebungsvariablen noch ConnectionStrings:DefaultConnection).");
}
