// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 


[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices", MessageId = "API")]


[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingAvailableServices.#TransliterationDevanagariToLatin", MessageId = "Devanagari")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingAvailableServices.#TransliterationHantToHans", MessageId = "Hant")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingAvailableServices.#TransliterationHansToHant", MessageId = "Hant")]

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]
[assembly: SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification = "There are places where unsigned values are used, which is considered not Cls compliant.")]

#region LinkDemand related
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.InteropTools.#Free`1(System.IntPtr&)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.InteropTools.#Pack`1(!!0&)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.InteropTools.#Unpack`1(System.IntPtr)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.InteropTools.#UnpackStringArray(System.IntPtr,System.UInt32)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.LinguisticException.#.ctor()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.LinguisticException.#.ctor(System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.LinguisticException.#.ctor(System.String,System.Exception)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.LinguisticException.#.ctor(System.UInt32)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingDataRange.#GetData()")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingPropertyBag.#Dispose(System.Boolean)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingPropertyBag.#.ctor(Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingOptions,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingService.#GetServices(Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingEnumOptions)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingService.#.ctor(System.Guid)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingService.#RecognizeText(System.String,System.Int32,System.Int32,Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingOptions)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.ServiceCache.#RollBack(System.IntPtr,System.Int32)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.ServiceCache.#TryRegisterServices(System.IntPtr,System.IntPtr[],System.Boolean&)")]

#endregion

[assembly: SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.ServiceCache")]
[assembly: SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingService.#EndDoAction(Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingActionAsyncResult)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingService.#EndRecognizeText(Microsoft.WindowsAPICodePack.ExtendedLinguisticServices.MappingRecognizeAsyncResult)")]
