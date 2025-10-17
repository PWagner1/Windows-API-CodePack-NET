# GitHub Actions Workflows

This repository includes comprehensive GitHub Actions workflows for building, testing, and releasing the Windows API CodePack project.

## Workflows Overview

### 1. CI Build (`ci.yml`)
**Triggers:** Push to main/develop branches, Pull Requests

**Features:**
- Builds all .NET projects (.NET Framework 4.6.2-4.8.1, .NET 8-9)
- Builds C++ DirectX project (x64, x86 platforms)
- Runs tests and uploads results
- Validates NuGet package generation
- Performs security scans
- Supports multiple configurations (Debug/Release) and platforms

**Matrix Strategy:**
- Configuration: Debug, Release
- Platform: Any CPU, x64, x86 (x86 excluded for .NET 8+)

### 2. Release (`release.yml`)
**Triggers:** Git tags (v*), Manual dispatch

**Features:**
- Automated version detection from tags
- Builds and packages all projects
- Publishes NuGet packages to NuGet.org
- Creates GitHub releases with release notes
- Supports prerelease versions
- Comprehensive error handling and notifications

**Manual Release:**
```bash
# Create a tag
git tag v8.0.11
git push origin v8.0.11

# Or use workflow dispatch in GitHub UI
```

### 3. Pull Request Validation (`pr-validation.yml`)
**Triggers:** Pull Requests to main/develop

**Features:**
- Validates all changes build successfully
- Checks for breaking changes
- Performs security vulnerability scans
- Validates NuGet package metadata
- Generates comprehensive PR summary

### 4. Dependency Updates (`dependency-updates.yml`)
**Triggers:** Weekly schedule (Mondays), Manual dispatch

**Features:**
- Checks for outdated packages
- Identifies security vulnerabilities
- Creates GitHub issues for updates needed
- Automated dependency updates (manual trigger)
- Creates PRs for dependency updates

## Setup Instructions

### 1. Required Secrets

Add these secrets to your GitHub repository:

- `NUGET_API_KEY`: Your NuGet API key for publishing packages
- `GITHUB_TOKEN`: Automatically provided by GitHub

### 2. Branch Protection Rules

Recommended branch protection settings:

```yaml
main:
  required_status_checks:
    strict: true
    contexts:
      - "CI Build"
      - "Pull Request Validation"
  enforce_admins: true
  required_pull_request_reviews:
    required_approving_review_count: 1
  restrictions:
    users: []
    teams: []
```

### 3. Workflow Permissions

The workflows require these permissions:
- `actions: write` (for artifacts)
- `contents: read` (for code checkout)
- `issues: write` (for dependency update issues)
- `pull-requests: write` (for PR validation)

## Usage Examples

### Creating a Release

1. **Automatic Release (Recommended):**
   ```bash
   git tag v8.0.11
   git push origin v8.0.11
   ```

2. **Manual Release:**
   - Go to Actions → Release workflow
   - Click "Run workflow"
   - Enter version (e.g., "8.0.11")
   - Choose prerelease if needed

### Updating Dependencies

1. **Check for Updates:**
   - Workflow runs automatically every Monday
   - Creates issues for outdated/vulnerable packages

2. **Manual Update:**
   - Go to Actions → Dependency Updates workflow
   - Click "Run workflow"
   - Choose update type (major/minor/patch/all)
   - Creates PR with updates

### Validating Pull Requests

- Automatically runs on all PRs
- Provides comprehensive validation report
- Checks for breaking changes
- Validates package metadata

## Workflow Customization

### Adding New Projects

To add a new .NET project:

1. Add to solution file
2. Update `SOLUTION_PATH` in workflows if needed
3. Ensure project follows naming conventions

### Modifying Build Matrix

Edit the `strategy.matrix` section in `ci.yml`:

```yaml
strategy:
  matrix:
    configuration: [Debug, Release]
    platform: [Any CPU, x64, x86]
    exclude:
      - configuration: Debug
        platform: x86
```

### Customizing Package Publishing

Modify the `publish-nuget` job in `release.yml`:

```yaml
- name: Publish to NuGet
  run: |
    dotnet nuget push *.nupkg --source ${{ env.NUGET_SOURCE }} --api-key ${{ secrets.NUGET_API_KEY }}
```

## Troubleshooting

### Common Issues

1. **Build Failures:**
   - Check Visual Studio version compatibility
   - Verify .NET SDK versions
   - Review platform-specific build issues

2. **Package Publishing Issues:**
   - Verify NuGet API key permissions
   - Check package version conflicts
   - Ensure package metadata is complete

3. **C++ Build Issues:**
   - Verify Visual Studio Build Tools installation
   - Check Windows SDK version
   - Review platform toolset compatibility

### Debugging Workflows

1. **Enable Debug Logging:**
   ```yaml
   - name: Debug step
     run: |
       echo "Debug information"
       dotnet --info
     env:
       ACTIONS_STEP_DEBUG: true
   ```

2. **Check Artifacts:**
   - Download build artifacts from failed runs
   - Review logs for specific error messages

3. **Local Testing:**
   ```bash
   # Test build locally
   dotnet build "Windows API CodePack/Windows API CodePack.sln" --configuration Release
   
   # Test package generation
   dotnet pack "Windows API CodePack/Windows API CodePack.sln" --configuration Release
   ```

## Contributing

When contributing to workflows:

1. Test changes in a fork first
2. Follow YAML best practices
3. Document any new features
4. Update this README if needed

## Support

For workflow-related issues:
- Check GitHub Actions logs
- Review this documentation
- Open an issue with detailed error information
- Include relevant workflow run URLs
