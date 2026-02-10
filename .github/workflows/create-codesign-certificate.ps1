# Code Signing Certificate Creation Script
# This script helps you create a self-signed code signing certificate for testing
# For production use, purchase a certificate from a trusted CA (DigiCert, Sectigo, etc.)

param(
    [string]$Subject = "CN=Windows API CodePack Code Signing",
    [int]$ValidYears = 3,
    [string]$OutputPath = "codesign.pfx",
    [string]$Password = ""
)

Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "     Code Signing Certificate Creation Script" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# Check if running as administrator (not required, but recommended)
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "⚠️  Warning: Not running as administrator. Certificate will be created in CurrentUser store." -ForegroundColor Yellow
    Write-Host ""
}

# Prompt for password if not provided
if ([string]::IsNullOrEmpty($Password)) {
    $securePassword = Read-Host -AsSecureString -Prompt "Enter password for PFX file"
    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
    $Password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
}

Write-Host "Creating self-signed code signing certificate..." -ForegroundColor Green
Write-Host "  Subject: $Subject" -ForegroundColor Gray
Write-Host "  Valid for: $ValidYears years" -ForegroundColor Gray
Write-Host ""

try {
    # Create the certificate
    $cert = New-SelfSignedCertificate `
        -Type CodeSigningCert `
        -Subject $Subject `
        -KeyUsage DigitalSignature `
        -KeyAlgorithm RSA `
        -KeyLength 2048 `
        -HashAlgorithm SHA256 `
        -CertStoreLocation Cert:\CurrentUser\My `
        -NotAfter (Get-Date).AddYears($ValidYears)

    Write-Host "✅ Certificate created successfully!" -ForegroundColor Green
    Write-Host "  Thumbprint: $($cert.Thumbprint)" -ForegroundColor Gray
    Write-Host "  Valid until: $($cert.NotAfter)" -ForegroundColor Gray
    Write-Host ""

    # Export to PFX
    Write-Host "Exporting certificate to PFX format..." -ForegroundColor Green
    $securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force
    Export-PfxCertificate `
        -Cert $cert `
        -FilePath $OutputPath `
        -Password $securePassword `
        -ErrorAction Stop

    Write-Host "✅ Certificate exported to: $OutputPath" -ForegroundColor Green
    Write-Host ""

    # Convert to Base64
    Write-Host "Converting to Base64 for GitHub Actions..." -ForegroundColor Green
    $certBytes = [System.IO.File]::ReadAllBytes($OutputPath)
    $certBase64 = [Convert]::ToBase64String($certBytes)
    
    $base64Path = "cert-base64.txt"
    $certBase64 | Out-File -FilePath $base64Path -Encoding ASCII -NoNewline
    Write-Host "✅ Base64 saved to: $base64Path" -ForegroundColor Green
    Write-Host ""

    # Display summary
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host "                    NEXT STEPS" -ForegroundColor Cyan
    Write-Host "═══════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. Copy the content of: $base64Path" -ForegroundColor Yellow
    Write-Host "2. Go to GitHub Repository → Settings → Secrets and variables → Actions" -ForegroundColor Yellow
    Write-Host "3. Add new secret:" -ForegroundColor Yellow
    Write-Host "   Name:  CODESIGN_CERTIFICATE_BASE64" -ForegroundColor White
    Write-Host "   Value: [paste content from $base64Path]" -ForegroundColor White
    Write-Host "4. Add password secret (if required):" -ForegroundColor Yellow
    Write-Host "   Name:  CODESIGN_CERTIFICATE_PASSWORD" -ForegroundColor White
    Write-Host "   Value: $Password" -ForegroundColor White
    Write-Host ""
    Write-Host "⚠️  IMPORTANT SECURITY NOTES:" -ForegroundColor Red
    Write-Host "   • This is a SELF-SIGNED certificate for testing only" -ForegroundColor Yellow
    Write-Host "   • End users will see a warning when running signed code" -ForegroundColor Yellow
    Write-Host "   • For production, purchase a certificate from a trusted CA" -ForegroundColor Yellow
    Write-Host "   • Never commit certificates or passwords to the repository" -ForegroundColor Yellow
    Write-Host "   • Keep your PFX file and password secure" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "📚 For production certificates, see:" -ForegroundColor Cyan
    Write-Host "   • DigiCert: https://www.digicert.com/code-signing/" -ForegroundColor Gray
    Write-Host "   • Sectigo: https://sectigo.com/ssl-certificates-tls/code-signing" -ForegroundColor Gray
    Write-Host "   • GlobalSign: https://www.globalsign.com/en/code-signing-certificate" -ForegroundColor Gray
    Write-Host ""

} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  • Ensure you have permissions to create certificates" -ForegroundColor Gray
    Write-Host "  • Check that the output path is writable" -ForegroundColor Gray
    Write-Host "  • Verify PowerShell execution policy allows script execution" -ForegroundColor Gray
    exit 1
}
