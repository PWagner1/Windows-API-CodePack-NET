# Automatic Version Increment System

This document describes the automatic version increment system implemented for the Windows API CodePack .NET project.

## Overview

The project uses a **hybrid versioning system** that combines:
- **Semantic Versioning** (Major.Minor.Patch) for releases
- **Build Numbers** (GitHub run number) for CI builds
- **Git SHA** for detailed tracking

## Version Format

### Release Versions
```
Major.Minor.Patch
Example: 8.0.10
```

### CI Build Versions
```
Major.Minor.Patch.BuildNumber
Example: 8.0.10.523
```

### Informational Version (Full Traceability)
```
Major.Minor.Patch.BuildNumber+GitSHA
Example: 8.0.10.523+abc123def456
```

## How It Works

### 1. CI Builds (Automatic)

Every push to `main` or `develop` triggers the CI workflow:

```yaml
Workflow: ci.yml
Trigger: Push to main/develop, Pull Requests
```

**Process:**
1. Reads base version from `Directory.Build.props` (e.g., `8.0.10`)
2. Appends GitHub run number as build number (e.g., `523`)
3. Creates full version: `8.0.10.523`
4. Sets assembly properties during build:
   - `AssemblyVersion`: `8.0.10.523`
   - `FileVersion`: `8.0.10.523`
   - `InformationalVersion`: `8.0.10.523+abc123`

### 2. Version Increment (Manual or Automatic)

The `version-increment.yml` workflow manages version bumps:

```yaml
Workflow: version-increment.yml
Trigger: Push to main/develop, Manual dispatch
Skip: Add [skip-version] to commit message
```

**Increment Types:**

| Type | Example | When to Use |
|------|---------|-------------|
| **major** | 8.0.10 → 9.0.0 | Breaking changes |
| **minor** | 8.0.10 → 8.1.0 | New features |
| **patch** | 8.0.10 → 8.0.11 | Bug fixes (default) |
| **build** | 8.0.10 → 8.0.10 (build++) | No version change |

**Manual Trigger:**
```bash
# Via GitHub Actions UI
Actions → Auto Version Increment → Run workflow
Select increment type → Run
```

**Automatic Trigger:**
```bash
# Default behavior on push to main
git push origin main  # Auto-increments patch version
```

**Skip Version Increment:**
```bash
git commit -m "docs: update README [skip-version]"
git push origin main
```

### 3. Releases (Tag-Based)

Create releases using Git tags:

```bash
# Create and push a version tag
git tag v8.0.11
git push origin v8.0.11

# Or use GitHub UI: Releases → Create a new release
```

**Process:**
1. Tag triggers `release.yml` workflow
2. Extracts version from tag (e.g., `v8.0.11` → `8.0.11`)
3. Adds build number: `8.0.11.524`
4. Builds with full version
5. Publishes to NuGet with base version: `8.0.11`

## Version Files

### Root Level

**`Directory.Build.props`**
```xml
<Project>
  <PropertyGroup>
    <LibraryVersion>8.0.10</LibraryVersion>
    <PackageVersion>8.0.10</PackageVersion>
  </PropertyGroup>
</Project>
```
- Base semantic version
- Used for NuGet package versions
- Manually updated or auto-incremented

### Solution Level

**`Source/Current/Windows API CodePack/Directory.Build.targets`**
```xml
<Project>
  <PropertyGroup>
    <AssemblyVersion>8.0.10</AssemblyVersion>
    <FileVersion>8.0.10</FileVersion>
    <PackageVersion>8.0.10</PackageVersion>
  </PropertyGroup>
</Project>
```
- Assembly and file versions
- Can include build number during CI
- Overridden by MSBuild properties in workflows

## Version Properties Explained

| Property | Purpose | Example | Visible To |
|----------|---------|---------|------------|
| **LibraryVersion** | Base version for NuGet | 8.0.10 | Package metadata |
| **PackageVersion** | NuGet package version | 8.0.10 | NuGet.org |
| **AssemblyVersion** | Strong name version | 8.0.10.523 | .NET runtime |
| **FileVersion** | File version | 8.0.10.523 | Windows Explorer |
| **InformationalVersion** | Display version | 8.0.10.523+abc123 | About dialogs |

## Usage Scenarios

### Scenario 1: Bug Fix Release

```bash
# 1. Fix the bug and commit
git commit -m "fix: resolve null reference exception"

# 2. Manually increment version
# Go to Actions → Auto Version Increment → Run workflow
# Select: patch
# This updates: 8.0.10 → 8.0.11

# 3. Create release tag
git pull  # Get version update
git tag v8.0.11
git push origin v8.0.11

# 4. Release workflow automatically publishes
```

### Scenario 2: New Feature Release

```bash
# 1. Develop feature and commit
git commit -m "feat: add new shell integration"

# 2. Manually increment version
# Go to Actions → Auto Version Increment → Run workflow
# Select: minor
# This updates: 8.0.10 → 8.1.0

# 3. Create release tag
git pull
git tag v8.1.0
git push origin v8.1.0
```

