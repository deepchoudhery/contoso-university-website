# ContosoUniversity .NET 10.0 Upgrade Tasks

## Overview

This document tracks the migration of ContosoUniversity from .NET Framework 4.8 (ASP.NET Web Forms) to .NET 10.0 (ASP.NET Core Razor Pages). This is a complete platform transformation requiring rewrite of the UI layer, migration to Entity Framework Core, and modernization of all application patterns.

**Progress**: 0/7 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [ ] TASK-001: Verify prerequisites and prepare environment
**References**: #plan:Prerequisites

- [ ] (1) Verify .NET 10.0 SDK is installed per #plan:Prerequisites
- [ ] (2) SDK version meets minimum requirements (Verify)
- [ ] (3) Verify SQL Server LocalDB or full instance is available
- [ ] (4) Database connection is accessible (Verify)

### [ ] TASK-002: Create new ASP.NET Core Razor Pages project structure
**References**: #plan:Phase-1

- [ ] (1) Create new ASP.NET Core Razor Pages project targeting net10.0 per #plan:1.1
- [ ] (2) New project structure created successfully (Verify)
- [ ] (3) Add required NuGet packages per #plan:1.2 (EF Core 10, Microsoft.Data.SqlClient 5.2, ApplicationInsights)
- [ ] (4) All packages added successfully (Verify)
- [ ] (5) Restore NuGet dependencies
- [ ] (6) All dependencies restored successfully (Verify)

### [ ] TASK-003: Migrate data layer to Entity Framework Core
**References**: #plan:Phase-2

- [ ] (1) Extract entity classes from EDMX model per #plan:2.1
- [ ] (2) All entity POCOs created (Verify)
- [ ] (3) Create DbContext with dependency injection constructor per #plan:2.2
- [ ] (4) DbContext created successfully (Verify)
- [ ] (5) Extract and migrate database initialization logic from Global.asax per #plan:2.3
- [ ] (6) DbInitializer class created (Verify)
- [ ] (7) Configure DbContext in Program.cs per #plan:2.4
- [ ] (8) DbContext registration complete (Verify)
- [ ] (9) Add connection string to appsettings.json per #plan:2.5
- [ ] (10) Connection string configured (Verify)

### [ ] TASK-004: Migrate UI layer from Web Forms to Razor Pages
**References**: #plan:Phase-3

- [ ] (1) Create Razor Pages for each Web Form per #plan:3.1 mapping (Students, Courses, Instructors, About)
- [ ] (2) All Razor Pages created (Verify)
- [ ] (3) Extract business logic from code-behind files to PageModel classes per #plan:3.2
- [ ] (4) All PageModel classes implemented (Verify)
- [ ] (5) Migrate static assets (CSS, JavaScript, images) to wwwroot per #plan:3.4
- [ ] (6) All static assets migrated (Verify)

### [ ] TASK-005: Migrate configuration and Application Insights
**References**: #plan:Phase-4

- [ ] (1) Configure Application Insights in Program.cs per #plan:4.1
- [ ] (2) Application Insights configured (Verify)
- [ ] (3) Migrate configuration settings from Web.config to appsettings.json
- [ ] (4) All configuration migrated (Verify)

### [ ] TASK-006: Build and resolve all compilation errors
**References**: #plan:Phase-5

- [ ] (1) Build new ASP.NET Core project
- [ ] (2) Fix all compilation errors per #plan:Breaking-Changes-Catalog (SqlClient namespaces, EF Core APIs, configuration access)
- [ ] (3) Rebuild project after fixes
- [ ] (4) Project builds with 0 errors and 0 warnings (Verify)

### [ ] TASK-007: Test and validate complete migration
**References**: #plan:Phase-6

- [ ] (1) Run application and verify database initialization per #plan:6.1
- [ ] (2) Database created and seeded successfully (Verify)
- [ ] (3) Test all CRUD operations per #plan:6.2 (Students, Courses, Instructors)
- [ ] (4) All CRUD operations functional (Verify)
- [ ] (5) Commit all changes with message: "Complete migration to .NET 10.0 - ContosoUniversity Web Forms to ASP.NET Core Razor Pages"

---
