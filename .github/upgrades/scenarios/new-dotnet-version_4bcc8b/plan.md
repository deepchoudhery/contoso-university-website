# .NET 10 Upgrade Plan: Contoso University

**Date**: 2025-01-13  
**Target Framework**: .NET 10.0  
**Current Framework**: .NET Framework 4.8  
**Strategy**: All-At-Once  

---

## Executive Summary

### Upgrade Overview

This plan outlines the upgrade of the Contoso University web application from .NET Framework 4.8 to .NET 10.0. This is a **major architectural migration** requiring conversion from ASP.NET Framework (Web Forms) to ASP.NET Core.

**Complexity Assessment**: 🔴 **HIGH**
- **Root Cause**: ASP.NET Web Forms to ASP.NET Core migration required
- **Codebase Size**: 1,314 lines of code across 26 files
- **Estimated Impact**: 245+ lines requiring modification (18.6% of codebase)
- **API Issues**: 245 total (169 binary incompatible, 76 source incompatible)

### Selected Strategy

**All-At-Once Strategy** - Complete migration in a single coordinated operation.

**Rationale**:
- Single-project solution (no complex dependencies)
- Small codebase (~1,300 LOC)
- Web Forms architecture requires complete rewrite anyway
- All-at-once is more efficient than phased approach for single project
- Clear migration path documented

### Critical Challenges

1. **ASP.NET Web Forms Architecture** (168 incompatible APIs)
   - System.Web.UI controls (GridView, DetailsView, TextBox, etc.)
   - Page lifecycle and ViewState management
   - Web Forms event model
   - **Resolution**: Migrate to ASP.NET Core MVC/Razor Pages

2. **Legacy Project Format**
   - Non-SDK-style project requires conversion
   - **Resolution**: Convert to SDK-style before framework upgrade

3. **Data Access Migration**
   - System.Data.SqlClient → Microsoft.Data.SqlClient
   - Configuration-based connection strings
   - **Resolution**: Update namespaces and configuration approach

4. **Entity Framework 6 on .NET Core**
   - EF6 can run on .NET Core but requires specific configuration
   - **Resolution**: Update EF6 package and migrate DbContext initialization

5. **Configuration System**
   - Web.config → appsettings.json migration
   - ConfigurationManager API changes
   - **Resolution**: Migrate to Microsoft.Extensions.Configuration

---

## Assessment Summary

### Current State

**Project**: ContosoUniversity.csproj
- **Type**: ASP.NET Web Application Project (WAP)
- **Target Framework**: .NET Framework 4.8
- **SDK-Style**: No (requires conversion)
- **Files**: 54 total, 15 with compatibility issues
- **Lines of Code**: 1,314
- **Dependencies**: None (single project solution)

### Technology Stack Analysis

| Technology | Current State | Issues | Migration Path |
|:---|:---|:---:|:---|
| ASP.NET Web Forms | System.Web.UI.* | 168 | Migrate to ASP.NET Core MVC/Razor Pages |
| Entity Framework | 6.1.3 | Minor | Upgrade to 6.5.1 and configure for .NET Core |
| Data Access | System.Data.SqlClient | 76 | Migrate to Microsoft.Data.SqlClient |
| Configuration | System.Configuration | 18 | Migrate to Microsoft.Extensions.Configuration |
| Application Insights | 2.1.0 (legacy packages) | 7 | Replace with modern ASP.NET Core packages |

### Package Compatibility

| Package | Current | Status | Target | Action |
|:---|:---:|:---|:---:|:---|
| EntityFramework | 6.1.3 | 🔄 Upgrade | 6.5.1 | Update version |
| Microsoft.ApplicationInsights | 2.1.0 | ⚠️ Incompatible | 3.0.0+ | Replace with ASP.NET Core packages |
| Microsoft.ApplicationInsights.Agent.Intercept | 1.2.1 | ⚠️ Incompatible | N/A | Remove |
| Microsoft.ApplicationInsights.DependencyCollector | 2.1.0 | ⚠️ Incompatible | 2.23.0 | Update |
| Microsoft.ApplicationInsights.Web | 2.1.0 | ⚠️ Incompatible | N/A | Replace |
| Microsoft.ApplicationInsights.WindowsServer* | 2.1.0 | ⚠️ Incompatible | 2.23.0+ | Update/Remove |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 1.0.0 | ℹ️ Framework | N/A | Remove |
| Microsoft.Net.Compilers | 1.0.0 | ℹ️ Framework | N/A | Remove |

