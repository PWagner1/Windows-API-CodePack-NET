// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.


[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.Interop.NativeColorRef.#set_Dword(System.UInt32)")]

[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#Microsoft.WindowsAPICodePack.ShellExtensions.Interop.IPreviewHandler.DoPreview()")]

[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.WpfPreviewHandler.#Initialize()")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider.#.ctor()")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.WpfPreviewHandler.#.ctor()")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.Interop", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ShellExtensions", MessageId = "API")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#SetBackground(System.Int32)", MessageId = "argb")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#SetForeground(System.Int32)", MessageId = "argb")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.IThumbnailFromStream")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.IPreviewFromStream")]

[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")]

[assembly: SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant")]

[assembly: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider.#GetThumbnailAlphaType()")]

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ShellExtensions")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.Interop")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.IPreviewFromStream")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.IThumbnailFromStream")]

[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandlers.PreviewHandler.#Microsoft.WindowsAPICodePack.ShellExtensions.Interop.IPreviewHandler.DoPreview()", Justification = "Exception is handled later")]
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#Microsoft.WindowsAPICodePack.ShellExtensions.Interop.IPreviewHandler.DoPreview()", Justification = "Exception is handled later")]

// All link demand related warnings suppressed.
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider.#Microsoft.WindowsAPICodePack.ShellExtensions.Interop.IThumbnailProvider.GetThumbnail(System.UInt32,System.IntPtr&,System.UInt32&)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.WpfPreviewHandler.#Initialize()")]

[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Justification = "Interfaces are implemented explicitly because they are native COM interfaces, we do not want child types to call them.", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Justification = "Interfaces are implemented explicitly because they are native COM interfaces, we do not want child types to call them.", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler.#System.Runtime.InteropServices.ICustomQueryInterface.GetInterface(System.Guid&,System.IntPtr&)")]

[assembly: SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification = "There are places where unsigned values are used, which is considered not Cls compliant.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Justification = "Populated via marshaling.", Target = "Microsoft.WindowsAPICodePack.ShellExtensions.Interop.NativeColorRef.#Dword")]
