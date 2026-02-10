# GitHub Actions Workflows

This repository includes comprehensive GitHub Actions workflows for building, testing, and releasing the Windows API CodePack project.

## Workflows Overview

### 1. CI Build (`ci.yml`)
**Triggers:** Push to main/develop branches, Pull Requests

**Features:**
- Builds all .NET projects (.NET Framework 4.6.2-4.8.1, .NET 8-10)
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
- **Authenticode signing** (if certificate secrets are configured)
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

#### Optional: Authenticode Signing Secrets

To enable Authenticode signing of DLLs during release builds, add these optional secrets:

- `CODESIGN_CERTIFICATE_BASE64`: Base64-encoded PFX certificate file (required for signing)
- `CODESIGN_CERTIFICATE_PASSWORD`: Password for the PFX certificate (if password-protected)

**How to create a code signing certificate:**

##### Option 1: Self-Signed Certificate (For Testing/Development)

Self-signed certificates are useful for testing but will show a warning to end users. They are **not trusted** by default on other machines.

**Create a self-signed certificate using PowerShell:**

```powershell
# Create a self-signed code signing certificate
$cert = New-SelfSignedCertificate `
    -Type CodeSigningCert `
    -Subject "CN=Your Company Name Code Signing" `
    -KeyUsage DigitalSignature `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -CertStoreLocation Cert:\CurrentUser\My `
    -NotAfter (Get-Date).AddYears(3)

Write-Host "Certificate created with thumbprint: $($cert.Thumbprint)"
Write-Host "Certificate location: $($cert.PSPath)"
```

**Export to PFX format:**

```powershell
# Export the certificate to PFX file
$password = Read-Host -AsSecureString -Prompt "Enter password for PFX file"
$cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Where-Object { $_.Thumbprint -eq "YOUR_THUMBPRINT_HERE" }
Export-PfxCertificate -Cert $cert -FilePath codesign.pfx -Password $password
```

##### Option 2: Trusted Certificate from Certificate Authority (For Production)

For production use, you need a certificate from a trusted Certificate Authority (CA). These certificates are trusted by Windows and won't show warnings to users.

**Popular Code Signing Certificate Providers:**

1. **DigiCert** - https://www.digicert.com/code-signing/
2. **Sectigo (formerly Comodo)** - https://sectigo.com/ssl-certificates-tls/code-signing
3. **GlobalSign** - https://www.globalsign.com/en/code-signing-certificate
4. **SSL.com** - https://www.ssl.com/certificates/code-signing/

**Purchase Process:**

1. Purchase a code signing certificate from a CA
2. Complete identity verification (required for code signing certificates)
3. Download the certificate or receive installation instructions
4. Install the certificate in your certificate store

**Export purchased certificate to PFX:**

```powershell
# List code signing certificates in your store
Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Format-Table Subject, Thumbprint, NotAfter

# Export specific certificate to PFX
$thumbprint = "YOUR_CERTIFICATE_THUMBPRINT"
$cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Where-Object { $_.Thumbprint -eq $thumbprint }
$password = Read-Host -AsSecureString -Prompt "Enter password for PFX file"
Export-PfxCertificate -Cert $cert -FilePath codesign.pfx -Password $password
```

**Note:** Some CAs provide the certificate directly as a PFX file. In that case, you can skip the export step.

##### Preparing Certificate for GitHub Actions

Once you have a PFX file, prepare it for GitHub Actions:

1. **Convert PFX to Base64:**
   ```powershell
   $certBytes = [System.IO.File]::ReadAllBytes("codesign.pfx")
   $certBase64 = [Convert]::ToBase64String($certBytes)
   $certBase64 | Out-File -FilePath "cert-base64.txt" -Encoding ASCII
   
   # Display the base64 (copy this for GitHub secret)
   Write-Host "Base64 certificate:"
   Write-Host $certBase64
   ```

2. **Add to GitHub Secrets:**
   - Go to Repository Settings → Secrets and variables → Actions
   - Click "New repository secret"
   - Name: `CODESIGN_CERTIFICATE_BASE64`
   - Value: Paste the base64 content from `cert-base64.txt`
   - Click "Add secret"
   - If your PFX has a password, add another secret:
     - Name: `CODESIGN_CERTIFICATE_PASSWORD`
     - Value: Your PFX password

**Quick Start - Use the Helper Script:**

We provide a PowerShell script to automate certificate creation:

```powershell
# Run the helper script (in the .github/workflows directory)
.\create-codesign-certificate.ps1

# Or with custom parameters:
.\create-codesign-certificate.ps1 -Subject "CN=My Company Code Signing" -ValidYears 3
```

The script will:
- Create a self-signed code signing certificate
- Export it to PFX format
- Convert to Base64 for GitHub Actions
- Display instructions for adding to GitHub Secrets

**Manual Certificate Creation:**

If you prefer to create the certificate manually:

```powershell
# Step 1: Create or get certificate
# For self-signed (testing):
$cert = New-SelfSignedCertificate `
    -Type CodeSigningCert `
    -Subject "CN=MyApp Code Signing" `
    -KeyUsage DigitalSignature `
    -CertStoreLocation Cert:\CurrentUser\My `
    -NotAfter (Get-Date).AddYears(1)

# Or get existing certificate:
# $cert = Get-ChildItem -Path Cert:\CurrentUser\My -CodeSigningCert | Select-Object -First 1

# Step 2: Export to PFX
$pfxPassword = "YourSecurePassword123!"
$pfxPath = "codesign.pfx"
Export-PfxCertificate `
    -Cert $cert `
    -FilePath $pfxPath `
    -Password (ConvertTo-SecureString -String $pfxPassword -AsPlainText -Force)

# Step 3: Convert to Base64
$certBytes = [System.IO.File]::ReadAllBytes($pfxPath)
$certBase64 = [Convert]::ToBase64String($certBytes)

# Step 4: Save base64 to file
$certBase64 | Out-File -FilePath "cert-base64.txt" -Encoding ASCII

Write-Host "Certificate exported to: $pfxPath"
Write-Host "Base64 saved to: cert-base64.txt"
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Copy the content of cert-base64.txt"
Write-Host "2. Add it as GitHub secret: CODESIGN_CERTIFICATE_BASE64"
Write-Host "3. Add password as GitHub secret: CODESIGN_CERTIFICATE_PASSWORD = $pfxPassword"
```

**Security Best Practices:**

- ✅ Store the PFX password securely (use GitHub Secrets)
- ✅ Never commit certificates or passwords to the repository
- ✅ Use strong passwords for PFX files
- ✅ Keep your private key secure
- ✅ For production, use certificates from trusted CAs
- ✅ Regularly renew certificates before expiration

**Note:** If these secrets are not provided, the build will complete successfully without Authenticode signing. The signing is optional and only applies to Release builds.

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