### API Migration Challenges

**Top Incompatible APIs**:
- System.Web.UI.WebControls.* (GridView, DetailsView, TextBox, etc.) - 15+ occurrences
- System.Web.UI.Page and lifecycle - 5 occurrences
- System.Data.SqlClient.* - 8 occurrences
- System.Configuration.ConfigurationManager - 3 occurrences

---

## Implementation Strategy

### Phase 0: Preparation & Prerequisites

**Objective**: Ensure environment and repository are ready for migration

**Prerequisites Checklist**:
- [ ] .NET 10 SDK installed and verified
- [ ] Working tree is clean (no uncommitted changes)
- [ ] Current solution builds successfully on .NET Framework 4.8
- [ ] Database connection string is available and valid
- [ ] Backup created (if needed)

**Validation**:
```powershell
# Verify .NET 10 SDK
dotnet --list-sdks | Select-String "10.0"

# Verify clean working tree
git status

# Build current solution
msbuild ContosoUniversity_Project.sln /p:Configuration=Release
```

### Phase 1: SDK-Style Conversion

**Objective**: Convert legacy project format to SDK-style while maintaining .NET Framework 4.8 target

**Why First**: 
- SDK-style is required for .NET Core/.NET 5+
- Separates structural changes from framework changes
- Easier to troubleshoot issues in isolation

**Operations**:
1. Convert ContosoUniversity.csproj to SDK-style format
2. Build and validate conversion
3. Remove packages.config (migrated to PackageReference)
4. Commit conversion changes

**Key Constraints**:
- ⚠️ **DO NOT** change TargetFramework during this phase
- ⚠️ **DO NOT** upgrade NuGet packages during this phase
- Only fix build issues directly caused by SDK conversion

**Expected Changes**:
- Project file structure modernized
- PackageReferences inlined in .csproj
- Simplified build configuration
- Removal of AssemblyInfo.cs (handled by SDK)

**Validation Criteria**:
- [ ] Project converted to SDK-style
- [ ] Still targets net48
- [ ] Solution builds without errors
- [ ] Solution builds without warnings
- [ ] packages.config removed

**Tools**: Use `convert_project_to_sdk_style` tool

### Phase 2: ASP.NET Core Migration

**Objective**: Migrate from ASP.NET Web Forms to ASP.NET Core

**Critical Decision**: Due to the fundamental architectural differences between Web Forms and ASP.NET Core, this requires:

**Approach**: Rewrite using ASP.NET Core MVC/Razor Pages pattern

**Sub-Steps**:

#### 2.1: Create ASP.NET Core Project Structure
- Update project file for ASP.NET Core Web App
- Add ASP.NET Core framework reference
- Create Program.cs with minimal hosting model
- Set up dependency injection container

#### 2.2: Migrate Data Access Layer
- Update to Microsoft.Data.SqlClient
- Update EF6 to 6.5.1 with .NET Core compatibility
- Migrate DbContext initialization from Global.asax to DI
- Update connection string configuration

#### 2.3: Migrate Configuration
- Replace Web.config with appsettings.json
- Migrate connection strings
- Set up configuration providers
- Update code to use IConfiguration

#### 2.4: Migrate UI Layer
- Convert Web Forms pages to Razor Pages or MVC Views
- Migrate page models and code-behind logic
- Replace ViewState with proper state management
- Migrate data binding to Razor syntax
- Convert GridView/DetailsView to HTML/Razor tables with model binding

#### 2.5: Migrate Application Insights
- Remove legacy Application Insights packages
- Add Microsoft.ApplicationInsights.AspNetCore
- Configure telemetry in Program.cs

**Key Files Requiring Migration**:

