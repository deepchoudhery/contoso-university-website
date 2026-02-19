# .NET 10.0 Upgrade Plan for ContosoUniversity

**Date**: 2025-01-22  
**Strategy**: All-At-Once (Complete Rewrite)  
**Source**: .NET Framework 4.8 (ASP.NET Web Forms)  
**Target**: .NET 10.0 (ASP.NET Core)

---

## Executive Summary

### Scope and Complexity

This upgrade represents a **complete architectural transformation** from ASP.NET Web Forms (.NET Framework 4.8) to ASP.NET Core (.NET 10.0). This is not a simple framework version upgrade but a fundamental platform migration requiring:

- Complete rewrite of the web UI layer (Web Forms → Razor Pages/MVC)
- Conversion from non-SDK-style to SDK-style project format
- Migration from Entity Framework 6 to Entity Framework Core
- Replacement of System.Data.SqlClient with Microsoft.Data.SqlClient
- Migration from web.config to appsettings.json configuration
- Modernization of dependency injection patterns

**Key Metrics from Assessment:**
- **Total Projects**: 1 (ContosoUniversity.csproj)
- **Current Framework**: .NET Framework 4.8
- **Target Framework**: .NET 10.0
- **Total Code Files**: 26
- **Lines of Code**: 1,314
- **Estimated Impact**: 245+ LOC (18.6% of codebase)
- **API Issues**: 245 total (169 binary incompatible, 76 source incompatible)
- **Package Issues**: 17 (7 incompatible, 1 upgrade recommended)

### Selected Strategy

**All-At-Once Strategy** - Complete platform migration executed as a coordinated transformation.

**Rationale**:
- Single project solution (simple structure)
- ASP.NET Web Forms cannot run on .NET Core - requires complete rewrite
- No incremental migration path available
- Cleaner to rebuild on modern platform than attempt gradual migration
- All 168 System.Web API issues are architectural (no .NET Core equivalent)

### Critical Migration Challenges

1. **Web Forms UI (Highest Impact)**: 168 API issues related to System.Web - requires complete UI rewrite to Razor Pages or MVC
2. **Entity Framework 6 → EF Core**: DbContext initialization, database seeding patterns
3. **Configuration System**: web.config → appsettings.json migration
4. **SqlClient Migration**: System.Data.SqlClient → Microsoft.Data.SqlClient (76 source incompatible APIs)
5. **Application Insights**: Outdated packages need modernization
6. **Global.asax → Program.cs**: Application initialization patterns

---

## Prerequisites

### Required Tools and SDK

