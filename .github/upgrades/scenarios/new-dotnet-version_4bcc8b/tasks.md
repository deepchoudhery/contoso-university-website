# Contoso University .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the Contoso University upgrade from .NET Framework 4.8 to .NET 10.0. The upgrade follows a phased approach: SDK conversion, ASP.NET Core migration, framework update, testing, and final cleanup.

**Progress**: 0/6 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites and prepare environment
**References**: #plan:Phase-0-Preparation

- [ ] (1) Verify .NET 10 SDK is installed per #plan:Prerequisites
- [ ] (2) .NET 10 SDK version meets minimum requirements (Verify)
- [ ] (3) Verify current solution builds on .NET Framework 4.8
- [ ] (4) Solution builds without errors (Verify)

### [ ] TASK-002: Convert project to SDK-style format
**References**: #plan:Phase-1-SDK-Conversion

- [ ] (1) Convert ContosoUniversity.csproj to SDK-style using convert_project_to_sdk_style tool per #plan:Phase-1
- [ ] (2) Project converted to SDK-style successfully (Verify)
- [ ] (3) Build converted project
- [ ] (4) Project builds with 0 errors and 0 warnings (Verify)
- [ ] (5) Verify packages.config file removed
- [ ] (6) packages.config removed successfully (Verify)
- [ ] (7) Commit changes with message: "feat: Convert ContosoUniversity to SDK-style project"

### [ ] TASK-003: Migrate to ASP.NET Core architecture
**References**: #plan:Phase-2-ASP-NET-Core-Migration

- [ ] (1) Create Program.cs with ASP.NET Core minimal hosting model per #plan:2.1
- [ ] (2) Migrate DbContext initialization from Global.asax to Program.cs per #plan:2.2
- [ ] (3) Replace Web.config with appsettings.json per #plan:2.3
- [ ] (4) Convert .aspx pages to Razor Pages (.cshtml) per #plan:2.4 and #plan:Detailed-File-Level-Changes
- [ ] (5) Migrate code-behind logic to PageModel classes
- [ ] (6) Replace ViewState with proper state management patterns
- [ ] (7) Update Application Insights configuration per #plan:2.5
- [ ] (8) All Web Forms components migrated to ASP.NET Core (Verify)
- [ ] (9) Commit changes with message: "feat: Migrate from ASP.NET Web Forms to ASP.NET Core"

### [ ] TASK-004: Update target framework to .NET 10.0 and packages
**References**: #plan:Phase-3-Framework-Update

- [ ] (1) Update TargetFramework to net10.0 in project file per #plan:3.1
- [ ] (2) Project targets net10.0 (Verify)
- [ ] (3) Update package references per #plan:3.2 and #plan:Package-Update-Reference (key: EntityFramework 6.5.1, Microsoft.Data.SqlClient, Microsoft.ApplicationInsights.AspNetCore)
- [ ] (4) All package references updated (Verify)
- [ ] (5) Remove incompatible packages (Microsoft.CodeDom.Providers, Microsoft.Net.Compilers, legacy App Insights) per #plan:Packages-to-Remove
- [ ] (6) Incompatible packages removed (Verify)
- [ ] (7) Restore all NuGet dependencies
- [ ] (8) All dependencies restored successfully (Verify)
- [ ] (9) Update System.Data.SqlClient to Microsoft.Data.SqlClient namespaces in all code files per #plan:Breaking-Changes
- [ ] (10) Build solution and fix all compilation errors per #plan:Breaking-Changes-Catalog
- [ ] (11) Solution builds with 0 errors and 0 warnings (Verify)
- [ ] (12) Commit changes with message: "feat: Upgrade to .NET 10.0"

### [ ] TASK-005: Run tests and validate upgrade
**References**: #plan:Phase-4-Testing

- [ ] (1) Run application and verify it starts successfully
- [ ] (2) Application starts with no runtime exceptions (Verify)
- [ ] (3) Test database connectivity and Entity Framework queries
- [ ] (4) Database operations execute successfully (Verify)
- [ ] (5) Test all CRUD operations (Create, Read, Update, Delete students)
- [ ] (6) All CRUD operations functional (Verify)
- [ ] (7) Commit changes with message: "test: Validate upgrade functionality"

### [ ] TASK-006: Final validation and cleanup
**References**: #plan:Phase-5-Final-Validation

- [ ] (1) Remove obsolete files (Global.asax, Web.config, .aspx files) per #plan:Phase-5
- [ ] (2) Obsolete files removed (Verify)
- [ ] (3) Clean build to verify no warnings
- [ ] (4) Solution builds with 0 errors and 0 warnings (Verify)
- [ ] (5) Verify no deprecated API usage
- [ ] (6) No deprecated APIs in use (Verify)
- [ ] (7) Commit changes with message: "chore: Clean up after .NET 10 migration"

---