Based on assessment, these file types will require changes:
- **.aspx files**: Convert to .cshtml Razor Pages/Views
- **.aspx.cs code-behind**: Migrate logic to PageModel or Controller
- **Global.asax.cs**: Migrate initialization to Program.cs
- **Web.config**: Convert to appsettings.json
- **DAL classes**: Update namespaces and DI registration

**Breaking Changes to Address**:

1. **Web Forms Controls → Razor Syntax**
   ```csharp
   // Old: Web Forms
   <asp:GridView ID="studentsGrid" runat="server" />
   studentsGrid.DataSource = students;
   studentsGrid.DataBind();
   
   // New: Razor Pages
   @foreach(var student in Model.Students) {
       <tr><td>@student.Name</td></tr>
   }
   ```

2. **Page Lifecycle → Middleware Pipeline**
   ```csharp
   // Old: Global.asax.cs
   protected void Application_Start() { }
   
   // New: Program.cs
   var builder = WebApplication.CreateBuilder(args);
   // Configure services
   var app = builder.Build();
   // Configure middleware
   ```

3. **Configuration Access**
   ```csharp
   // Old
   ConfigurationManager.ConnectionStrings["ContosoUniversity"].ConnectionString
   
   // New
   configuration.GetConnectionString("ContosoUniversity")
   ```

4. **SqlClient Namespace**
   ```csharp
   // Old
   using System.Data.SqlClient;
   
   // New
   using Microsoft.Data.SqlClient;
   ```

5. **EF DbContext Registration**
   ```csharp
   // Old: Global.asax.cs
   Database.SetInitializer(...);
   
   // New: Program.cs
   builder.Services.AddDbContext<SchoolContext>(options =>
       options.UseSqlServer(connectionString));
   ```

### Phase 3: Update Target Framework & Packages

**Objective**: Update to .NET 10.0 and modern package versions

**Operations** (performed as single coordinated batch):

#### 3.1: Update Project Target Framework
```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
</PropertyGroup>
```

#### 3.2: Update Package References

**Core Packages**:
- Add: Microsoft.AspNetCore.App (framework reference)
- Update: EntityFramework 6.1.3 → 6.5.1
- Add: Microsoft.Data.SqlClient (latest stable)
- Remove: All System.Web.* packages (incompatible)
- Remove: Microsoft.CodeDom.Providers.DotNetCompilerPlatform
- Remove: Microsoft.Net.Compilers

**Application Insights Packages**:
- Remove: Microsoft.ApplicationInsights.Web
- Remove: Microsoft.ApplicationInsights.WindowsServer*
- Remove: Microsoft.ApplicationInsights.Agent.Intercept
- Add: Microsoft.ApplicationInsights.AspNetCore (latest)

**Configuration Packages**:
- Add: Microsoft.Extensions.Configuration.Json
- Add: Microsoft.Extensions.Configuration.EnvironmentVariables

#### 3.3: Restore and Build
```powershell
dotnet restore
dotnet build
```

#### 3.4: Fix Compilation Errors
- Address remaining API incompatibilities
- Update deprecated API usage
- Fix namespace references
- Handle breaking changes

**Validation Criteria**:
- [ ] Project targets net10.0
- [ ] All packages compatible with .NET 10
- [ ] Solution builds without errors
- [ ] Solution builds without warnings

### Phase 4: Testing & Validation

**Objective**: Ensure migrated application functions correctly

**Test Levels**:

#### 4.1: Build Validation
- [ ] Clean build succeeds
- [ ] No compiler errors
- [ ] No compiler warnings
- [ ] All dependencies resolved

#### 4.2: Runtime Validation
- [ ] Application starts successfully
- [ ] Database connection works
- [ ] All pages/routes accessible
- [ ] No runtime exceptions on startup

#### 4.3: Functional Validation
- [ ] Student listing displays correctly
- [ ] Student details view works
- [ ] Create/Edit/Delete operations function
- [ ] Database queries execute properly
- [ ] Application Insights telemetry collected

#### 4.4: Data Access Validation
- [ ] Entity Framework context initializes
- [ ] Database queries return correct results
- [ ] Connection string configuration works
- [ ] SQL Server connectivity confirmed

