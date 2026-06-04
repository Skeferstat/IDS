# .NET 10 Upgrade Plan

## Overview

**Target**: IdsSampleClient solution (2 projects)
**Scope**: Small modern .NET solution (~5.2k LOC) upgrading from .NET 9 and .NET Standard 2.1 to .NET 10

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 2 projects, simple 2-tier dependency graph, already on modern .NET (net9.0, netstandard2.1), low complexity upgrade.

## Tasks

### 01-prerequisites: Verify SDK and Environment

Verify that .NET 10 SDK is installed and functional. Check for any global.json files that might pin SDK versions and ensure they're compatible with .NET 10. Validate that the development environment is ready for the upgrade.

**Done when**: .NET 10 SDK verified as installed, no global.json conflicts detected, environment ready for upgrade

---

### 02-upgrade-projects: Upgrade All Projects to .NET 10

Update both projects' target frameworks to .NET 10 in a single atomic operation. Update IdsLibrary.csproj from netstandard2.1 to net10.0, and IdsSampleClient.csproj from net9.0-windows to net10.0-windows. Update all NuGet package references to versions compatible with .NET 10.

**Key concerns:**
- **1 incompatible package**: DevExpress.Win.Design (24.2.5) needs replacement or upgrade path
- **1 security vulnerability**: AutoMapper (13.0.1) must be upgraded to 16.1.1
- **2 recommended package updates**: Microsoft.Extensions.Hosting (9.0.0 → 10.0.8)
- **710+ API binary incompatibilities**: Primarily Windows Forms APIs that need compilation verification
- **60 behavioral API changes**: System.Uri and other APIs with runtime behavior changes

Assessment reports 770+ LOC potentially impacted (14.8% of codebase), mostly due to Windows Forms binary incompatibilities which are typically resolved through recompilation.

**Done when**: Both projects target .NET 10, all packages restored successfully, solution builds with zero errors, all warnings fixed

---

### 03-validation: Final Validation and Testing

Run full solution build to confirm zero errors and zero warnings. Execute all tests to verify functionality. Document any deferred recommendations or technical debt items identified during the upgrade.

**Done when**: Solution builds cleanly, all tests pass, upgrade complete and validated
