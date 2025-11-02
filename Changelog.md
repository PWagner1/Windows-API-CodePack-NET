02/11/2025

Version 8.0.12
* Added support for .NET 10
* Fixed README inclusion in all NuGet packages to comply with NuGet.org requirements
* Updated package descriptions to reflect .NET 8-10 support
* Fixed [#30](https://github.com/PWagner1/Windows-API-CodePack-NET/issues/30), ShellThumbnail COM object marshaling issue - ShellThumbnail now maintains a reference to parent ShellObject to prevent premature COM object release

=========

18/10/2025

Version 8.0.11
* Added unified `WindowsAPICodePack` NuGet package containing all components (Core, Shell, Sensors, ExtendedLinguisticServices, ShellExtensions)
* Implemented automatic build number incrementation using GitHub Actions
* Enhanced CI/CD workflows with version management
* Added comprehensive workflow documentation

=========

19/09/2025

Version 8.0.10
* General improvements and bug fixes

=========

15/08/2025

Version 8.0.9

- Upgrade `sln` file to `slnx` format
- More `null` checking
- Removed support for .NET 6
- Implemented [#14](https://github.com/PWagner1/Windows-API-CodePack-NET/issues/14), How to perform a search with `ExplorerBrowser`

=========

24/12/2024

Version 8.0.8
- Implemented [#21](https://github.com/PWagner1/Windows-API-CodePack-NET/issues/21), Enhance the `OnViewEnumerationComplete` Event of the `ExplorerBrowser` Control 

=========

27/10/2024

Version 8.0.7
- Implemented [#20](https://github.com/PWagner1/Windows-API-CodePack-NET/issues/20), Add `System.RecordedTV.EpisodeNumber`
- More `null` checking

=========

22/10/2024

Version 8.0.6
- Resolved [#18](https://github.com/PWagner1/Windows-API-CodePack-NET/issues/18), `CommonFileDialog.FileName` property changes extension back to default
- New `.editorconfig` configuration

=========

01/07/2024

Version 8.0.5
- Resolved [#16](https://github.com/Wagnerp/Windows-API-CodePack-NET/issues/16), Useless System.Net.Http
- Fix NuGet descriptions

=========

10/06/2024

Version 8.0.4
- Removed support for .NET Core App 3.1
- Removed support for .NET 5
- Removed support for .NET 7
- Some more nullables
- Add samples

=========

01/03/2024

Version 8.0.3
- Fix the implementation of the property `CheckSelect`

=========

24/02/2024

Version 8.0.2
- Fix for multi-dotted extensions
- Remove the border on the `ExplorerBrowser` control 
- New `SelectionChangedEventArgs` for selected folder and file names
- Ensure that `Shell` is thread safe

=========

14/02/2024

Version 8.0.1
- Update year 2023 -> 2024
- Add `Readme` to NuGet packages
- Add preliminary support for .NET 9

=========

14/11/2023

Version 8.0.0
- Add .NET 8 support
- More `nullable` checking

=========

26/02/2023

Version 7.0.4
- Support for `nullable`
- Usage of `new()`
- New symbol packages

=========

17/02/2023

Version 7.0.3
- Add strong naming

=========

13/11/2022

Version 7.0.0
- Updated to use SDK style projects
- Use .NET Framework 4.6.2 - 4.8.1, .NET Core 3.1 &  .NET 5 - 7
- Updated NuGet icon
- Use C# 10

=========

02/01/2016
 
Core 1.1.2
- TaskDialog icons were visible only when defined in Opened event
- TaskDialog custom/hyperlink button not closing dialog from within Click event
