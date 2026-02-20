# .NET 10 Upgrade Plan for ContosoUniversity

**Date**: 2025  
**Target Framework**: .NET 10 (net10.0)  
**Strategy**: All-At-Once

---

## Executive Summary

### Overview
This plan outlines the upgrade of ContosoUniversity solution from .NET Framework 4.8 to .NET 10. The solution consists of 1 ASP.NET Web Application Project (WAP) containing 1,314 lines of code across 26 code files.

### Selected Strategy
**All-At-Once Strategy** - The single project will be upgraded simultaneously in one coordinated operation.

**Rationale**: 
- Single project solution (minimal complexity)
- Clear conversion path from ASP.NET Framework to ASP.NET Core
- All package upgrades have been identified
- Coordinated approach reduces risk of partial migration state

### Complexity Assessment
**Difficulty Level**: 🔴 High

**Key Challenges**:
1. **ASP.NET Web Forms to ASP.NET Core Migration** - The application uses System.Web.UI controls (GridView, DetailsView, TextBox, etc.) which are not available in ASP.NET Core. This represents a fundamental architectural shift from Web Forms to modern ASP.NET Core patterns (Razor Pages, MVC, or Blazor).
2. **SDK-Style Conversion** - Project must be converted from legacy .csproj format to SDK-style
3. **API Compatibility** - 245 API issues identified (169 binary incompatible, 76 source incompatible)
4. **Package Updates** - 8 out of 10 packages need updates or replacements
5. **Configuration Migration** - Move from web.config to appsettings.json
6. **Database Access** - Migrate from System.Data.SqlClient to Microsoft.Data.SqlClient
7. **Entity Framework** - Classic EF 6 needs migration to modern patterns

### Impact Summary
- **Projects to Upgrade**: 1
- **Estimated LOC Impact**: 245+ lines (18.6% of codebase)
- **Code Files with Changes**: 15 out of 26
- **Package Changes**: 10 packages (8 updates, 2 removals)

---

## Project Analysis

### Dependency Structure

```
ContosoUniversity.csproj (net48)
└── (No project dependencies - standalone application)
```

### Current State

| Project | Type | Current TFM | Target TFM | LOC | Risk |
|---------|------|-------------|------------|-----|------|
| ContosoUniversity.csproj | ASP.NET WAP | net48 | net10.0 | 1,314 | 🔴 High |

**Project Characteristics**:
- **Type**: Classic ASP.NET Web Application Project (WAP) using Web Forms
- **SDK-Style**: No (requires conversion)
- **Dependencies**: 0 project dependencies
- **Files**: 54 total, 26 code files, 15 with compatibility issues

---

## Upgrade Strategy

### Approach: Incremental Architectural Transformation

Given the fundamental architectural differences between ASP.NET Web Forms and ASP.NET Core, this upgrade requires more than just framework version changes. We'll proceed with:

**Phase 0: Preparation**
- SDK installation verification
- Branch and source control setup
- Baseline build verification

**Phase 1: Project Modernization**
- Convert to SDK-style project
- Update target framework to net10.0
- Update package references

**Phase 2: ASP.NET Core Migration**
- Replace Web Forms with Razor Pages (or MVC)
- Migrate configuration from web.config to appsettings.json
- Update database access patterns
- Migrate Entity Framework initialization

**Phase 3: API Compatibility Fixes**
- Address System.Web dependency removals
- Update System.Data.SqlClient to Microsoft.Data.SqlClient
- Fix source incompatibilities

**Phase 4: Validation & Testing**
- Build verification
- Functional testing
- Security validation

### Source Control Strategy

**Commit Strategy**: Atomic commits per phase
- Phase 1 completion → Commit 1
- Phase 2 completion → Commit 2
- Phase 3 completion → Commit 3
- Phase 4 completion → Final commit

**Branch**: copilot/upgrade-dotnet-10-app (already created)

---

## Technology Migration Paths

### ASP.NET Framework to ASP.NET Core

**Current State**: ASP.NET Web Forms (System.Web.UI)
- Uses Page lifecycle (Page_Load, IsPostBack)
- Server controls (GridView, DetailsView, TextBox, DropDownList)
- ViewState for state management
- Code-behind pattern

