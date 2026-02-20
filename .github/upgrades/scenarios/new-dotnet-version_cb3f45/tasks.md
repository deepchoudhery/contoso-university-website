# ContosoUniversity .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the ContosoUniversity upgrade from .NET Framework 4.8 to .NET 10.0. The single-project solution will be upgraded using an all-at-once approach, converting from ASP.NET Web Forms to ASP.NET Core Razor Pages.

**Progress**: 7/7 tasks complete (100%) ![100%](https://progress-bar.xyz/100)

---

## Tasks

### [x] TASK-001: Verify prerequisites
**References**: #plan:Prerequisites

- [x] (1) Verify .NET 10 SDK is installed on the machine
- [x] (2) .NET 10 SDK installation verified (Verify)

### [x] TASK-002: Convert project to SDK-style and update target framework
**References**: #plan:Step-1-Convert-SDK-Style, #plan:Step-2-Update-TargetFramework, #plan:Package-Update-Reference

- [x] (1) Convert ContosoUniversity.csproj to SDK-style format using convert_project_to_sdk_style tool
- [x] (2) Project converted to SDK-style format (Verify)
- [x] (3) Update target framework from net48 to net10.0 in the project file
- [x] (4) Remove incompatible NuGet packages per #plan:Package-Update-Reference (ApplicationInsights packages, CodeDom, Net.Compilers)
- [x] (5) Add Microsoft.Data.SqlClient package reference (5.2.2)
- [x] (6) Add System.Configuration.ConfigurationManager package reference for interim compatibility (9.0.0)
- [x] (7) All package references updated (Verify)
- [x] (8) Remove packages.config file after SDK-style conversion
- [x] (9) packages.config removed (Verify)

### [x] TASK-003: Create ASP.NET Core application structure
**References**: #plan:Step-4-Create-ASP.NET-Core-Structure

- [x] (1) Create Program.cs with ASP.NET Core host builder, Razor Pages, and static files middleware
- [x] (2) Create appsettings.json with connection strings migrated from Web.config
- [x] (3) Create Pages directory structure and Shared/_Layout.cshtml from Site.Master
- [x] (4) ASP.NET Core application structure created (Verify)

### [x] TASK-004: Migrate Web Forms pages to Razor Pages
**References**: #plan:Step-5-Migrate-Web-Forms

- [x] (1) Convert Home.aspx and code-behind to Pages/Index.cshtml and Pages/Index.cshtml.cs
- [x] (2) Convert About.aspx and code-behind to Pages/About.cshtml and Pages/About.cshtml.cs
- [x] (3) Convert Students.aspx and code-behind to Pages/Students.cshtml and Pages/Students.cshtml.cs
- [x] (4) Convert Courses.aspx and code-behind to Pages/Courses.cshtml and Pages/Courses.cshtml.cs
- [x] (5) Convert Instructors.aspx and code-behind to Pages/Instructors.cshtml and Pages/Instructors.cshtml.cs
- [x] (6) All Web Forms pages converted to Razor Pages (Verify)

### [x] TASK-005: Migrate Entity Framework and database access
**References**: #plan:Step-6-Migrate-EF, #plan:Step-7-Migrate-SqlClient, #plan:Step-8-Update-Configuration

- [x] (1) Update DbContext constructor to accept connection string per ef-dbcontext-migration skill
- [x] (2) Register DbContext in Program.cs using ASP.NET Core DI
- [x] (3) Migrate System.Data.SqlClient usages to Microsoft.Data.SqlClient in BLL files per sqlclient-migration skill
- [x] (4) Update configuration access from System.Configuration.ConfigurationManager to IConfiguration where applicable
- [x] (5) All database access code migrated (Verify)

### [x] TASK-006: Build and fix all compilation errors
**References**: #plan:Step-10-Build-Verification, #plan:Breaking-Changes-Catalog

- [x] (1) Build the project and identify all compilation errors
- [x] (2) Fix all compilation errors related to System.Web incompatibilities and other breaking changes per #plan:Breaking-Changes-Catalog
- [x] (3) Rebuild the project to verify all errors are resolved
- [x] (4) Project builds with 0 errors (Verify)
- [x] (5) Project builds with 0 warnings (Verify)

### [x] TASK-007: Final commit
**References**: #plan:Success-Criteria

- [x] (1) Commit all changes with message: "Complete upgrade to .NET 10.0"

---