- [ ] .NET 10.0 SDK installed and verified
- [ ] Visual Studio 2022 (or VS Code with C# Dev Kit)
- [ ] SQL Server (LocalDB or full instance) for development
- [ ] Git for source control

### Pre-Upgrade Validation

- [ ] Current application builds successfully in .NET Framework 4.8
- [ ] Database connection string and schema are documented
- [ ] All Web Forms pages (.aspx) catalogued
- [ ] Entity Framework model (Model1.edmx) is understood
- [ ] Global.asax initialization logic is documented

---

## Implementation Approach

### Migration Philosophy

Given the complete architectural incompatibility between Web Forms and ASP.NET Core, we will:

1. **Create a new ASP.NET Core project** using modern templates
2. **Port business logic and data models** from the existing project
3. **Recreate UI** using Razor Pages (similar page-based model to Web Forms)
4. **Modernize data access** using Entity Framework Core
5. **Preserve functionality** while adopting modern patterns

### Why Razor Pages?

Razor Pages is recommended over MVC because:
- Page-based model similar to Web Forms (easier mental model for migration)
- Simpler for form-heavy CRUD applications
- Less ceremony than MVC
- Each page is self-contained (like Web Forms)

---

## Detailed Migration Plan

### Phase 0: Preparation and Analysis

#### 0.1 Document Current Application

**Catalog existing pages:**
- Home.aspx - Landing page
- About.aspx - About page
- Students.aspx - Student management
- Courses.aspx - Course management
- Instructors.aspx - Instructor management

**Extract business logic from code-behind:**
- Review all `.aspx.cs` files for business logic
- Identify data access patterns
- Document validation rules
- Note any custom controls or user controls

**Analyze Entity Framework Model:**
- Review `Models/Model1.edmx` to understand database schema
- Identify entities: Student, Course, Instructor, Enrollment, Department, etc.
- Document relationships and navigation properties
- Extract database initialization and seeding logic from Global.asax.cs

**Configuration inventory:**
- Connection strings from Web.config
- Application settings
- Authentication/authorization configuration (if any)

#### 0.2 Create Migration Branch

```bash
# Already on copilot/upgrade-to-dotnet-10 branch
git status  # Verify clean working tree
```

---

### Phase 1: Create New ASP.NET Core Project Structure

#### 1.1 Create New ASP.NET Core Razor Pages Project

**Operations:**
```bash
cd ContosoUniversity_Project
dotnet new razor -n ContosoUniversity -o ContosoUniversity.Core -f net10.0
```

**Project structure created:**
```
ContosoUniversity.Core/
├── Pages/
│   ├── Index.cshtml
│   ├── Privacy.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── ContosoUniversity.Core.csproj
```

#### 1.2 Add Required NuGet Packages

**Add to ContosoUniversity.Core.csproj:**
```xml
<ItemGroup>
  <!-- Entity Framework Core -->
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  
  <!-- SQL Client -->
  <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />
  
  <!-- Application Insights (modern version) -->
  <PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.22.0" />
</ItemGroup>
```

**Package migration summary:**

| Old Package (Framework) | New Package (Core) | Notes |
|--------------------------|---------------------|-------|
| EntityFramework 6.1.3 | Microsoft.EntityFrameworkCore.SqlServer 10.0.0 | Complete migration required |
| System.Data.SqlClient | Microsoft.Data.SqlClient 5.2.0 | Direct replacement |
| Microsoft.ApplicationInsights 2.1.0 | Microsoft.ApplicationInsights.AspNetCore 2.22.0 | Modern version |
| Microsoft.Net.Compilers 1.0.0 | *(Removed)* | Included in SDK |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | *(Removed)* | Included in SDK |

---

### Phase 2: Migrate Data Layer (Entity Framework)

#### 2.1 Create Entity Classes from EDMX

**From existing Model1.edmx, extract entity classes:**

Based on typical Contoso University schema, create POCOs in `ContosoUniversity.Core/Models/`:

**Student.cs:**
```csharp
namespace ContosoUniversity.Core.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
```

**Course.cs:**
```csharp
namespace ContosoUniversity.Core.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
        
        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
```

**Instructor.cs:**
```csharp
namespace ContosoUniversity.Core.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        
        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
```

**Additional entities to create based on EDMX:**
- Enrollment.cs
- Department.cs
- CourseAssignment.cs
- OfficeAssignment.cs

#### 2.2 Create DbContext

**Create `Data/SchoolContext.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) 
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

            // Configure relationships
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}
```

#### 2.3 Migrate Database Initialization and Seeding

**Extract seeding logic from Global.asax.cs and create `Data/DbInitializer.cs`:**
```csharp
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            // Check if DB is already seeded
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            // Add seed data from old initialization logic
            var students = new Student[]
            {
                new Student { FirstName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { FirstName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01") },
                // ... etc
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            // Repeat for Courses, Instructors, etc.
        }
    }
}
```

#### 2.4 Configure DbContext in Program.cs

**Add to Program.cs:**
```csharp
using ContosoUniversity.Core.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Configure DbContext
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SchoolContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure middleware pipeline
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
```

#### 2.5 Add Connection String to appsettings.json

**Update appsettings.json:**
```json
{
  "ConnectionStrings": {
    "SchoolContext": "Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Note**: Extract actual connection string from old Web.config.

---

### Phase 3: Migrate UI Layer (Web Forms → Razor Pages)

#### 3.1 Create Razor Pages for Each Web Form

**Migration mapping:**

| Old Web Form | New Razor Page | Purpose |
|--------------|----------------|---------|
| Home.aspx | Pages/Index.cshtml | Landing page |
| About.aspx | Pages/About.cshtml | About page |
| Students.aspx | Pages/Students/Index.cshtml | Student list/management |
| Courses.aspx | Pages/Courses/Index.cshtml | Course list/management |
| Instructors.aspx | Pages/Instructors/Index.cshtml | Instructor list/management |

#### 3.2 Students Management Page Example

**Create `Pages/Students/Index.cshtml`:**
```cshtml
@page
@model ContosoUniversity.Core.Pages.Students.IndexModel
@{
    ViewData["Title"] = "Students";
}

<h2>Students</h2>

<p>
    <a asp-page="Create">Create New</a>
</p>

<form asp-page="./Index" method="get">
    <div class="form-actions no-color">
        <p>
            Find by name: <input type="text" name="SearchString" value="@Model.CurrentFilter" />
            <input type="submit" value="Search" class="btn btn-default" /> |
            <a asp-page="./Index">Back to Full List</a>
        </p>
    </div>
</form>

<table class="table">
    <thead>
        <tr>
            <th>
                <a asp-page="./Index" asp-route-sortOrder="@Model.NameSort">Last Name</a>
            </th>
            <th>First Name</th>
            <th>Enrollment Date</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.Students)
        {
            <tr>
                <td>@Html.DisplayFor(modelItem => item.LastName)</td>
                <td>@Html.DisplayFor(modelItem => item.FirstName)</td>
                <td>@Html.DisplayFor(modelItem => item.EnrollmentDate)</td>
                <td>
                    <a asp-page="./Edit" asp-route-id="@item.StudentID">Edit</a> |
                    <a asp-page="./Details" asp-route-id="@item.StudentID">Details</a> |
                    <a asp-page="./Delete" asp-route-id="@item.StudentID">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>

@{
    var prevDisabled = !Model.Students.HasPreviousPage ? "disabled" : "";
    var nextDisabled = !Model.Students.HasNextPage ? "disabled" : "";
}

<a asp-page="./Index"
   asp-route-sortOrder="@Model.CurrentSort"
   asp-route-pageIndex="@(Model.Students.PageIndex - 1)"
   asp-route-currentFilter="@Model.CurrentFilter"
   class="btn btn-default @prevDisabled">
    Previous
</a>
<a asp-page="./Index"
   asp-route-sortOrder="@Model.CurrentSort"
   asp-route-pageIndex="@(Model.Students.PageIndex + 1)"
   asp-route-currentFilter="@Model.CurrentFilter"
   class="btn btn-default @nextDisabled">
    Next
</a>
```

**Create `Pages/Students/Index.cshtml.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Core.Data;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Students { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 10;
            Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
```

**Create pagination helper `PaginatedList.cs`:**
```csharp
using Microsoft.EntityFrameworkCore;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
```

#### 3.3 Repeat for Other Pages

**Create similar Razor Pages for:**
- **Courses** (Index, Create, Edit, Delete, Details)
- **Instructors** (Index, Create, Edit, Delete, Details)
- **About** (static page)

**Port business logic from `.aspx.cs` code-behind files to PageModel classes.**

#### 3.4 Migrate Static Assets

**Copy and adapt:**
- CSS files from `ContosoUniversity/CSS/` → `ContosoUniversity.Core/wwwroot/css/`
- JavaScript from `ContosoUniversity/JQuery/` → `ContosoUniversity.Core/wwwroot/js/`
- Images from `ContosoUniversity/Images/` → `ContosoUniversity.Core/wwwroot/images/`

**Update references in _Layout.cshtml to use new paths.**

---

### Phase 4: Configuration and Application Insights

#### 4.1 Migrate Application Insights

**Add to Program.cs:**
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

**Copy instrumentation key from ApplicationInsights.config to appsettings.json:**
```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key-here"
  }
}
```

**Or use connection string (modern approach):**
```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=your-key-here;..."
  }
}
```

---

### Phase 5: Build and Resolve Compilation Errors

#### 5.1 Build New Project

```bash
cd ContosoUniversity.Core
dotnet build
```

**Expected issues to resolve:**
- Missing using statements
- Type mismatches between EF6 and EF Core APIs
- SqlClient namespace changes (System.Data.SqlClient → Microsoft.Data.SqlClient)
- ViewState/PostBack patterns need replacement with modern state management

#### 5.2 Fix Common Migration Issues

**System.Data.SqlClient → Microsoft.Data.SqlClient:**
- Update all `using System.Data.SqlClient;` to `using Microsoft.Data.SqlClient;`
- If any direct ADO.NET code exists in BLL folder, update namespace

**Entity Framework 6 → EF Core:**
- Remove `.Include()` string-based queries, use lambda expressions
- Update lazy loading patterns (virtual navigation properties need explicit configuration in EF Core)
- Replace `Database.SetInitializer` with `DbInitializer.Initialize` pattern

**Configuration access:**
- Replace `ConfigurationManager.ConnectionStrings["name"]` with dependency-injected `IConfiguration`

---

### Phase 6: Testing and Validation

#### 6.1 Database Testing

- [ ] Run application, verify database is created
- [ ] Verify seed data is populated
- [ ] Test CRUD operations on Students
- [ ] Test CRUD operations on Courses
- [ ] Test CRUD operations on Instructors
- [ ] Verify relationships (Enrollments, CourseAssignments) work correctly

#### 6.2 Functional Testing

Test each page for functionality parity with original:

**Students Page:**
- [ ] List displays correctly
- [ ] Search by name works
- [ ] Sorting by name and date works
- [ ] Pagination works
- [ ] Create new student
- [ ] Edit existing student
- [ ] Delete student
- [ ] View student details

**Courses Page:**
- [ ] List displays with department information
- [ ] CRUD operations work
- [ ] Department relationship preserved

**Instructors Page:**
- [ ] List displays correctly
- [ ] Office assignment shown
- [ ] Course assignments displayed
- [ ] CRUD operations work

**About Page:**
- [ ] Content displays correctly

#### 6.3 Performance Testing

- [ ] Page load times acceptable
- [ ] Database query performance (check for N+1 queries)
- [ ] Application Insights telemetry is being collected

---

### Phase 7: Cleanup and Final Steps

#### 7.1 Remove Old Project

Once new project is validated:
- Keep old `ContosoUniversity` folder as reference
- Update solution file to point to new project
- Or maintain side-by-side during transition period

#### 7.2 Update Documentation

- [ ] Update README with new setup instructions
- [ ] Document .NET 10.0 prerequisites
- [ ] Update connection string configuration
- [ ] Note breaking changes from Web Forms

#### 7.3 Source Control

```bash
git add .
git commit -m "Migrate ContosoUniversity from .NET Framework 4.8 Web Forms to .NET 10.0 Razor Pages