**Target State**: ASP.NET Core Razor Pages
- Tag Helpers instead of server controls
- Model binding instead of ViewState
- Dependency injection throughout
- Modern routing and middleware pipeline

**Migration Pattern**:
1. Map each .aspx page to a Razor Page (.cshtml + .cshtml.cs)
2. Replace server controls with HTML + Tag Helpers
3. Convert code-behind logic to PageModel methods
4. Replace ViewState with model properties
5. Update data binding to use model binding

### Configuration System

**Current**: web.config XML-based configuration
**Target**: appsettings.json + environment variables

**Migration Steps**:
1. Extract connection strings from web.config → appsettings.json
2. Extract app settings → appsettings.json
3. Update code to use IConfiguration
4. Remove ConfigurationManager references

### Entity Framework

**Current**: Entity Framework 6.1.3 with Database.SetInitializer
**Target**: Entity Framework 6.5.1 (or consider EF Core migration)

**Migration Pattern**:
1. Update to EF 6.5.1 (compatible with .NET Core)
2. Migrate DbContext initialization from Global.asax to Program.cs
3. Register DbContext in dependency injection
4. Update connection string access patterns
5. Consider future migration to EF Core 9

---

## Detailed Implementation Plan

### Phase 0: Preparation

#### Step 0.1: Verify Prerequisites
- [ ] Verify .NET 10 SDK installed
- [ ] Verify current branch: copilot/upgrade-dotnet-10-app
- [ ] Clean working directory (no uncommitted changes)

#### Step 0.2: Baseline Build
- [ ] Build solution with MSBuild (using vswhere to locate MSBuild.exe)
- [ ] Document current build status and warnings
- [ ] Verify application runs successfully on .NET Framework 4.8

**Validation**: Solution builds and runs successfully

---

### Phase 1: Project Modernization

#### Step 1.1: Convert to SDK-Style Project

**Action**: Use SDK-style conversion tool for ContosoUniversity.csproj

**Expected Changes**:
- Simplified .csproj file structure
- Automatic file inclusion (globbing)
- Removal of packages.config
- Migration to PackageReference

**Notes**:
- ASP.NET Web Application Projects have special considerations
- May require manual adjustments after tool conversion
- Reference: https://learn.microsoft.com/nuget/consume-packages/migrate-packages-config-to-package-reference

**Validation**: 
- [ ] Project converts successfully
- [ ] packages.config removed
- [ ] Project still targets net48 (no framework change yet)
- [ ] Project builds successfully after conversion

#### Step 1.2: Update Target Framework

**Action**: Update TargetFramework in ContosoUniversity.csproj

```xml
<!-- Before -->
<TargetFramework>net48</TargetFramework>

<!-- After -->
<TargetFramework>net10.0</TargetFramework>
```

**Validation**: Project file updated

#### Step 1.3: Update Package References

**Action**: Update all NuGet packages according to Package Update Reference (§5)

**Critical Updates**:
- EntityFramework 6.1.3 → 6.5.1
- Microsoft.ApplicationInsights packages → 3.0.0+ versions
- Remove Microsoft.CodeDom.Providers.DotNetCompilerPlatform (framework-included)
- Remove Microsoft.Net.Compilers (framework-included)
- Add Microsoft.Data.SqlClient (replacement for System.Data.SqlClient)

**Validation**:
- [ ] All package references updated
- [ ] Deprecated packages removed
- [ ] NuGet restore succeeds

---

### Phase 2: ASP.NET Core Migration

This is the most significant phase, involving architectural transformation.

#### Step 2.1: Add ASP.NET Core Infrastructure

**Action**: Convert project type from ASP.NET WAP to ASP.NET Core Web App

**Changes Required**:
1. Update SDK in .csproj:
   ```xml
   <Project Sdk="Microsoft.NET.Sdk.Web">
   ```

2. Add ASP.NET Core framework reference:
   ```xml
   <ItemGroup>
     <FrameworkReference Include="Microsoft.AspNetCore.App" />
   </ItemGroup>
   ```

3. Create Program.cs (new entry point):
   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   
   // Add services
   builder.Services.AddRazorPages();
   
   var app = builder.Build();
   
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

**Validation**: Project structure established

