# Microsoft Windows API Codepack

[![CI Build](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/ci.yml/badge.svg)](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/ci.yml)
[![Release](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/release.yml/badge.svg)](https://github.com/Wagnerp/Windows-API-CodePack-NET/actions/workflows/release.yml)
[![NuGet Version](https://img.shields.io/nuget/v/WindowsAPICodePackCore.svg)](https://www.nuget.org/packages/WindowsAPICodePackCore/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/WindowsAPICodePackCore.svg)](https://www.nuget.org/packages/WindowsAPICodePackCore/)

=========================

License
-------

The library is not developed anymore by Microsoft and seems to have been left as 'free to use'. A clarification or update about the licence terms from Microsoft is welcome, however.
 
Release notes
-------------

This release has the latest bug fixes applied, including the fix for 64-bit exceptions.

Bugs
----

When you submit a bug:

 - provide a short example code showing the bug
 - describe the expected behavior/result
 - if a pull request is applicable, please reference it

Pull Requests
-------------

I'll be glad to accept pull requests if they fix a bug or add a worthwhile feature.


Usage notes
-----------

**TaskDialog**

If you get the following exception when you instantiate a `TaskDialog`:

```
An unhandled exception of type 'System.NotSupportedException' occurred in Microsoft.WindowsAPICodePack.dll

Additional information: TaskDialog feature needs to load version 6 of comctl32.dll but a different version is current loaded in memory.
```

To fix it, create an application manifest and un-comment the following block section:

```
  <!-- Enable themes for Windows common controls and dialogs (Windows XP and later) -->
  <!-- <dependency>
    <dependentAssembly>
      <assemblyIdentity
          type="win32"
          name="Microsoft.Windows.Common-Controls"
          version="6.0.0.0"
          processorArchitecture="*"
          publicKeyToken="6595b64144ccf1df"
          language="*"
        />
    </dependentAssembly>
  </dependency>-->
```

Note: you might have to restart Visual Studio as the DLLs seems to be cached in memory and rebuilding your project doesn't seem to be enough in some cases.

**DirectX**

The DirectX package will work under x86 and x64 configuration platforms but not for AnyCPU platform (because there is no such platform for C++/CLI projects). Consequently, the package will purposefully fail the build and tell you why it did.

Note: package is here for historical reasons, it is highly recommended to use [SharpDX](http://sharpdx.org/) instead.

**Authenticode Signing**

The project supports Authenticode signing of DLLs to provide code integrity verification. To enable Authenticode signing during build:

1. **Using a Certificate File (.pfx):**
   ```xml
   <PropertyGroup>
     <EnableAuthenticodeSigning>true</EnableAuthenticodeSigning>
     <CodeSigningCertificatePath>path\to\your\certificate.pfx</CodeSigningCertificatePath>
     <CodeSigningCertificatePassword>your-password</CodeSigningCertificatePassword> <!-- Optional -->
   </PropertyGroup>
   ```

2. **Using a Certificate from Certificate Store:**
   ```xml
   <PropertyGroup>
     <EnableAuthenticodeSigning>true</EnableAuthenticodeSigning>
     <CodeSigningCertificateThumbprint>your-certificate-thumbprint</CodeSigningCertificateThumbprint>
   </PropertyGroup>
   ```

3. **Via MSBuild Command Line:**
   ```bash
   dotnet build /p:EnableAuthenticodeSigning=true /p:CodeSigningCertificatePath="path\to\certificate.pfx" /p:CodeSigningCertificatePassword="password"
   ```

**Requirements:**
- Windows SDK must be installed (SignTool.exe is required)
- A valid code signing certificate (either .pfx file or installed in certificate store)
- The certificate must be valid for code signing

**Note:** Authenticode signing is disabled by default. You must explicitly enable it by setting `EnableAuthenticodeSigning=true`. The build will continue even if signing fails (with a warning), so you can build without a certificate for development purposes.

**GitHub Actions Workflow Support:**

Authenticode signing is also supported in GitHub Actions workflows. To enable signing in release builds:

1. Add your code signing certificate as a GitHub secret:
   - `CODESIGN_CERTIFICATE_BASE64`: Base64-encoded PFX certificate file
   - `CODESIGN_CERTIFICATE_PASSWORD`: Certificate password (if required)

2. The release workflow will automatically sign all DLLs when these secrets are present.

See `.github/workflows/README.md` for detailed setup instructions.