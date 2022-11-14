//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    /// <summary>
    /// Provides a strongly typed collection for file dialog filters.
    /// </summary>
    public class CommonFileDialogFilterCollection : Collection<CommonFileDialogFilter>
    {
        // Make the default constructor internal so users can't instantiate this 
        // collection by themselves.
        internal CommonFileDialogFilterCollection() { }

        internal ShellNativeMethods.FilterSpec[] GetAllFilterSpecs()
        {
            ShellNativeMethods.FilterSpec[] filterSpecs = new ShellNativeMethods.FilterSpec[Count];

            for (int i = 0; i < Count; i++)
            {
                filterSpecs[i] = this[i].GetFilterSpec();
            }

            return filterSpecs;
        }
    }
}