#### Step 2.2: Migrate Configuration

**Action**: Create appsettings.json and migrate settings from web.config

**Steps**:
1. Extract connection strings from web.config
2. Extract app settings
3. Create appsettings.json:
   ```json
   {
     "ConnectionStrings": {
       "SchoolContext": "[extracted from web.config]"
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

4. Remove web.config (or keep minimal version for IIS deployment)

**Validation**: Configuration accessible via IConfiguration

#### Step 2.3: Migrate Entity Framework DbContext

**Action**: Update DbContext initialization for ASP.NET Core

**Based on ef-dbcontext-migration skill**:

1. **Find DbContext classes** (e.g., SchoolContext)
2. **Update DbContext constructor**:
   ```csharp
   // Before
   public SchoolContext() : base("name=SchoolContext") { }
   
   // After
   public SchoolContext(DbContextOptions<SchoolContext> options) 
       : base(options) { }
   ```

3. **Register in Program.cs**:
   ```csharp
   // For EF 6.5.1 (classic EF on .NET Core)
   builder.Services.AddScoped<SchoolContext>(provider =>
       new SchoolContext("name=SchoolContext"));
   ```
   
   **OR** (if migrating to EF Core):
   ```csharp
   builder.Services.AddDbContext<SchoolContext>(options =>
       options.UseSqlServer(
           builder.Configuration.GetConnectionString("SchoolContext")));
   ```

4. **Migrate Database Initialization**:
   - Find Database.SetInitializer calls in Global.asax
   - Move to Program.cs after app.Build()
   - Update seeding logic if needed

**Validation**:
- [ ] DbContext registered in DI
- [ ] Connection strings accessible
- [ ] Database initialization migrated

#### Step 2.4: Convert Web Forms Pages to Razor Pages

**Action**: Migrate each .aspx page to Razor Pages

**Pages to Migrate** (based on typical ContosoUniversity structure):
- Default.aspx → Pages/Index.cshtml
- Students pages (List, Create, Edit, Delete, Details)
- Courses pages
- Instructors pages
- Departments pages
- About.aspx → Pages/About.cshtml

**Migration Pattern for Each Page**:

1. **Create Razor Page structure**:
   - Pages/Students/Index.cshtml (view)
   - Pages/Students/Index.cshtml.cs (PageModel)

2. **Migrate markup**:
   ```html
   <!-- Before (ASPX) -->
   <asp:GridView ID="studentsGrid" runat="server" 
                 AutoGenerateColumns="false">
       <Columns>
           <asp:BoundField DataField="LastName" HeaderText="Last Name" />
       </Columns>
   </asp:GridView>
   
   <!-- After (Razor) -->
   <table class="table">
       <thead>
           <tr><th>Last Name</th></tr>
       </thead>
       <tbody>
           @foreach (var student in Model.Students)
           {
               <tr><td>@student.LastName</td></tr>
           }
       </tbody>
   </table>
   ```

3. **Migrate code-behind**:
   ```csharp
   // Before (ASPX.CS)
   protected void Page_Load(object sender, EventArgs e)
   {
       if (!IsPostBack)
       {
           studentsGrid.DataSource = GetStudents();
           studentsGrid.DataBind();
       }
   }
   
   // After (PageModel)
   public class IndexModel : PageModel
   {
       private readonly SchoolContext _context;
       
       public IndexModel(SchoolContext context)
       {
           _context = context;
       }
       
       public IList<Student> Students { get; set; }
       
       public async Task OnGetAsync()
       {
           Students = await _context.Students.ToListAsync();
       }
   }
   ```

4. **Update navigation/links**:
   ```html
   <!-- Before -->
   <a href="Students/Details.aspx?id=1">Details</a>
   
   <!-- After -->
   <a asp-page="/Students/Details" asp-route-id="1">Details</a>
   ```

**Validation**:
- [ ] All .aspx pages converted
- [ ] Navigation works
- [ ] Data display functions correctly

#### Step 2.5: Remove Global.asax and Update Application Startup

**Action**: Remove Global.asax.cs and migrate any application-level logic

**Steps**:
1. Review Global.asax.cs for:
   - Application_Start → Move to Program.cs
   - Session_Start → Use session middleware if needed
   - Application_Error → Use exception handling middleware
   - Database initialization → Already moved in Step 2.3

2. Delete Global.asax and Global.asax.cs

**Validation**: Application starts correctly with new Program.cs

---

### Phase 3: API Compatibility Fixes

#### Step 3.1: Migrate System.Data.SqlClient to Microsoft.Data.SqlClient

**Action**: Apply sqlclient-migration skill

**Based on assessment**: 
- 29 API usages of System.Data.SqlClient identified
- Source incompatible (requires namespace changes)

**Steps**:

1. **Update package reference** (already done in Phase 1.3)

2. **Update using statements** in all affected files:
   ```csharp
   // Before
   using System.Data.SqlClient;
   
   // After
   using Microsoft.Data.SqlClient;
   ```

3. **Files to update** (search for System.Data.SqlClient usage):
   - DAL/*.cs (Data Access Layer files)
   - Any files with database queries

4. **Connection string encryption settings**:
   - Review connection strings in appsettings.json
   - Add explicit Encrypt and TrustServerCertificate if needed:
     ```json
     "ConnectionStrings": {
       "SchoolContext": "Server=...;Encrypt=true;TrustServerCertificate=false"
     }
     ```

**Behavioral Changes to Verify**:
- Encrypt defaults to true in Microsoft.Data.SqlClient (was false)
- DateTime with DbType.Time requires TimeSpan (not DateTime)
- Server certificate validation is always enabled

**Validation**:
- [ ] All System.Data.SqlClient references removed
- [ ] All using statements updated
- [ ] Connection strings reviewed
- [ ] Code compiles

#### Step 3.2: Update Configuration Access

**Action**: Replace ConfigurationManager with IConfiguration

**Affected Files**: Search for System.Configuration.ConfigurationManager usage

**Changes**:
```csharp
// Before
var connString = ConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString;
var appSetting = ConfigurationManager.AppSettings["SomeSetting"];

