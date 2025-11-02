//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Provides default values and localized messages for dialog components,  including captions, instructions, content,
/// and control settings.
/// </summary>
/// <remarks>This class contains constants and properties that define default values  for dialog-related elements,
/// such as progress bar limits, ideal dimensions,  and control ID ranges. It also provides access to localized strings
/// for  common dialog text.</remarks>
public static class DialogsDefaults
{
    /// <summary>
    /// Gets the default caption text for dialogs.
    /// </summary>
    public static string Caption => LocalizedMessages.DialogDefaultCaption;

    /// <summary>
    /// Gets the default main instruction text for a dialog.
    /// </summary>
    public static string MainInstruction => LocalizedMessages.DialogDefaultMainInstruction;

    /// <summary>
    /// Gets the default content message for the dialog.
    /// </summary>
    public static string Content => LocalizedMessages.DialogDefaultContent;

    /// <summary>
    /// Represents the starting value of a progress bar.
    /// </summary>
    /// <remarks>This constant is typically used to initialize the progress bar to its default starting
    /// state.</remarks>
    public const int ProgressBarStartingValue = 0;

    /// <summary>
    /// Represents the minimum value for a progress bar.
    /// </summary>
    /// <remarks>This constant defines the lowest possible value that a progress bar can represent. It is
    /// typically used to initialize or validate progress bar values.</remarks>
    public const int ProgressBarMinimumValue = 0;

    /// <summary>
    /// Represents the maximum value that a progress bar can reach.
    /// </summary>
    /// <remarks>This constant is typically used to define the upper limit of a progress bar's
    /// range.</remarks>
    public const int ProgressBarMaximumValue = 100;

    /// <summary>
    /// Represents the ideal width for a specific layout or component.
    /// </summary>
    /// <remarks>This constant is used as a default or reference value for width-related
    /// calculations.</remarks>
    public const int IdealWidth = 0;

    // For generating control ID numbers that won't 
    // collide with the standard button return IDs.
    /// <summary>
    /// Represents the minimum value for dialog control IDs to ensure they do not conflict with standard button return
    /// IDs.
    /// </summary>
    /// <remarks>This constant is calculated as one greater than the value of the  <see
    /// cref="TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close"/>  enumeration member. It is intended to be
    /// used as a starting point for  custom control IDs in task dialogs.</remarks>
    public const int MinimumDialogControlId =
        (int)TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close + 1;
}