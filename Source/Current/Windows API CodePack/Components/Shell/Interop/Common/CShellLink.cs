//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell;
// Disable warning if a method declaration hides another inherited from a parent COM interface
// To successfully import a COM interface, all inherited methods need to be declared again with 
// the exception of those already declared in "IUnknown"
#pragma warning disable 108

#region COM Interfaces

[ComImport,
 Guid(ShellIIDGuid.CShellLink),
 ClassInterface(ClassInterfaceType.None)]
internal class CShellLink { }

// Summary:
//     Provides the managed definition of the IPersistStream interface, with functionality
//     from IPersist.

#endregion

#pragma warning restore 108