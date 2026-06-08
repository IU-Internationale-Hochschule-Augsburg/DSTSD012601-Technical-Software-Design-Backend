using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Application.Models;
using Subscription_Control_Backend.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins(
                    "http://localhost:8011")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var connectionString = ""; /*builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' wurde nicht gefunden.");*/

/*builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));*/

builder.Services.AddScoped<IExampleService, ExampleService>();
var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(myAllowSpecificOrigins);
app.UseHttpsRedirection();
app.MapControllers();

app.Run();