**Test Commands**:
```powershell
# Build
dotnet build --configuration Release

# Run application
dotnet run --project ContosoUniversity\ContosoUniversity.csproj

# Check for warnings
dotnet build --configuration Release /warnaserror
```

### Phase 5: Final Validation & Cleanup

**Objective**: Ensure migration is complete and production-ready

**Cleanup Tasks**:
- [ ] Remove obsolete files (Global.asax, Web.config, .aspx files)
- [ ] Remove unused using statements
- [ ] Remove commented-out legacy code
- [ ] Clean up project references
- [ ] Remove packages directory

**Documentation Updates**:
- [ ] Update README with .NET 10 requirements
- [ ] Document configuration changes
- [ ] Note any behavioral changes
- [ ] Update deployment instructions

**Final Validation**:
- [ ] Full regression test pass
- [ ] Performance baseline acceptable
- [ ] No security vulnerabilities (run security scan)
- [ ] Application Insights working
- [ ] All critical features functional

---

## Detailed File-Level Changes

### Files Requiring Migration

Based on typical Contoso University structure:

#### UI Layer (.aspx → .cshtml)
- **Students.aspx** → **Pages/Students/Index.cshtml**
  - Migrate GridView to Razor table
  - Convert code-behind to PageModel
  - Update data binding

- **StudentDetails.aspx** → **Pages/Students/Details.cshtml**
  - Migrate DetailsView to Razor HTML
  - Convert code-behind to PageModel
  - Update parameter handling

- **StudentEdit.aspx** → **Pages/Students/Edit.cshtml**
  - Migrate form controls to Razor inputs
  - Convert validation
  - Update postback handling

- **StudentCreate.aspx** → **Pages/Students/Create.cshtml**
  - Similar to Edit page migration

- **StudentDelete.aspx** → **Pages/Students/Delete.cshtml**
  - Migrate confirmation UI
  - Convert delete logic

#### Application Files
- **Global.asax.cs** → **Program.cs**
  - Application startup
  - DbContext initialization
  - Application Insights configuration

- **Web.config** → **appsettings.json**
  - Connection strings
  - Application settings
  - Logging configuration

#### Data Access Layer
- **SchoolContext.cs** (DAL folder)
  - Update: System.Data.Entity → (stays same for EF6)
  - Update: System.Data.SqlClient → Microsoft.Data.SqlClient
  - Remove: Database initializer from Global.asax
  - Add: DI registration in Program.cs

- **Repository classes** (if any)
  - Update: ConfigurationManager usage
  - Update: SqlClient namespace
  - Add: Constructor injection for configuration

#### Model Classes
- **Student.cs, Course.cs, Enrollment.cs** (Models folder)
  - Minimal changes expected
  - May need data annotation updates
  - Validation attribute updates if needed

---

## Package Update Reference

### Packages to Remove

| Package | Reason |
|:---|:---|
| Microsoft.ApplicationInsights.Web | ASP.NET Framework specific |
| Microsoft.ApplicationInsights.WindowsServer | Replaced by AspNetCore package |
| Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel | Replaced by AspNetCore package |
| Microsoft.ApplicationInsights.PerfCounterCollector | Replaced by AspNetCore package |
| Microsoft.ApplicationInsights.DependencyCollector | Replaced by AspNetCore package |
| Microsoft.ApplicationInsights.Agent.Intercept | No longer needed |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | Framework functionality |
| Microsoft.Net.Compilers | Framework functionality |

### Packages to Add

| Package | Version | Purpose |
|:---|:---:|:---|
| Microsoft.Data.SqlClient | Latest | Replaces System.Data.SqlClient |
| Microsoft.ApplicationInsights.AspNetCore | Latest | Modern Application Insights |
| Microsoft.Extensions.Configuration.Json | Latest | JSON configuration |
| System.Configuration.ConfigurationManager | Latest | Bridge for ConfigurationManager (if needed temporarily) |

### Packages to Update

