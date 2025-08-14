//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    public static class DialogsDefaults
    {
        public static string Caption => LocalizedMessages.DialogDefaultCaption;
        public static string MainInstruction => LocalizedMessages.DialogDefaultMainInstruction;
        public static string Content => LocalizedMessages.DialogDefaultContent;

        public const int ProgressBarStartingValue = 0;
        public const int ProgressBarMinimumValue = 0;
        public const int ProgressBarMaximumValue = 100;

        public const int IdealWidth = 0;

        // For generating control ID numbers that won't 
        // collide with the standard button return IDs.
        public const int MinimumDialogControlId =
            (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close + 1;
    }
}
