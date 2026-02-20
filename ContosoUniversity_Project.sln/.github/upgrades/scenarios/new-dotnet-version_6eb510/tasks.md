# ContosoUniversity .NET 10 Upgrade Tasks

## Overview

This document tracks the execution of the ContosoUniversity upgrade from .NET Framework 4.8 to .NET 10. The single project will be upgraded using a phased approach that addresses SDK conversion, ASP.NET Core migration, and API compatibility.

**Progress**: 0/5 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites and prepare environment
**References**: #plan:Phase-0-Preparation

- [ ] (1) Verify .NET 10 SDK is installed per #plan:Phase-0
- [ ] (2) SDK version meets requirements (Verify)
- [ ] (3) Build solution with MSBuild using vswhere to establish baseline per #plan:Phase-0
- [ ] (4) Baseline build succeeds (Verify)

### [ ] TASK-002: Convert project to SDK-style and update target framework
**References**: #plan:Phase-1-Project-Modernization

- [ ] (1) Convert ContosoUniversity.csproj to SDK-style using conversion tool per #plan:Phase-1-Project-Modernization
- [ ] (2) Project converted successfully and packages.config removed (Verify)
- [ ] (3) Build converted project with MSBuild to verify conversion
- [ ] (4) Converted project builds on net48 (Verify)
- [ ] (5) Update target framework from net48 to net10.0 in ContosoUniversity.csproj
- [ ] (6) Target framework updated to net10.0 (Verify)
- [ ] (7) Update package references per #plan:Package-Update-Reference (key: EntityFramework 6.5.1, Microsoft.Data.SqlClient 5.2.0, remove incompatible packages)
- [ ] (8) All package references updated (Verify)
- [ ] (9) Restore NuGet dependencies
- [ ] (10) Dependencies restored successfully (Verify)
- [ ] (11) Commit changes with message: "TASK-002: Phase 1 - Project modernization complete"

### [ ] TASK-003: Migrate to ASP.NET Core and update configuration
**References**: #plan:Phase-2-ASP-NET-Core-Migration

- [ ] (1) Update project SDK to Microsoft.NET.Sdk.Web in ContosoUniversity.csproj per #plan:Step-2.1
- [ ] (2) Add ASP.NET Core framework reference per #plan:Step-2.1
- [ ] (3) Create Program.cs with ASP.NET Core pipeline per #plan:Step-2.1
- [ ] (4) ASP.NET Core infrastructure added (Verify)
- [ ] (5) Extract connection strings from web.config and create appsettings.json per #plan:Step-2.2
- [ ] (6) Configuration files created (Verify)
- [ ] (7) Update DbContext constructors and register in Program.cs per #plan:Step-2.3 and ef-dbcontext-migration skill
- [ ] (8) DbContext registration complete (Verify)
- [ ] (9) Convert all .aspx pages to Razor Pages per #plan:Step-2.4 (Students, Courses, Instructors, Departments, About)
- [ ] (10) All Web Forms pages converted to Razor Pages (Verify)
- [ ] (11) Remove Global.asax and migrate application logic to Program.cs per #plan:Step-2.5
- [ ] (12) Global.asax removed and logic migrated (Verify)
- [ ] (13) Commit changes with message: "TASK-003: Phase 2 - ASP.NET Core migration complete"

### [ ] TASK-004: Fix API compatibility issues
**References**: #plan:Phase-3-API-Compatibility-Fixes

- [ ] (1) Update all System.Data.SqlClient using statements to Microsoft.Data.SqlClient per #plan:Step-3.1 and sqlclient-migration skill
- [ ] (2) All System.Data.SqlClient references removed (Verify)
- [ ] (3) Update connection strings with encryption settings per #plan:Step-3.1
- [ ] (4) Connection strings updated (Verify)
- [ ] (5) Replace ConfigurationManager with IConfiguration using dependency injection per #plan:Step-3.2
- [ ] (6) ConfigurationManager references removed (Verify)
- [ ] (7) Build solution and fix all compilation errors per #plan:Step-3.4 and #plan:Breaking-Changes-Catalog
- [ ] (8) Solution builds with 0 errors (Verify)
- [ ] (9) Commit changes with message: "TASK-004: Phase 3 - API compatibility fixes complete"

### [ ] TASK-005: Validate upgrade and ensure zero build warnings
**References**: #plan:Phase-4-Validation-and-Testing

- [ ] (1) Clean and rebuild solution
- [ ] (2) Solution builds with 0 errors and 0 warnings (Verify)
- [ ] (3) Verify application starts without errors
- [ ] (4) Application starts successfully (Verify)
- [ ] (5) Verify database connection and DbContext initialization
- [ ] (6) Database connectivity works (Verify)
- [ ] (7) Commit changes with message: "TASK-005: Phase 4 - Upgrade validation complete"

---
