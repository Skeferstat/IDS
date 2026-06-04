# Upgrade Options — IdsSampleClient

Assessment: 2 projects (net9.0-windows, netstandard2.1), 770+ LOC impacted, 1 incompatible package, 1 security vulnerability

## Strategy

### Upgrade Strategy
Small modern .NET solution with simple dependency graph (2 projects, 2-tier depth)

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects simultaneously in a single atomic pass — fastest approach for small modern solutions |
| Top-Down | Upgrade application first, temporarily multi-target library — adds overhead without benefit for 2-project solutions |
