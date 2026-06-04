# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 10.0 (LTS)

## Source Control
- **Source Branch**: master
- **Working Branch**: dotnet-version-upgrade
- **Commit Strategy**: Single Commit at End

## Upgrade Options
**Source**: .github/upgrades/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

## Strategy
**Selected**: All-At-Once
**Rationale**: 2 projects, simple 2-tier dependency graph, all on modern .NET (net9.0, netstandard2.1), straightforward upgrade.

### Execution Constraints
- Single atomic upgrade — all projects updated together in one pass
- Validate full solution build after upgrade (zero errors, zero warnings required)
- No tier ordering — both projects upgraded simultaneously
- Package updates and code fixes handled in the same upgrade task