- Created new ASP.NET Core Razor Pages project targeting net10.0
- Migrated Entity Framework 6 to Entity Framework Core 10
- Converted all Web Forms pages to Razor Pages
- Migrated System.Data.SqlClient to Microsoft.Data.SqlClient
- Updated Application Insights to modern version
- Migrated configuration from web.config to appsettings.json
- Implemented DbContext with dependency injection
- Created database initializer for seed data
- All CRUD operations tested and functional"
```

---

## Package Update Reference

### Complete Package Migration Matrix

| Category | Old Package | Old Version | New Package | New Version | Action | Projects |
|----------|-------------|-------------|-------------|-------------|--------|----------|
| **ORM** | EntityFramework | 6.1.3 | Microsoft.EntityFrameworkCore.SqlServer | 10.0.0 | Replace | All |
| **ORM Tools** | - | - | Microsoft.EntityFrameworkCore.Tools | 10.0.0 | Add | All |
| **Data Access** | System.Data.SqlClient | (Framework) | Microsoft.Data.SqlClient | 5.2.0 | Replace | All |
| **Telemetry** | Microsoft.ApplicationInsights | 2.1.0 | Microsoft.ApplicationInsights.AspNetCore | 2.22.0 | Replace | All |
| **Telemetry** | Microsoft.ApplicationInsights.Agent.Intercept | 1.2.1 | *(removed)* | - | Remove | All |
| **Telemetry** | Microsoft.ApplicationInsights.DependencyCollector | 2.1.0 | *(included in AspNetCore)* | - | Remove | All |
| **Telemetry** | Microsoft.ApplicationInsights.PerfCounterCollector | 2.1.0 | *(included in AspNetCore)* | - | Remove | All |
| **Telemetry** | Microsoft.ApplicationInsights.Web | 2.1.0 | *(included in AspNetCore)* | - | Remove | All |
| **Telemetry** | Microsoft.ApplicationInsights.WindowsServer | 2.1.0 | *(included in AspNetCore)* | - | Remove | All |
| **Telemetry** | Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel | 2.1.0 | *(included in AspNetCore)* | - | Remove | All |
| **Compiler** | Microsoft.Net.Compilers | 1.0.0 | *(SDK included)* | - | Remove | All |
| **Compiler** | Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 1.0.0 | *(SDK included)* | - | Remove | All |

---

## Breaking Changes Catalog

### Major Architectural Changes

#### 1. Web Forms → Razor Pages (CRITICAL)

**Impact**: Complete UI rewrite required

**Old Pattern (Web Forms):**
```csharp
// Students.aspx.cs
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        BindGrid();
    }
}

