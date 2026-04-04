using System.Data.Entity;
using System.Data.Entity.SqlServer;
using ContosoUniversity;
using ContosoUniversity.Models;
using ContosoUniversity.BLL;
using ContosoUniversity.Bll;

var builder = WebApplication.CreateBuilder(args);

// Register EF6 SQL Server provider
DbConfiguration.SetConfiguration(new ContosoUniversityDbConfiguration());

// Add Razor Pages
builder.Services.AddRazorPages();

// Register DbContext
var connectionString = builder.Configuration.GetConnectionString("ContosoUniversityEntities")
    ?? throw new InvalidOperationException("Connection string 'ContosoUniversityEntities' not found.");

builder.Services.AddScoped<ContosoUniversityEntities>(provider =>
    new ContosoUniversityEntities(connectionString));

// Register BLL services
builder.Services.AddScoped<StudentsListLogic>();
builder.Services.AddScoped<Courses_Logic>();
builder.Services.AddScoped<Instructors_Logic>();
builder.Services.AddScoped<Enrollmet_Logic>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
