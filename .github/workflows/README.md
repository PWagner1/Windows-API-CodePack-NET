# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the Windows API CodePack .NET project.

## Workflows Overview

### 🔨 `build.yml` - Build and Test
**Triggers:** Push to main/master/develop branches, Pull requests, Manual dispatch

This workflow:
- Builds the solution for multiple configurations (Debug/Release) and platforms (Any CPU, x64, x86)
- Supports all target frameworks (.NET Framework 4.6.2-4.8.1, .NET 8.0-9.0)
- Handles both managed C# projects and native C++ DirectX project
- Creates build artifacts for Release builds
- Generates NuGet packages on main/master branch builds
- Provides a placeholder for future test execution

**Matrix Strategy:**
- Configuration: Debug, Release
- Platform: Any CPU, x64, x86

### 🚀 `release.yml` - Release and Publish
**Triggers:** Release published, Manual dispatch

This workflow:
- Automatically builds and publishes NuGet packages when a GitHub release is created
- Supports manual version updates via workflow dispatch
- Publishes to both NuGet.org (if `NUGET_API_KEY` secret is configured) and GitHub Packages
- Uploads packages as GitHub release assets
- Automatically updates release notes with package information

**Manual Dispatch Options:**
- `version`: Specify version to release (e.g., "8.0.10")
- `publish_to_nuget`: Whether to publish to NuGet.org

### ✅ `pr-validation.yml` - PR Validation
**Triggers:** Pull request opened/updated

This workflow:
- Performs fast Debug build validation
- Validates project structure and required files
- Checks version consistency between configuration files
- Runs basic code quality checks
- Identifies large files that might need attention

## Setup Instructions

### Required Secrets

To enable full functionality, configure these repository secrets:

1. **`NUGET_API_KEY`** (Optional but recommended)
   - Your NuGet.org API key for publishing packages
   - Get from: https://www.nuget.org/account/apikeys
   - Scope: Push packages

2. **`GITHUB_TOKEN`** (Automatic)
   - Automatically provided by GitHub Actions
   - Used for GitHub Packages publishing and release management

### Setting up Secrets

1. Go to your repository on GitHub
2. Navigate to Settings → Secrets and variables → Actions
3. Click "New repository secret"
4. Add the secrets mentioned above

### Branch Protection (Recommended)

Consider setting up branch protection rules for `main`/`master`:

1. Go to Settings → Branches
2. Add a branch protection rule
3. Enable:
   - Require a pull request before merging
   - Require status checks to pass before merging
   - Select the "build" and "validate" checks

## Package Publishing

### Automatic Publishing (Recommended)

1. Create a new release on GitHub:
   - Go to Releases → Create a new release
   - Create a new tag (e.g., `v8.0.10`)
   - Add release notes
   - Publish the release

2. The `release.yml` workflow will automatically:
   - Build the solution
   - Generate NuGet packages
   - Publish to configured package feeds
   - Update release notes

### Manual Publishing

You can also trigger releases manually:

1. Go to Actions → Release and Publish
2. Click "Run workflow"
3. Specify the version and publishing options
4. Run the workflow

## Supported Frameworks

The workflows support building for:

- **.NET Framework:** 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1
- **.NET:** 8.0-windows, 9.0-windows

## Generated Packages

The workflows generate these NuGet packages:

- `WindowsAPICodePackCore`
- `WindowsAPICodePackShell`
- `WindowsAPICodePackExtendedLinguisticServices`
- `WindowsAPICodePackSensors`
- `WindowsAPICodePackShellExtensions`

Each package includes both the main package (`.nupkg`) and symbol package (`.snupkg`).

## Troubleshooting

### Build Failures

1. Check the build logs in the Actions tab
2. Verify all required files are present in the repository
3. Ensure Directory.Build.props has correct version format

### Publishing Failures

1. Verify `NUGET_API_KEY` secret is correctly configured
2. Check if package versions already exist on NuGet.org
3. Ensure the API key has sufficient permissions

### DirectX Project Issues

The solution includes a native C++ DirectX project that requires:
- Visual Studio Build Tools with C++ workload
- Windows SDK
- The workflow uses MSBuild with appropriate platform configurations

## Adding Tests

To add test execution to the workflows:

1. Add test projects to your solution
2. Update the `test` job in `build.yml` to discover and run tests
3. Consider adding test result reporting and code coverage

Example test execution:
```yaml
- name: Run tests
  run: |
    dotnet test "Source/Current/Windows API CodePack/Windows API CodePack.sln" `
      --configuration Release `
      --no-build `
      --verbosity normal `
      --logger trx `
      --results-directory TestResults
```

## Workflow Status

You can monitor workflow status via:
- Repository Actions tab
- Status badges (add to main README.md)
- Email notifications (configure in GitHub settings)

## Contributing

When modifying workflows:
1. Test changes in a fork first
2. Use workflow dispatch for testing
3. Keep workflows focused and maintainable
4. Document any new requirements or secrets
