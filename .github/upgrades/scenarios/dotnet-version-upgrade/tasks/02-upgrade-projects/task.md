# 02-upgrade-projects: Upgrade All Projects to .NET 10

Update both projects' target frameworks to .NET 10 in a single atomic operation. Update IdsLibrary.csproj from netstandard2.1 to net10.0, and IdsSampleClient.csproj from net9.0-windows to net10.0-windows. Update all NuGet package references to versions compatible with .NET 10.

## Research Findings

### Projects in Scope

**1. IdsLibrary.csproj** (Foundation Library)
- Current TFM: `netstandard2.1`
- Target TFM: `net10.0`
- Path: `X:\workspaces\dtcrssmd\IDS\Library\IdsLibrary\IdsLibrary.csproj`
- SDK-style: ✅ Yes
- Dependencies: None (leaf library)
- API Issues: 41 behavioral changes (System.Uri - 1%, low impact)
- Package References:
  - Ardalis.SmartEnum 8.2.0 (✅ compatible)
  - FluentValidation 11.11.0 (✅ compatible)
- Impact: 41+ LOC (1% of 3971 LOC) — minimal changes expected

**2. IdsSampleClient.csproj** (WPF Application)
- Current TFM: `net9.0-windows`
- Target TFM: `net10.0-windows`
- Path: `X:\workspaces\dtcrssmd\IDS\Client\IdsSampleClient\IdsSampleClient.csproj`
- SDK-style: ✅ Yes
- Project Kind: WPF
- Dependencies: IdsLibrary.csproj
- API Issues: 710 binary incompatible (Windows Forms APIs), 19 behavioral changes
- Technologies: Windows Forms (97.3% of issues), 1 legacy control
- Impact: 729+ LOC (59.3% of 1230 LOC) — primarily recompilation fixes
- Package References (10 total):
  - AutoMapper 13.0.1 → **16.1.1** (🔴 security vulnerability CVE)
  - DevExpress.Win.Design 24.2.5 (⚠️ incompatible — needs investigation)
  - Microsoft.Extensions.Hosting 9.0.0 → **10.0.8** (recommended)
  - Microsoft.Web.WebView2 1.0.2903.40 (✅ compatible)
  - Serilog 4.2.0 (✅ compatible)
  - Serilog.Extensions.Hosting 9.0.0 (✅ compatible)
  - Serilog.Settings.Configuration 9.0.0 (✅ compatible)
  - Serilog.Sinks.Console 6.0.0 (✅ compatible)
  - Serilog.Sinks.Debug 3.0.0 (✅ compatible)
  - Serilog.Sinks.File 6.0.0 (✅ compatible)

### Package Actions Required

1. **Security Fix (Critical)**: AutoMapper 13.0.1 → 16.1.1
2. **DevExpress Investigation**: Check if DevExpress.Win.Design 24.2.5 has .NET 10 compatible version
3. **Recommended Update**: Microsoft.Extensions.Hosting 9.0.0 → 10.0.8

### API Compatibility Notes

**Windows Forms Binary Incompatibilities (710 instances)**:
- Primarily designer-generated code in `.Designer.cs` files
- Common types: GroupBox (79), TextBox (53), Button (49), Label (48), Control collections (34)
- **Expected resolution**: Recompilation will resolve most binary incompatibilities automatically
- No code changes anticipated — these are reference metadata differences between .NET 9 and .NET 10

**Behavioral Changes (60 total)**:
- System.Uri (51 instances, 6.6%) — constructor behavior changes
- System.Xml.Serialization.XmlSerializer (4 instances) — serialization behavior changes
- **Action**: Recompilation + runtime testing required

**Legacy Control (1 instance)**:
- One usage of legacy Windows Forms control (StatusBar, DataGrid, ContextMenu, MainMenu, MenuItem, or ToolBar)
- **Action**: Identify and replace with modern equivalent (ToolStrip, MenuStrip, ContextMenuStrip, DataGridView)

### Execution Plan

1. **Update IdsLibrary.csproj TFM**: `netstandard2.1` → `net10.0`
2. **Update IdsSampleClient.csproj TFM**: `net9.0-windows` → `net10.0-windows`
3. **Update packages**:
   - Check DevExpress.Win.Design compatibility for .NET 10
   - Update AutoMapper to 16.1.1 (security fix)
   - Update Microsoft.Extensions.Hosting to 10.0.8
4. **Restore dependencies**: `dotnet restore`
5. **Build solution**: `dotnet build` — resolve any compilation errors
6. **Fix warnings**: Ensure zero warnings in both projects
7. **Identify and replace legacy Windows Forms control**

**Key concerns:**
- **1 incompatible package**: DevExpress.Win.Design (24.2.5) needs replacement or upgrade path
- **1 security vulnerability**: AutoMapper (13.0.1) must be upgraded to 16.1.1
- **2 recommended package updates**: Microsoft.Extensions.Hosting (9.0.0 → 10.0.8)
- **710+ API binary incompatibilities**: Primarily Windows Forms APIs that need compilation verification
- **60 behavioral API changes**: System.Uri and other APIs with runtime behavior changes

Assessment reports 770+ LOC potentially impacted (14.8% of codebase), mostly due to Windows Forms binary incompatibilities which are typically resolved through recompilation.

**Done when**: Both projects target .NET 10, all packages restored successfully, solution builds with zero errors, all warnings fixed
