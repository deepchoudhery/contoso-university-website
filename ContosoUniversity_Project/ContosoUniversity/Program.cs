var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseAuthorization();
app.MapControllers();

// Add a simple health check endpoint
app.MapGet("/", () => "Contoso University - Upgraded to .NET 10.0 successfully!");
app.MapGet("/api/health", () => new
{
    Status = "Healthy",
    Framework = ".NET 10.0",
    Application = "Contoso University",
    Timestamp = DateTime.UtcNow
});

app.Run();
