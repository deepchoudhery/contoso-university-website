# .NET 10 Upgrade Plan: ContosoUniversity_Project.sln

**Date**: 2026-02-20  
**Target Framework**: net10.0  
**Source Framework**: net48  
**Strategy**: All-At-Once (single project solution)

---

## Executive Summary

The ContosoUniversity project is an ASP.NET Web Forms application targeting .NET Framework 4.8. Upgrading to .NET 10 requires a significant architectural transformation because ASP.NET Web Forms is not supported in .NET 5+. The migration path involves:

1. Converting the project from a legacy WAP to an SDK-style ASP.NET Core Web Application
2. Replacing Web Forms pages (.aspx) with ASP.NET Core Razor Pages
3. Migrating Entity Framework 6 initialization to ASP.NET Core DI
4. Migrating System.Data.SqlClient to Microsoft.Data.SqlClient
5. Replacing the legacy XML configuration system with ASP.NET Core configuration
6. Updating all incompatible NuGet packages

**Difficulty**: 🔴 High  
**Estimated LOC to modify**: 245+ lines (18.6% of codebase)

---

## Upgrade Strategy

**Selected: All-At-Once**

Justification:
- Solution has only 1 project (fewer than 5)
- Simple dependency structure (no project-to-project dependencies)
- Small total codebase (1,314 LOC)
- Single comprehensive transformation required

---

## Dependency Analysis

The solution has a single project with no project-to-project dependencies:

```
ContosoUniversity.csproj (net48 → net10.0)
```

**Upgrade Order**: Single phase - ContosoUniversity.csproj

---

## Project-by-Project Plan

### ContosoUniversity\ContosoUniversity.csproj

**Current Framework**: net48  
**Target Framework**: net10.0  
**Project Type**: WAP (Web Application Project) → ASP.NET Core Web Application  
**Risk**: 🔴 High

#### Steps:

**Step 1: Convert to SDK-style project**
- Use `convert_project_to_sdk_style` tool to convert the legacy project format
- Remove `packages.config` after conversion
- Remove incompatible MSBuild imports (Microsoft.Net.Compilers, Microsoft.CodeDom.Providers.DotNetCompilerPlatform)

**Step 2: Update TargetFramework**
- Change `TargetFrameworkVersion` from `v4.8` to `net10.0`
- Change output type from Library (WAP) to the ASP.NET Core pattern

**Step 3: Replace incompatible NuGet packages**
See Package Update Reference section below.

**Step 4: Create ASP.NET Core application structure**
- Create `Program.cs` with ASP.NET Core host builder
- Create `appsettings.json` with connection strings from `Web.config`
- Add Razor Pages support

**Step 5: Migrate Web Forms pages to Razor Pages**
- Convert `Home.aspx` → `Pages/Home.cshtml` + `Pages/Home.cshtml.cs`
- Convert `About.aspx` → `Pages/About.cshtml` + `Pages/About.cshtml.cs`
- Convert `Students.aspx` → `Pages/Students.cshtml` + `Pages/Students.cshtml.cs`
- Convert `Courses.aspx` → `Pages/Courses.cshtml` + `Pages/Courses.cshtml.cs`
- Convert `Instructors.aspx` → `Pages/Instructors.cshtml` + `Pages/Instructors.cshtml.cs`
- Convert `Site.Master` → `Pages/Shared/_Layout.cshtml`

**Step 6: Migrate Entity Framework setup**
- Follow `ef-dbcontext-migration` skill
- Move DbContext registration to `Program.cs`
- Update Model1.Context.cs constructor to accept connection string
- Migrate connection string from `Web.config` to `appsettings.json`

**Step 7: Migrate System.Data.SqlClient → Microsoft.Data.SqlClient**
- Follow `sqlclient-migration` skill
- Update all using statements in BLL files
- Review connection string encryption settings

**Step 8: Update configuration**
- Replace `System.Configuration.ConfigurationManager` with `IConfiguration`
- Migrate `Web.config` connection strings to `appsettings.json`

**Step 9: Fix remaining API incompatibilities**
- Replace `System.Web.UI.*` types with ASP.NET Core equivalents in Razor Pages