| Package | From | To | Notes |
|:---|:---:|:---:|:---|
| EntityFramework | 6.1.3 | 6.5.1 | .NET Core compatible version |
| Microsoft.ApplicationInsights | 2.1.0 | Latest | Core telemetry |

---

## Breaking Changes Catalog

### ASP.NET Framework → ASP.NET Core

#### UI Model Changes

**Web Forms → Razor Pages**
- **Impact**: Complete UI rewrite required
- **Scope**: All .aspx files
- **Resolution**: 
  - Create corresponding .cshtml Razor Pages
  - Migrate UI logic to PageModel classes
  - Replace server controls with HTML helpers/tag helpers

**ViewState Elimination**
- **Impact**: State management changes
- **Scope**: All pages using ViewState
- **Resolution**: 
  - Use proper HTTP POST for updates
  - Store state in session if needed
  - Use TempData for cross-request data

**Page Lifecycle**
- **Impact**: Event model changes
- **Scope**: All code-behind files
- **Resolution**:
  - Migrate Page_Load → OnGet/OnPost
  - Migrate event handlers → handler methods
  - Update initialization logic

#### Server Controls

**GridView → Razor Tables**
```csharp
// Before: Web Forms
<asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
    </Columns>
</asp:GridView>

// Code-behind
gvStudents.DataSource = students;
gvStudents.DataBind();

// After: Razor Pages
<table class="table">
    <thead>
        <tr><th>Last Name</th></tr>
    </thead>
    <tbody>
        @foreach(var student in Model.Students) {
            <tr><td>@student.LastName</td></tr>
        }
    </tbody>
</table>

// PageModel
public IList<Student> Students { get; set; }
public void OnGet() {
    Students = _context.Students.ToList();
}
```

**DetailsView → Razor HTML**
```csharp
// Before: Web Forms
<asp:DetailsView ID="dvStudent" runat="server" AutoGenerateRows="false">
    <Fields>
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
    </Fields>
</asp:DetailsView>

// After: Razor Pages
<dl class="row">
    <dt class="col-sm-2">First Name</dt>
    <dd class="col-sm-10">@Model.Student.FirstName</dd>
</dl>

// PageModel
public Student Student { get; set; }
public async Task OnGetAsync(int id) {
    Student = await _context.Students.FindAsync(id);
}
```

### Configuration System

**Web.config → appsettings.json**
```xml
<!-- Before: Web.config -->
<connectionStrings>
    <add name="SchoolContext" 
         connectionString="Data Source=.;Initial Catalog=ContosoUniversity;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

```json
// After: appsettings.json
{
  "ConnectionStrings": {
    "SchoolContext": "Data Source=.;Initial Catalog=ContosoUniversity;Integrated Security=True;Encrypt=False"
  }
}
```

**ConfigurationManager → IConfiguration**
```csharp
// Before
var connString = ConfigurationManager.ConnectionStrings["SchoolContext"].ConnectionString;

// After (inject IConfiguration)
var connString = _configuration.GetConnectionString("SchoolContext");
```

### Data Access

**System.Data.SqlClient → Microsoft.Data.SqlClient**
```csharp
// Before
using System.Data.SqlClient;

// After
using Microsoft.Data.SqlClient;
```

**Connection String Encryption**
- **Impact**: New default requires explicit encryption setting
- **Resolution**: Add `Encrypt=False` or configure proper certificate

### Entity Framework 6

**DbContext Initialization**
```csharp
// Before: Global.asax.cs
protected void Application_Start()
{
    Database.SetInitializer(new SchoolInitializer());
}

// After: Program.cs
builder.Services.AddDbContext<SchoolContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SchoolContext")
    );
});

// For EF6-specific initialization (if needed)
Database.SetInitializer(new SchoolInitializer());
```

### Application Insights

**Package Migration**
```csharp
// Before: Global.asax.cs
using Microsoft.ApplicationInsights;
protected void Application_Start()
{
    // Automatically initialized
}

