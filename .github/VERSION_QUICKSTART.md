# Version Increment Quick Start

## 🚀 Quick Reference

### Current Version
Check `Directory.Build.props`:
```xml
<LibraryVersion>8.0.10</LibraryVersion>
```

### CI Build Version Format
```
8.0.10.{GitHub_Run_Number}
Example: 8.0.10.523
```

---

## 📦 Creating a Release

### Option 1: Tag-Based Release (Recommended)

```bash
# 1. Update version (manual or via workflow)
# 2. Pull latest changes
git pull origin main

# 3. Create and push tag
git tag v8.0.11
git push origin v8.0.11

# ✅ Done! Release workflow publishes automatically
```

### Option 2: Manual Workflow Dispatch

```
1. Go to GitHub → Actions
2. Select "Release" workflow
3. Click "Run workflow"
4. Enter version: 8.0.11
5. Click "Run workflow"
```

---

## 🔢 Incrementing Version

### Via GitHub Actions (Recommended)

```
1. Go to GitHub → Actions
2. Select "Auto Version Increment"
3. Click "Run workflow"
4. Select increment type:
   - patch: 8.0.10 → 8.0.11 (bug fixes)
   - minor: 8.0.10 → 8.1.0 (new features)
   - major: 8.0.10 → 9.0.0 (breaking changes)
5. Click "Run workflow"
```

### Manual Edit

Edit `Directory.Build.props`:
```xml
<LibraryVersion>8.0.11</LibraryVersion>
<PackageVersion>8.0.11</PackageVersion>
```

Edit `Source/Current/Windows API CodePack/Directory.Build.targets`:
```xml
<AssemblyVersion>8.0.11</AssemblyVersion>
<FileVersion>8.0.11</FileVersion>
<PackageVersion>8.0.11</PackageVersion>
```

Commit and push:
```bash
git commit -m "chore: bump version to 8.0.11 [skip-version]"
git push origin main
```

---

## 🎯 Common Scenarios

### Bug Fix Release

```bash
# Fix bug → Increment patch → Release
git commit -m "fix: resolve issue"
# Run workflow: increment patch
git pull && git tag v8.0.11 && git push origin v8.0.11
```

### New Feature Release

```bash
# Add feature → Increment minor → Release
git commit -m "feat: add feature"
# Run workflow: increment minor
git pull && git tag v8.1.0 && git push origin v8.1.0
```

### Breaking Change Release

```bash
# Breaking change → Increment major → Release
git commit -m "feat!: breaking change"
# Run workflow: increment major
git pull && git tag v9.0.0 && git push origin v9.0.0
```

### CI Build Only (No Release)

```bash
# Just push - CI builds with automatic build number
git push origin develop
# Version: 8.0.10.{run_number}
```

---

## 🛑 Skip Version Increment

Add `[skip-version]` to commit message:

```bash
git commit -m "docs: update README [skip-version]"
git commit -m "chore: cleanup code [skip-version]"
```

---

## 📊 Version Number Meanings

| Position | Name | Changes When | Example |
|----------|------|--------------|---------|
| **8**.0.10 | Major | Breaking changes | 8 → 9 |
| 8.**0**.10 | Minor | New features | 0 → 1 |
| 8.0.**10** | Patch | Bug fixes | 10 → 11 |
| 8.0.10.**523** | Build | Every CI run | Auto |

---

## 🔍 Check Current Version

### In Files
- `Directory.Build.props` → Base version
- `Directory.Build.targets` → Assembly version
- Assemblies → Runtime version

### Via Code
```csharp
var version = Assembly.GetExecutingAssembly().GetName().Version;
Console.WriteLine(version); // 8.0.10.523
```

### Via PowerShell
```powershell
# Check version in props file
Select-String -Path "Directory.Build.props" -Pattern "LibraryVersion"

# Check assembly version
[Reflection.Assembly]::LoadFile("path\to\dll").GetName().Version
```

---

## ⚠️ Important Notes

1. **NuGet Packages** use base version only (e.g., `8.0.10`)
2. **Assemblies** include build number (e.g., `8.0.10.523`)
3. **Build numbers** increment automatically with each CI run
4. **Tags** should match version (e.g., `v8.0.11`)
5. **Pre-release** tags: use `-alpha`, `-beta`, `-rc` suffix

---

## 🆘 Troubleshooting

### Wrong Version After Build
→ Check if `/p:AssemblyVersion` is passed to build command

### Version Not Incrementing
→ Ensure `[skip-version]` is not in commit message

### Git Conflicts After Version Bump
→ `git fetch && git rebase origin/main`

### NuGet Package Wrong Version
→ Check `Directory.Build.props` LibraryVersion

---

## 📚 Full Documentation

See `.github/VERSIONING.md` for complete details.

