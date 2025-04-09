namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport,
     Guid(ShellIIDGuid.IConditionFactory),
     CoClass(typeof(ConditionFactoryCoClass))]
    internal interface INativeConditionFactory : IConditionFactory
    {
    }
}