// After: Program.cs
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});
```

---

## Risk Assessment & Mitigation

### High Risks

#### 1. Architectural Migration Complexity
- **Risk**: ASP.NET Web Forms to ASP.NET Core is fundamental rewrite
- **Likelihood**: High
- **Impact**: High
- **Mitigation**:
  - Follow incremental migration approach within phase
  - Test each page conversion individually
  - Keep Web Forms version as reference
  - Document conversion patterns

#### 2. Data Access Changes
- **Risk**: SqlClient and EF6 behavior differences on .NET Core
- **Likelihood**: Medium
- **Impact**: High
- **Mitigation**:
  - Test database operations thoroughly
  - Verify connection string encryption settings
  - Test all CRUD operations
  - Monitor for subtle behavior changes

#### 3. State Management
- **Risk**: ViewState removal requires alternative patterns
- **Likelihood**: High
- **Impact**: Medium
- **Mitigation**:
  - Identify ViewState usage before migration
  - Design proper state management strategy
  - Use session state where appropriate
  - Document state handling patterns

### Medium Risks

#### 4. Configuration Migration
- **Risk**: Missing or incorrect configuration values
- **Likelihood**: Medium
- **Impact**: Medium
- **Mitigation**:
  - Create comprehensive configuration mapping
  - Validate all settings migrated
  - Test configuration loading
  - Maintain Web.config as reference

#### 5. Third-Party Dependencies
- **Risk**: Application Insights behavior changes
- **Likelihood**: Low
- **Impact**: Medium
- **Mitigation**:
  - Review Application Insights documentation
  - Test telemetry collection
  - Verify dashboard compatibility
  - Have rollback plan

### Low Risks

#### 6. Build Configuration
- **Risk**: SDK-style project build issues
- **Likelihood**: Low
- **Impact**: Low
- **Mitigation**:
  - Use official conversion tool
  - Test build immediately after conversion
  - Keep original project as backup

---

## Rollback Strategy

### Rollback Points

**After SDK Conversion** (Phase 1):
```powershell
git revert <commit-hash-of-sdk-conversion>
git push
```

**After ASP.NET Core Migration** (Phase 2):
```powershell
git revert <commit-hash-of-aspnetcore-migration>
git push
```

**After Framework Update** (Phase 3):
```powershell
git revert <commit-hash-of-framework-update>
git push
```

### Rollback Criteria

Trigger rollback if:
- Critical functionality broken beyond repair timeline
- Performance degradation > 50%
- Data integrity issues discovered
- Blocker issues with no known resolution
- Business requirements cannot be met

### Recovery Time

- **SDK Conversion Rollback**: < 15 minutes
- **Full Migration Rollback**: < 30 minutes
- **Rebuild from backup**: < 1 hour

---

## Success Criteria

### Technical Criteria

The upgrade is complete when:

1. **Build Success**
   - [ ] Solution builds without errors
   - [ ] Solution builds without warnings
   - [ ] All dependencies resolve correctly

2. **Framework Targets**
   - [ ] Project targets .NET 10.0
   - [ ] SDK-style project format
   - [ ] Modern package references

3. **Runtime Success**
   - [ ] Application starts without errors
   - [ ] All pages/routes load
   - [ ] No runtime exceptions in happy path

4. **Functional Success**
   - [ ] All CRUD operations work
   - [ ] Database connectivity confirmed
   - [ ] Entity Framework queries execute
   - [ ] Application Insights collecting data

5. **Code Quality**
   - [ ] No deprecated API usage
   - [ ] No security vulnerabilities in packages
   - [ ] Proper error handling maintained
   - [ ] Logging functional

### Business Criteria

- [ ] All critical user workflows functional
- [ ] Performance acceptable (comparable to original)
- [ ] Data integrity maintained
- [ ] Feature parity achieved (or documented exceptions)

### Documentation Criteria

- [ ] README updated with .NET 10 requirements
- [ ] Configuration changes documented
- [ ] Deployment instructions updated
- [ ] Breaking changes documented

---

## Source Control Strategy

### Commit Strategy

**Recommended**: Multiple logical commits for traceability

**Commit 1: SDK Conversion**
```
feat: Convert ContosoUniversity to SDK-style project

