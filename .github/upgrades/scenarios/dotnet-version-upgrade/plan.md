# .NET 10 Upgrade Plan

## Overview

**Target**: IdsServer solution (4 projects)
**Scope**: ASP.NET Core web application with supporting libraries upgrading from .NET 8 and .NET Standard 2.1 to .NET 10

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single atomic operation.
**Rationale**: 4 projects, simple 3-tier dependency graph (IdsLibrary → IdsServer.Library/IdsServer.Database → IdsServer), all on modern .NET (net8.0, net10.0, netstandard2.1), straightforward upgrade.

### Project Hierarchy
```
Level 0 (Foundation):
  - IdsLibrary (net10.0) ✅ Already upgraded

Level 1 (Depends on Level 0):
  - IdsServer.Library (netstandard2.1) → net10.0
  - IdsServer.Database (net8.0) → net10.0

Level 2 (Application):
  - IdsServer (net8.0) → net10.0 (ASP.NET Core web app with 8 issues)
```

## Tasks

### 01-prerequisites: Verify SDK and Environment

Verify that .NET 10 SDK is installed and functional. Check for any global.json files that might pin SDK versions and ensure they're compatible with .NET 10. Validate that the development environment is ready for the upgrade.

**Scope**: IdsServer.sln (4 projects)

**Done when**: .NET 10 SDK verified as installed, no global.json conflicts detected, environment ready for upgrade

---

### 02-upgrade-projects: Upgrade All Projects to .NET 10

Update all 4 projects' target frameworks to .NET 10 in a single atomic operation. IdsLibrary is already at net10.0. Update IdsServer.Library from netstandard2.1 to net10.0, IdsServer.Database from net8.0 to net10.0, and IdsServer from net8.0 to net10.0. Update all NuGet package references to versions compatible with .NET 10.

**Scope**: 
- IdsLibrary.csproj (net10.0) - already upgraded, verify only
- IdsServer.Library.csproj (netstandard2.1 → net10.0)
- IdsServer.Database.csproj (net8.0 → net10.0)
- IdsServer.csproj (net8.0 → net10.0) - ASP.NET Core web application

**Assessment context**:
- **IdsServer.csproj**: 8 total issues (2 mandatory, 6 potential)
  - Binary incompatible APIs for .NET 10
  - Behavioral changes in APIs
  - Package updates recommended
  - Target framework needs change
- **Other projects**: 0 issues detected, straightforward TFM update

**Known risks**:
- IdsServer is an ASP.NET Core application with DevExpress dependencies
- Entity Framework Core usage in IdsServer.Database may need package updates
- Cross-project references need verification after TFM changes

**Research starting points**:
- Query assessment for IdsServer.csproj specific issues
- Check DevExpress package compatibility with .NET 10
- Verify Entity Framework Core version compatibility
- Review IdsServer dependencies on IdsLibrary, IdsServer.Library, IdsServer.Database

**Done when**: All 4 projects target .NET 10, all packages restored successfully, solution builds with zero errors, all warnings fixed

---

### 03-validation: Final Validation and Testing

Run full solution build to confirm zero errors and zero warnings. Execute all tests to verify functionality. Verify IdsServer web application can run successfully. Document any deferred recommendations or technical debt items identified during the upgrade.

**Scope**: Full IdsServer.sln validation

**Done when**: Solution builds cleanly with zero errors and zero warnings, all tests pass (if any), IdsServer web app runs successfully, upgrade complete and validated
