using ContosoUniversity;
using ContosoUniversity.Models;

var builder = WebApplication.CreateBuilder(args);

// Initialize connection strings from configuration
ConnectionStringProvider.EfConnectionString = builder.Configuration.GetConnectionString("ContosoUniversityEntities")
    ?? ConnectionStringProvider.EfConnectionString;
ConnectionStringProvider.SqlConnectionString = builder.Configuration.GetConnectionString("ContosoUniversity")
    ?? ConnectionStringProvider.SqlConnectionString;

// Add services to the container
builder.Services.AddRazorPages();

// Register DbContext with EF6 (classic Entity Framework)
builder.Services.AddScoped<ContosoUniversityEntities>(provider =>
    new ContosoUniversityEntities("name=ContosoUniversityEntities"));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