// After (via dependency injection)
public class SomeClass
{
    private readonly IConfiguration _configuration;
    
    public SomeClass(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void SomeMethod()
    {
        var connString = _configuration.GetConnectionString("SchoolContext");
        var appSetting = _configuration["SomeSetting"];
    }
}
```

**Validation**:
- [ ] ConfigurationManager references removed
- [ ] IConfiguration injected where needed
- [ ] Configuration values accessible

#### Step 3.3: Remove System.Web Dependencies

**Action**: Address remaining System.Web API issues

**Most Common Issues**:
- System.Web.UI.* controls → Already addressed in Phase 2.4
- System.Web.HttpContext → Use Microsoft.AspNetCore.Http.HttpContext
- System.Web.Security → Use ASP.NET Core Identity (if authentication exists)

**If authentication/authorization exists**:
1. Identify authentication mechanism (Forms Authentication, etc.)
2. Migrate to ASP.NET Core Authentication/Authorization
3. Update [Authorize] attributes if needed

**Validation**: All System.Web references removed

#### Step 3.4: Fix Compilation Errors

**Action**: Build solution and address all compilation errors

**Expected Error Categories**:
1. Missing using statements
2. API signature changes
3. Removed APIs requiring replacement
4. Type incompatibilities

**Process**:
- Build project
- For each error:
  - Identify the cause
  - Apply appropriate fix
  - Document significant changes
- Rebuild
- Repeat until zero errors

**Validation**:
- [ ] Solution builds with 0 errors
- [ ] All breaking changes addressed

---

### Phase 4: Validation & Testing

#### Step 4.1: Build Verification

**Action**: Full clean build with zero warnings

**Steps**:
1. Clean solution
2. Restore packages
3. Build solution
4. Address all warnings

**Target**: Zero build warnings

**Validation**:
- [ ] Clean build succeeds
- [ ] No compilation errors
- [ ] No compilation warnings

#### Step 4.2: Functional Testing

**Action**: Verify application functionality

**Test Scenarios**:
1. **Application Start**:
   - [ ] Application starts without errors
   - [ ] Home page loads

2. **Database Connectivity**:
   - [ ] Database connection established
   - [ ] DbContext initializes correctly
   - [ ] Data seeding works (if applicable)

3. **Student Pages** (example):
   - [ ] List students
   - [ ] View student details
   - [ ] Create new student
   - [ ] Edit student
   - [ ] Delete student

4. **Other Entity Pages**:
   - [ ] Courses CRUD operations
   - [ ] Instructors CRUD operations
   - [ ] Departments CRUD operations

5. **Navigation**:
   - [ ] All links work correctly
   - [ ] Routing functions properly

**Validation**: All core features functional

#### Step 4.3: Security Validation

**Action**: Verify security package updates applied

**Checks**:
- [ ] No security vulnerabilities in packages (per assessment)
- [ ] HTTPS redirection enabled
- [ ] Connection string encryption configured
- [ ] No deprecated security packages

**Validation**: Security posture maintained or improved

#### Step 4.4: Final Commit

**Action**: Commit all changes

**Commit Message**:
```
Upgrade ContosoUniversity to .NET 10

- Converted project to SDK-style
- Migrated from .NET Framework 4.8 to .NET 10
- Converted from ASP.NET Web Forms to ASP.NET Core Razor Pages
- Updated Entity Framework 6.1.3 to 6.5.1
- Migrated from System.Data.SqlClient to Microsoft.Data.SqlClient
- Updated all NuGet packages
- Migrated configuration from web.config to appsettings.json
- Fixed all compilation warnings and errors
- Verified application functionality

Breaking changes addressed:
- Replaced System.Web.UI controls with Razor Pages markup
- Migrated Global.asax to Program.cs
- Updated DbContext registration to use dependency injection
- Updated connection string encryption settings
```

**Validation**: Changes committed

---

## Package Update Reference

### Packages to Update

| Package | Current | Target | Status | Notes |
|---------|---------|--------|--------|-------|
| EntityFramework | 6.1.3 | 6.5.1 | ⚠️ Update | Required for .NET Core support |
| Microsoft.ApplicationInsights | 2.1.0 | 3.0.0+ | ⚠️ Update | Incompatible - major version upgrade |
| Microsoft.ApplicationInsights.Agent.Intercept | 1.2.1 | Remove | ⚠️ Remove | Incompatible with .NET Core |
| Microsoft.ApplicationInsights.DependencyCollector | 2.1.0 | 2.23.0 | ⚠️ Update | Incompatible - needs update |
| Microsoft.ApplicationInsights.PerfCounterCollector | 2.1.0 | 2.23.0 | ⚠️ Update | Incompatible - needs update |
| Microsoft.ApplicationInsights.Web | 2.1.0 | Remove | ⚠️ Remove | Incompatible - use ASP.NET Core integration |
| Microsoft.ApplicationInsights.WindowsServer | 2.1.0 | 2.23.0 | ⚠️ Update | Incompatible - needs update |
| Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel | 2.1.0 | 2.23.0 | ⚠️ Update | Incompatible - needs update |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 1.0.0 | Remove | ✅ Remove | Functionality included in framework |
| Microsoft.Net.Compilers | 1.0.0 | Remove | ✅ Remove | Functionality included in framework |

### Packages to Add

| Package | Version | Reason |
|---------|---------|--------|
| Microsoft.Data.SqlClient | 5.x | Replacement for System.Data.SqlClient |
| Microsoft.AspNetCore.App | (framework) | ASP.NET Core framework reference |

### Package Update Commands

```powershell
# Remove packages
dotnet remove package Microsoft.ApplicationInsights.Agent.Intercept
dotnet remove package Microsoft.ApplicationInsights.Web
dotnet remove package Microsoft.CodeDom.Providers.DotNetCompilerPlatform
dotnet remove package Microsoft.Net.Compilers

# Update packages
dotnet add package EntityFramework --version 6.5.1
dotnet add package Microsoft.ApplicationInsights --version 3.0.0
dotnet add package Microsoft.ApplicationInsights.DependencyCollector --version 2.23.0
dotnet add package Microsoft.ApplicationInsights.PerfCounterCollector --version 2.23.0
dotnet add package Microsoft.ApplicationInsights.WindowsServer --version 2.23.0
dotnet add package Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel --version 2.23.0

# Add new packages
dotnet add package Microsoft.Data.SqlClient --version 5.2.0
```

---

## Breaking Changes Catalog

### Major Breaking Changes

#### 1. ASP.NET Web Forms Removal

**Impact**: 🔴 Critical - Entire application architecture

**Description**: 
- System.Web.UI namespace and all Web Forms controls don't exist in .NET Core
- 168 API issues related to System.Web (68.6% of all issues)

**Migration Path**:
- Convert Web Forms pages to Razor Pages
- Replace server controls with HTML + Tag Helpers
- Replace ViewState with model binding
- Implement new application startup pattern

**Affected APIs**:
- System.Web.UI.Page
- System.Web.UI.WebControls.GridView
- System.Web.UI.WebControls.DetailsView
- System.Web.UI.WebControls.TextBox
- System.Web.UI.WebControls.DropDownList
- System.Web.UI.WebControls.Button
- And 162 more System.Web APIs

#### 2. System.Data.SqlClient Namespace Change

**Impact**: 🟡 Medium - Source incompatibility

**Description**:
- System.Data.SqlClient moved to Microsoft.Data.SqlClient package
- 29 API usages need namespace updates

**Migration Path**:
- Update package reference
- Change using statements
- Verify connection string encryption settings

**Behavioral Changes**:
- Encrypt defaults to true (was false)
- Certificate validation always enabled
- DateTime with DbType.Time requires TimeSpan

**Affected APIs**:
- SqlConnection
- SqlCommand
- SqlDataReader
- SqlParameter
- SqlDataAdapter

#### 3. Configuration System Replacement

**Impact**: 🟡 Medium - API changes required

**Description**:
- ConfigurationManager (web.config/app.config) replaced with IConfiguration
- 18 API issues (7.3% of total)

**Migration Path**:
- Move settings to appsettings.json
- Inject IConfiguration via dependency injection
- Update all configuration access code

**Affected APIs**:
- ConfigurationManager.ConnectionStrings
- ConfigurationManager.AppSettings
- ConnectionStringSettings

#### 4. Entity Framework Initialization

**Impact**: 🟡 Medium - Pattern change

**Description**:
- Global.asax pattern removed
- Database.SetInitializer pattern needs migration

**Migration Path**:
- Move initialization to Program.cs
- Register DbContext in dependency injection
- Update seeding patterns

### Framework-Level Changes

#### .NET Framework 4.8 → .NET 10

**New Features Available**:
- C# 13 language features
- Performance improvements
- Modern BCL APIs
- Nullable reference types (opt-in)

**Potential Issues**:
- Some legacy APIs removed
- Different default behaviors
- Stricter type checking

---

## Risk Assessment & Mitigation

### High-Risk Areas

#### 1. Web Forms to Razor Pages Migration

**Risk Level**: 🔴 High

**Description**: Complete UI framework replacement

**Mitigation**:
- Migrate one page at a time
- Test each page conversion thoroughly
- Keep original .aspx files until verified
- Document UI/UX changes

**Rollback Plan**: Revert to previous commit if critical issues found

#### 2. Database Access Layer

**Risk Level**: 🟡 Medium

**Description**: DbContext initialization and SqlClient migration

**Mitigation**:
- Verify connection strings before deployment
- Test database operations thoroughly
- Validate encryption settings in all environments
- Keep backup of web.config connection strings

**Rollback Plan**: Revert database access code if connection issues

#### 3. Application Insights Integration

**Risk Level**: 🟡 Medium

**Description**: Major version upgrade with potential config changes

**Mitigation**:
- Review Application Insights documentation for v3.x
- Verify telemetry after upgrade
- Test in non-production environment first

**Rollback Plan**: Can temporarily disable if telemetry fails

### Medium-Risk Areas

#### 4. Entity Framework 6.5.1 Compatibility

**Risk Level**: 🟡 Medium

**Description**: Update from 6.1.3 to 6.5.1

**Mitigation**:
- Review EF 6.5 release notes
- Test all database operations
- Verify migrations still work
- Consider future EF Core migration

#### 5. Build System Changes

**Risk Level**: 🟢 Low-Medium

**Description**: SDK-style project conversion

**Mitigation**:
- Use automated conversion tool
- Verify all files included correctly
- Test build in multiple environments
- Document any manual adjustments needed

---

## Testing Strategy

### Multi-Level Testing

#### Pre-Upgrade Baseline
- [ ] Document current application behavior
- [ ] Capture screenshots of all pages
- [ ] Document all features and workflows
- [ ] Note any existing bugs

#### During Upgrade (Per Phase)

**Phase 1: Project Modernization**
- [ ] Project builds after SDK conversion
- [ ] All files included correctly
- [ ] No dependency conflicts

**Phase 2: ASP.NET Core Migration**
- [ ] Application starts
- [ ] Pages load
- [ ] Navigation works
- [ ] Data displays correctly

**Phase 3: API Fixes**
- [ ] No compilation errors
- [ ] Database connectivity works
- [ ] Configuration loads correctly

**Phase 4: Final Validation**
- [ ] All features functional
- [ ] Performance acceptable
- [ ] No security regressions

#### Post-Upgrade Validation

**Functional Testing**:
- [ ] All CRUD operations work
- [ ] Data integrity maintained
- [ ] Search and filtering functional
- [ ] Validation works correctly

**Integration Testing**:
- [ ] Database operations succeed
- [ ] Application Insights telemetry flows
- [ ] Error handling works

**UI Testing**:
- [ ] All pages render correctly
- [ ] Forms submit properly
- [ ] Navigation functional
- [ ] Responsive design works

**Performance Testing**:
- [ ] Page load times acceptable
- [ ] Database query performance
- [ ] Memory usage normal

---

## Success Criteria

### Technical Completion

✅ **All criteria must be met**:

1. **Project Structure**
   - [ ] Project converted to SDK-style
   - [ ] Target framework is net10.0
   - [ ] All package references updated

2. **Build**
   - [ ] Solution builds with 0 errors
   - [ ] Solution builds with 0 warnings
   - [ ] All projects reference compatible packages

3. **Code Quality**
   - [ ] No System.Web dependencies remain
   - [ ] No ConfigurationManager usage remains
   - [ ] System.Data.SqlClient fully migrated to Microsoft.Data.SqlClient
   - [ ] Entity Framework properly initialized

4. **Functionality**
   - [ ] Application starts successfully
   - [ ] All pages load
   - [ ] Database operations work
   - [ ] CRUD operations functional

5. **Security**
   - [ ] No vulnerable packages
   - [ ] Connection string encryption configured
   - [ ] HTTPS enabled
   - [ ] No deprecated security features

### Functional Completion

✅ **Application feature parity**:
- [ ] All student management features work
- [ ] All course management features work
- [ ] All instructor management features work
- [ ] All department management features work
- [ ] Reporting/statistics functional

### Documentation

- [ ] Migration notes documented
- [ ] Breaking changes cataloged
- [ ] Configuration changes documented
- [ ] Deployment guide updated

---

## Implementation Timeline

### Estimated Effort

| Phase | Estimated Time | Complexity |
|-------|---------------|------------|
| Phase 0: Preparation | 0.5 hours | 🟢 Low |
| Phase 1: Project Modernization | 1-2 hours | 🟡 Medium |
| Phase 2: ASP.NET Core Migration | 4-6 hours | 🔴 High |
| Phase 3: API Compatibility Fixes | 2-3 hours | 🟡 Medium |
| Phase 4: Validation & Testing | 2-3 hours | 🟡 Medium |
| **Total** | **10-15 hours** | 🔴 **High** |

**Note**: Estimates assume no major unexpected issues and include testing time.

### Schedule

**Recommended Approach**:
- Execute in single dedicated session (1-2 days)
- Maintain focus through all phases
- Test thoroughly at each phase
- Don't deploy to production until fully validated

---

## Rollback Plan

### Version Control Safety

**Primary Protection**: Git commits per phase
- Each phase commits separately
- Can rollback to any phase completion
- Branch isolation protects main codebase

### Rollback Triggers

**Rollback if**:
- Critical functionality broken
- Database corruption or data loss
- Unresolvable compilation errors
- Performance degradation > 50%
- Security vulnerabilities introduced

### Rollback Procedure

1. **Immediate Rollback** (during upgrade):
   ```bash
   git reset --hard HEAD^  # Rollback last commit
   # OR
   git checkout <previous-phase-commit>
   ```

2. **Selective Rollback** (specific phase):
   ```bash
   git revert <phase-commit-hash>
   ```

3. **Full Rollback** (to pre-upgrade):
   ```bash
   git checkout <pre-upgrade-branch>
   ```

### Post-Rollback Actions

- [ ] Document reason for rollback
- [ ] Analyze what went wrong
- [ ] Plan remediation approach
- [ ] Re-attempt with fixes

---

## Additional Considerations

### Future Enhancements (Post-Upgrade)

**Consider after successful .NET 10 upgrade**:

1. **Entity Framework Core Migration**
   - Current plan keeps EF 6.5.1
   - Consider migrating to EF Core 9 for:
     - Better performance
     - Modern LINQ features
     - Improved tooling

2. **Blazor Evaluation**
   - Razor Pages provide good migration path
   - Could consider Blazor for richer UI in future

3. **Minimal APIs**
   - If adding API endpoints, consider minimal APIs

4. **Performance Optimizations**
   - Profile application after migration
   - Implement caching strategies
   - Optimize database queries

### Dependencies on External Systems

**Verify compatibility**:
- [ ] Database server version supports encryption settings
- [ ] Application Insights workspace configured
- [ ] IIS version supports .NET 10 (if deploying to IIS)
- [ ] Any external service integrations still work

### Deployment Considerations

**For Production Deployment**:
- [ ] Update deployment scripts for .NET 10
- [ ] Install .NET 10 runtime on servers
- [ ] Update IIS configuration if applicable
- [ ] Plan for blue-green or staged deployment
- [ ] Prepare rollback procedure
- [ ] Schedule maintenance window

---

## Appendices

### Appendix A: Tool Reference

**Tools to Use**:
1. **SDK-Style Conversion**: AppModernization-convert_project_to_sdk_style
2. **Project Analysis**: AppModernization-analyze_projects
3. **Package Version Lookup**: AppModernization-get_supported_package_version
4. **Build**: MSBuild (via vswhere) or `dotnet build`

### Appendix B: Key Documentation Links

- [ASP.NET Core Migration Guide](https://learn.microsoft.com/aspnet/core/migration/proper-to-2x/)
- [Entity Framework 6 on .NET Core](https://learn.microsoft.com/ef/ef6/what-is-new/)
- [Microsoft.Data.SqlClient Migration](https://learn.microsoft.com/sql/connect/ado-net/introduction-microsoft-data-sqlclient-namespace)
- [Razor Pages Documentation](https://learn.microsoft.com/aspnet/core/razor-pages/)
- [.NET 10 Breaking Changes](https://learn.microsoft.com/dotnet/core/compatibility/10.0)

### Appendix C: Common Patterns

**Dependency Injection Pattern**:
```csharp
// In Program.cs
builder.Services.AddScoped<IMyService, MyService>();

// In PageModel or Controller
public class SomePageModel : PageModel
{
    private readonly IMyService _service;
    
    public SomePageModel(IMyService service)
    {
        _service = service;
    }
}
```

**Configuration Access**:
```csharp
// In Program.cs
var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

// Via DI
public class SomeClass
{
    private readonly IConfiguration _config;
    
    public SomeClass(IConfiguration config)
    {
        _config = config;
    }
    
    public void UseConfig()
    {
        var value = _config["SomeKey"];
    }
}
```

---

## Conclusion

This plan provides a comprehensive roadmap for upgrading ContosoUniversity from .NET Framework 4.8 to .NET 10. The migration involves not just a framework upgrade but a fundamental architectural shift from ASP.NET Web Forms to ASP.NET Core Razor Pages.

**Key Takeaways**:
- High complexity due to Web Forms to Razor Pages migration
- All-at-once strategy appropriate for single-project solution
- Phased approach with validation at each step
- Estimated 10-15 hours of focused effort
- Strong rollback capabilities via Git commits

**Next Steps**:
1. Review and approve this plan
2. Proceed to Execution stage
3. Execute Phase 0 (Preparation)
4. Continue through phases with thorough testing

The plan is ready for execution. The Execution stage will convert this plan into validated tasks and execute them systematically.

---

*This plan supports the Execution stage of the .NET modernization workflow.*