private void BindGrid()
{
    using (var context = new SchoolContext())
    {
        GridView1.DataSource = context.Students.ToList();
        GridView1.DataBind();
    }
}
```

**New Pattern (Razor Pages):**
```csharp
// Pages/Students/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly SchoolContext _context;
    
    public IndexModel(SchoolContext context) => _context = context;
    
    public List<Student> Students { get; set; }
    
    public async Task OnGetAsync()
    {
        Students = await _context.Students.ToListAsync();
    }
}
```

**Migration Path**:
- Identify all Page_Load, button click handlers, and ViewState usage
- Extract business logic to services or PageModel methods
- Replace server controls with HTML helpers and tag helpers
- Implement proper async/await patterns

#### 2. Entity Framework 6 → Entity Framework Core

**Impact**: DbContext initialization, LINQ queries, lazy loading

**Old Pattern (EF6):**
```csharp
// Global.asax.cs
Database.SetInitializer(new SchoolInitializer());

// Usage with implicit lazy loading
public class SchoolContext : DbContext
{
    public SchoolContext() : base("name=SchoolContext") { }
    
    public DbSet<Student> Students { get; set; }
}
```

**New Pattern (EF Core):**
```csharp
// Program.cs
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(connectionString));

// Usage with DI
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) 
        : base(options) { }
    
    public DbSet<Student> Students { get; set; }
}
```

**Migration Path**:
- Convert EDMX model to POCO classes
- Replace Database.SetInitializer with explicit initialization in Program.cs
- Update navigation property patterns for lazy loading
- Test all LINQ queries (some EF6 queries won't translate in EF Core)

#### 3. System.Data.SqlClient → Microsoft.Data.SqlClient

**Impact**: Namespace changes, connection string encryption defaults

**Old Code:**
```csharp
using System.Data.SqlClient;