### Scenario 3: Breaking Change Release

```bash
# 1. Implement breaking changes and commit
git commit -m "feat!: redesign API interface"

# 2. Manually increment version
# Go to Actions → Auto Version Increment → Run workflow
# Select: major
# This updates: 8.0.10 → 9.0.0

# 3. Create release tag
git pull
git tag v9.0.0
git push origin v9.0.0
```

### Scenario 4: CI Build Only (No Release)

```bash
# Just push to main or develop
git push origin develop

# CI runs with: 8.0.10.{run_number}
# No version files changed
# No release created
```

## Workflow Integration

### CI Workflow (`ci.yml`)

```yaml
- name: Set build version
  id: set-version
  run: |
    $baseVersion = "8.0.10"  # From Directory.Build.props
    $buildNumber = ${{ github.run_number }}
    $fullVersion = "$baseVersion.$buildNumber"
    echo "full-version=$fullVersion" >> $env:GITHUB_OUTPUT

- name: Build .NET projects
  run: |
    dotnet build `
      /p:AssemblyVersion=${{ steps.set-version.outputs.full-version }} `
      /p:FileVersion=${{ steps.set-version.outputs.full-version }}
```

### Release Workflow (`release.yml`)

```yaml
- name: Determine version
  id: version
  run: |
    $version = "8.0.11"  # From tag
    $buildNumber = ${{ github.run_number }}
    $versionWithBuild = "$version.$buildNumber"

- name: Build solution
  run: |
    dotnet build `
      /p:AssemblyVersion=$versionWithBuild `
      /p:FileVersion=$versionWithBuild
```

## Best Practices

### 1. Commit Message Conventions

Use conventional commits to indicate version changes:

```bash
# Patch version bump
git commit -m "fix: resolve memory leak"

# Minor version bump
git commit -m "feat: add new functionality"

# Major version bump  
git commit -m "feat!: breaking API change"

# No version bump
git commit -m "docs: update README [skip-version]"
git commit -m "chore: update dependencies [skip-version]"
```

### 2. Version Increment Timing

- **Patch**: After bug fixes, before release
- **Minor**: After new features, before release
- **Major**: After breaking changes, before release
- **Build**: Use for CI tracking only

### 3. Release Process

1. Merge all changes to `main`
2. Run version increment workflow (if not automatic)
3. Pull latest changes
4. Create and push tag
5. Release workflow handles the rest

### 4. Pre-release Versions

For alpha/beta releases:

```bash
# Create pre-release tag
git tag v8.1.0-beta.1
git push origin v8.1.0-beta.1

# Release workflow detects and marks as pre-release
```

## Troubleshooting

### Version Not Incrementing

**Problem**: Version files not updating after workflow run

**Solution**:
1. Check workflow logs for errors
2. Ensure `[skip-version]` is not in commit message
3. Verify GitHub token permissions

### Build Number Not Applied

**Problem**: Assemblies show base version without build number

**Solution**:
1. Check CI workflow passed the version correctly
2. Verify MSBuild properties in build step
3. Inspect assembly properties after build

### NuGet Package Wrong Version

**Problem**: Package version doesn't match expected version

**Solution**:
1. Package version uses base version only (no build number)
2. Check `Directory.Build.props` for correct base version
3. Verify pack command doesn't override version

### Conflicts After Version Increment

**Problem**: Git conflicts when pulling after auto-increment

**Solution**:
```bash
# Fetch and rebase instead of pull
git fetch origin
git rebase origin/main

# Or discard local changes
git reset --hard origin/main
```

## Version Query

### Check Version in Code

```csharp
using System.Reflection;

// Get assembly version
var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
Console.WriteLine($"Assembly: {assemblyVersion}");

// Get file version
var fileVersion = FileVersionInfo.GetVersionInfo(
    Assembly.GetExecutingAssembly().Location).FileVersion;
Console.WriteLine($"File: {fileVersion}");

// Get informational version
var infoVersion = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
Console.WriteLine($"Informational: {infoVersion}");
```

### Check Version via PowerShell

```powershell
# Get version from assembly
[System.Reflection.Assembly]::LoadFile("path\to\dll").GetName().Version

# Get version from file properties
(Get-Item "path\to\dll").VersionInfo.FileVersion
```

## Future Enhancements

Potential improvements:

1. **GitVersion Integration**: Use GitVersion for automatic versioning based on Git history
2. **Changelog Generation**: Auto-generate CHANGELOG.md from commit messages
3. **Version Validation**: Ensure versions follow semantic versioning rules
4. **Dependency Version Sync**: Automatically update inter-package dependencies

## References

- [Semantic Versioning 2.0.0](https://semver.org/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [GitHub Actions Run Number](https://docs.github.com/en/actions/learn-github-actions/contexts#github-context)
- [MSBuild Version Properties](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props)