**Step 10: Build verification**
- Build the project
- Fix all compiler errors and warnings

---

## Package Update Reference

| Package | Current Version | Target Version | Action | Reason |
| :--- | :---: | :---: | :--- | :--- |
| EntityFramework | 6.1.3 | 6.5.1 | Upgrade | Security & compatibility |
| Microsoft.ApplicationInsights | 2.1.0 | Remove | Remove | Not compatible with net10.0; use OpenTelemetry instead |
| Microsoft.ApplicationInsights.Agent.Intercept | 1.2.1 | Remove | Remove | Incompatible with net10.0 |
| Microsoft.ApplicationInsights.DependencyCollector | 2.1.0 | Remove | Remove | Incompatible with net10.0 |
| Microsoft.ApplicationInsights.PerfCounterCollector | 2.1.0 | Remove | Remove | Incompatible with net10.0 |
| Microsoft.ApplicationInsights.Web | 2.1.0 | Remove | Remove | Incompatible with net10.0 (web-specific) |
| Microsoft.ApplicationInsights.WindowsServer | 2.1.0 | Remove | Remove | Incompatible with net10.0 |
| Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel | 2.1.0 | Remove | Remove | Incompatible with net10.0 |
| Microsoft.CodeDom.Providers.DotNetCompilerPlatform | 1.0.0 | Remove | Remove | Included with framework |
| Microsoft.Net.Compilers | 1.0.0 | Remove | Remove | Included with framework |
| **New packages to add** | | | | |
| Microsoft.Data.SqlClient | - | latest | Add | Replace System.Data.SqlClient |
| Microsoft.AspNetCore.Mvc.RazorPages | - | included | Included | Part of ASP.NET Core SDK |

---

## Breaking Changes Catalog

### Critical: System.Web Not Available in .NET 10

**All System.Web.UI.* types** (TextBox, GridView, DetailsView, Page, etc.) are not available.

**Migration path**: Replace Web Forms pages with Razor Pages:
- Controls → HTML + Tag Helpers
- Code-behind → Razor Page Models
- Master Pages → _Layout.cshtml

### System.Data.SqlClient → Microsoft.Data.SqlClient

- `System.Data.SqlClient` namespace changes to `Microsoft.Data.SqlClient`
- Default `Encrypt` behavior changed (now `true` by default)
- Follow `sqlclient-migration` skill for full details

### Legacy Configuration System

- `System.Configuration.ConfigurationManager` requires the `System.Configuration.ConfigurationManager` NuGet package, or migrate to `Microsoft.Extensions.Configuration`
- `Web.config` → `appsettings.json`

### Entity Framework 6 in .NET 10

- EF6 can run on .NET 10 via the `EntityFramework` NuGet package (version 6.5.x)
- DbContext registration needs to move to ASP.NET Core DI

---

## Testing Strategy

### After Conversion:
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Application starts successfully
- [ ] Home page loads
- [ ] Students page loads and displays data
- [ ] Courses page loads and displays data
- [ ] Instructors page loads and displays data
- [ ] About page loads

---

## Risk Management

### Risks

1. **Web Forms to Razor Pages migration** (High)
   - All UI code must be rewritten
   - GridView, DetailsView controls don't exist in Razor Pages
   - Mitigation: Use HTML tables with model data binding

2. **Entity Framework compatibility** (Medium)
   - EF6 is supported on .NET 10 but with limited updates
   - EDMX-based model may need special handling
   - Mitigation: Keep EF6, upgrade to 6.5.x

3. **AjaxControlToolkit** (High)
   - AjaxControlToolkit is Web Forms-specific and not compatible with .NET 10
   - Must be removed; functionality replicated with HTML/JavaScript

---

## Success Criteria

1. ✅ All projects target net10.0
2. ✅ All package updates applied
3. ✅ Project builds without errors
4. ✅ Project builds without warnings
5. ✅ All pages functional (Home, Students, Courses, Instructors, About)
6. ✅ Database connectivity verified
7. ✅ No security vulnerabilities remain

---

*This plan is ready for Execution stage.*