- Convert project file to SDK-style format
- Migrate packages.config to PackageReference
- Remove obsolete AssemblyInfo attributes
- Target framework remains net48
```

**Commit 2: ASP.NET Core Migration**
```
feat: Migrate from ASP.NET Web Forms to ASP.NET Core

- Convert .aspx pages to Razor Pages
- Migrate Global.asax to Program.cs
- Replace Web.config with appsettings.json
- Update Application Insights configuration
- Migrate DbContext initialization
- Still targeting net48 temporarily
```

**Commit 3: Framework & Package Updates**
```
feat: Upgrade to .NET 10.0

- Update TargetFramework to net10.0
- Update EntityFramework to 6.5.1
- Migrate to Microsoft.Data.SqlClient
- Update Application Insights packages
- Remove incompatible packages
```

**Commit 4: Fix Compilation Issues**
```
fix: Resolve .NET 10 compilation issues

- Fix API incompatibilities
- Update deprecated API usage
- Resolve namespace conflicts
```

**Commit 5: Final Cleanup**
```
chore: Clean up after .NET 10 migration

- Remove obsolete files
- Clean up unused usings
- Update documentation
```

### Branch Strategy

- **Source Branch**: `copilot/upgrade-dotnet-10` (current)
- **PR Target**: `main` (or default branch)
- **Branch Protection**: Maintain until testing complete

---

## Timeline Estimate

### Time Estimates (for reference, not constraints)

| Phase | Description | Estimated Effort | Dependencies |
|:---:|:---|:---:|:---|
| 0 | Preparation | 30 minutes | None |
| 1 | SDK Conversion | 1-2 hours | Phase 0 |
| 2 | ASP.NET Core Migration | 8-16 hours | Phase 1 |
| 3 | Framework & Package Update | 2-4 hours | Phase 2 |
| 4 | Testing & Validation | 4-8 hours | Phase 3 |
| 5 | Final Cleanup | 1-2 hours | Phase 4 |
| **Total** | **End-to-End** | **16-32 hours** | Sequential |

**Note**: Times assume:
- Experienced developer familiar with both Web Forms and ASP.NET Core
- No major unexpected issues
- Database available for testing
- Includes testing time

### Factors That May Increase Duration

- Complex business logic in code-behind files
- Extensive ViewState usage
- Custom server controls
- Complex data binding scenarios
- Insufficient test coverage requiring manual testing
- Learning curve for ASP.NET Core patterns

---

## Additional Considerations

### Performance Expectations

**ASP.NET Core Performance**:
- Generally faster than ASP.NET Framework
- Lower memory footprint
- Better scalability
- Expect 20-30% improvement in throughput

### Breaking Behavioral Changes

1. **Request Pipeline**
   - Middleware order matters (unlike HTTP modules)
   - Filters vs ActionFilters have different execution

2. **Dependency Injection**
   - Scoped lifetime vs per-request
   - Service locator anti-pattern must be avoided

3. **Configuration**
   - Case-sensitive in Linux deployments
   - Environment-specific overrides work differently

### Future Upgrade Path

After .NET 10:
- **Consider EF Core**: Migrate from EF6 to EF Core for better performance
- **Modern UI**: Consider Blazor or modern SPA framework
- **Minimal APIs**: For simple APIs, consider minimal API pattern
- **gRPC**: For service-to-service communication

---

## Conclusion

This plan provides a comprehensive roadmap for upgrading Contoso University from .NET Framework 4.8 to .NET 10.0. The migration requires a fundamental architectural change from ASP.NET Web Forms to ASP.NET Core, but follows a structured approach that minimizes risk.

The all-at-once strategy is appropriate for this single-project solution, allowing the entire migration to be completed in a single coordinated effort rather than maintaining hybrid states.

**Key Success Factors**:
1. Follow phases sequentially
2. Test thoroughly after each phase
3. Document all changes
4. Maintain original code as reference
5. Allow adequate time for testing

**Next Steps**: 
- Review and approve this plan
- Proceed to Execution stage
- Execute phases in order
- Validate at each checkpoint

---

*This plan will be executed in the Execution stage using a detailed task breakdown and progress tracking.*