var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString);
```

**New Code:**
```csharp
using Microsoft.Data.SqlClient;

var conn = new SqlConnection(_configuration.GetConnectionString("SchoolContext"));
```

**Breaking Changes**:
- `Encrypt=false` was default in System.Data.SqlClient
- `Encrypt=true` is default in Microsoft.Data.SqlClient 4.0+
- Add `TrustServerCertificate=true` to connection string for local development

#### 4. Configuration System

**Impact**: All configuration access patterns change

**Old Pattern:**
```csharp
var connString = ConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString;
var setting = ConfigurationManager.AppSettings["SettingName"];
```

**New Pattern:**
```csharp
// Via DI
private readonly IConfiguration _configuration;

var connString = _configuration.GetConnectionString("SchoolContext");
var setting = _configuration["SettingName"];
```

**Migration Path**:
- Extract all connection strings from web.config to appsettings.json
- Extract all app settings to appsettings.json
- Inject IConfiguration where needed
- Use Options pattern for complex configuration sections

#### 5. Global.asax → Program.cs

**Impact**: Application startup and initialization

**Old Pattern:**
```csharp
// Global.asax.cs
protected void Application_Start()
{
    Database.SetInitializer(new SchoolInitializer());
    // Other initialization
}
```

**New Pattern:**
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
// Configure services

var app = builder.Build();
// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    DbInitializer.Initialize(context);
}
// Configure middleware
app.Run();
```

---

## Risk Assessment and Mitigation

### High-Risk Areas

#### Risk 1: Complete UI Rewrite (CRITICAL)

**Risk**: Web Forms pages have no direct equivalent in ASP.NET Core

**Likelihood**: Certain  
**Impact**: High - Complete rewrite required  

**Mitigation**:
- Start with simplest page (About) to establish patterns
- Create reusable components for common UI patterns
- Test each page thoroughly before moving to next
- Keep old application running for reference
- Use Razor Pages (not MVC) to maintain page-based model

#### Risk 2: Entity Framework Migration Complexity

**Risk**: EF6 → EF Core has subtle behavioral differences

**Likelihood**: High  
**Impact**: Medium - Data access bugs, performance issues  

**Mitigation**:
- Test all CRUD operations thoroughly
- Check for N+1 query problems (use `.Include()` explicitly)
- Verify lazy loading works as expected
- Test all custom queries and stored procedure calls
- Monitor query performance in Application Insights

#### Risk 3: Data Loss During Migration

**Risk**: Database schema or seed data issues

**Likelihood**: Medium  
**Impact**: High - Lost or corrupted data  

**Mitigation**:
- Backup database before any migration
- Use same database schema (don't recreate)
- Verify EF Core migrations match existing schema
- Test seed data initialization on clean database
- Keep original project as fallback

#### Risk 4: Missing Functionality

**Risk**: Web Forms features not ported correctly

**Likelihood**: Medium  
**Impact**: Medium - Feature gaps  

**Mitigation**:
- Create comprehensive checklist of all features
- Test each feature in isolation
- Get user acceptance testing before decommissioning old app
- Document any intentional changes or removals

### Medium-Risk Areas

#### Risk 5: Third-Party Dependencies

**Risk**: AjaxControlToolkit has no .NET Core equivalent

**Likelihood**: High  
**Impact**: Low - Can use modern alternatives  

**Mitigation**:
- Identify all Ajax Toolkit controls used
- Replace with jQuery, Bootstrap, or modern alternatives
- Test interactive features thoroughly

---

## Testing Strategy

### Unit Testing (If Applicable)

Create test project:
```bash
dotnet new xunit -n ContosoUniversity.Tests
cd ContosoUniversity.Tests
dotnet add reference ../ContosoUniversity.Core/ContosoUniversity.Core.csproj
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

Test data access:
```csharp
public class SchoolContextTests
{
    [Fact]
    public async Task CanAddAndRetrieveStudent()
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new SchoolContext(options))
        {
            context.Students.Add(new Student 
            { 
                FirstName = "Test", 
                LastName = "Student", 
                EnrollmentDate = DateTime.Now 
            });
            await context.SaveChangesAsync();
        }

        using (var context = new SchoolContext(options))
        {
            var student = await context.Students.FirstOrDefaultAsync();
            Assert.NotNull(student);
            Assert.Equal("Test", student.FirstName);
        }
    }
}
```

### Integration Testing

Test complete page workflows:
- Page loads correctly
- Form submissions work
- Data is saved to database
- Validation works
- Error handling is correct

### Manual Testing Checklist

**For Each Page:**
- [ ] Page renders correctly
- [ ] All links work
- [ ] Forms submit successfully
- [ ] Validation messages display
- [ ] Data displays correctly from database
- [ ] Sorting works (if applicable)
- [ ] Filtering/search works (if applicable)
- [ ] Pagination works (if applicable)
- [ ] CSS and JavaScript load correctly
- [ ] Responsive design works (test mobile)

---

## Success Criteria

The migration is complete when:

### Technical Criteria

- [ ] New ASP.NET Core project targeting .NET 10.0 exists
- [ ] All pages from old application recreated as Razor Pages
- [ ] Entity Framework Core configured and working
- [ ] Database connects and seeds correctly
- [ ] All CRUD operations functional
- [ ] Application builds with **0 errors** and **0 warnings**
- [ ] All packages updated to .NET 10-compatible versions
- [ ] Application Insights telemetry working

### Functional Criteria

- [ ] All student management features work
- [ ] All course management features work
- [ ] All instructor management features work
- [ ] About page displays correctly
- [ ] Search and filtering work correctly
- [ ] Sorting works correctly
- [ ] Pagination works correctly
- [ ] Database relationships maintained

### Quality Criteria

- [ ] No package security vulnerabilities
- [ ] Performance acceptable (page loads < 2s)
- [ ] Error handling in place
- [ ] Logging configured
- [ ] Code follows .NET Core conventions

---

## Rollback Plan

### If Migration Fails

1. **Preserve original project** - Do not delete `ContosoUniversity` folder
2. **Keep separate solution** - Maintain old .sln until migration validated
3. **Database backup** - Restore from backup if schema changes fail
4. **Git revert** - Use version control to revert changes

### Rollback Steps

```bash
# If needed, revert to original state
git checkout main  # or original branch
git branch -D copilot/upgrade-to-dotnet-10

# Restore database
sqlcmd -S (localdb)\mssqllocaldb -Q "RESTORE DATABASE ContosoUniversity FROM DISK='path\to\backup.bak'"
```

---

## Timeline Estimate

### Conservative Estimate (for reference, actual execution will be automated)

| Phase | Estimated Time | Complexity |
|-------|----------------|------------|
| Phase 0: Preparation | 2-4 hours | Low |
| Phase 1: New Project Setup | 1-2 hours | Low |
| Phase 2: Data Layer Migration | 4-6 hours | Medium |
| Phase 3: UI Layer Migration | 12-16 hours | High |
| Phase 4: Configuration | 1-2 hours | Low |
| Phase 5: Build Fixes | 4-6 hours | Medium |
| Phase 6: Testing | 8-12 hours | High |
| Phase 7: Cleanup | 1-2 hours | Low |
| **Total** | **33-50 hours** | **High** |

**Note**: Timeline assumes one developer working sequentially. Automated execution will be significantly faster.

---

## Appendix A: File Mapping Reference

### Old Project → New Project File Mapping

| Old File | New File | Status |
|----------|----------|--------|
| **Web Forms** |
| Home.aspx | Pages/Index.cshtml | Rewrite |
| About.aspx | Pages/About.cshtml | Port |
| Students.aspx | Pages/Students/Index.cshtml | Rewrite |
| Courses.aspx | Pages/Courses/Index.cshtml | Rewrite |
| Instructors.aspx | Pages/Instructors/Index.cshtml | Rewrite |
| **Code-Behind** |
| *.aspx.cs | *.cshtml.cs (PageModel) | Extract logic |
| **Data Models** |
| Models/Model1.edmx | Models/*.cs (POCOs) | Convert |
| Models/Model1.Context.cs | Data/SchoolContext.cs | Rewrite |
| **Configuration** |
| Web.config | appsettings.json | Migrate |
| ApplicationInsights.config | appsettings.json | Migrate |
| **Global** |
| Global.asax | Program.cs | Rewrite |
| **Static Files** |
| CSS/* | wwwroot/css/* | Copy/adapt |
| JQuery/* | wwwroot/js/* | Copy/adapt |
| Images/* | wwwroot/images/* | Copy |
| **Project** |
| ContosoUniversity.csproj | ContosoUniversity.Core.csproj | New SDK-style |
| packages.config | *(PackageReference in csproj)* | Convert |

---

## Appendix B: Key API Replacements

### System.Web → ASP.NET Core

| Old API | New API | Notes |
|---------|---------|-------|
| `Page.IsPostBack` | `Request.Method == "POST"` | Check HTTP verb |
| `Response.Redirect()` | `RedirectToPage()` | Razor Pages navigation |
| `Server.MapPath()` | `IWebHostEnvironment.ContentRootPath` | Inject IWebHostEnvironment |
| `ViewState["key"]` | TempData or session state | Or redesign without state |
| `Request.QueryString["key"]` | `Request.Query["key"]` | Or use page parameters |
| `ConfigurationManager.AppSettings` | `IConfiguration["key"]` | Dependency injection |

### Entity Framework 6 → EF Core

| Old API | New API | Notes |
|---------|---------|-------|
| `context.Entry(entity).State = EntityState.Modified` | `context.Update(entity)` | Simplified |
| `Database.SetInitializer()` | Manual initialization in Program.cs | No automatic initializers |
| `.Include("NavigationProperty")` | `.Include(x => x.NavigationProperty)` | Strongly typed |
| `DbModelBuilder` | `ModelBuilder` | OnModelCreating |

---

## Appendix C: Connection String Encryption Notes

### Important: Microsoft.Data.SqlClient Encryption Changes

**System.Data.SqlClient** (old):
- `Encrypt=false` by default
- Server certificate validation only when `Encrypt=true`

**Microsoft.Data.SqlClient 4.0+** (new):
- `Encrypt=true` by default (BREAKING CHANGE)
- Server certificate always validated

### Recommended Connection String for Local Development

```json
{
  "ConnectionStrings": {
    "SchoolContext": "Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
  }
}
```

**Or with encryption enabled (production):**
```json
{
  "ConnectionStrings": {
    "SchoolContext": "Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

---

## Summary

This plan outlines a complete migration from .NET Framework 4.8 ASP.NET Web Forms to .NET 10.0 ASP.NET Core Razor Pages. The migration is complex due to the architectural differences between the platforms, but following this structured approach will ensure a successful transition while maintaining functionality and improving the application's maintainability and performance.

**Key Success Factors:**
1. Thorough understanding of the existing application
2. Careful migration of data models and business logic
3. Complete rewrite of UI layer using modern patterns
4. Comprehensive testing at every stage
5. Maintaining the original application as reference during migration

The all-at-once approach is appropriate for this single-project solution, allowing for a clean break from the legacy platform and full adoption of modern .NET practices.
