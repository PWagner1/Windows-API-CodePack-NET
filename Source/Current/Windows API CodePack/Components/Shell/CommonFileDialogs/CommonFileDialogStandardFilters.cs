//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Defines the class of commonly used file filters.
/// </summary>
public static class CommonFileDialogStandardFilters
{
    private static CommonFileDialogFilter? _textFilesFilter;
    /// <summary>
    /// Gets a value that specifies the filter for *.txt files.
    /// </summary>
    public static CommonFileDialogFilter? TextFiles
    {
        get
        {
            if (_textFilesFilter == null)
            {
                _textFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersText, "*.txt");
            }
            return _textFilesFilter;
        }
    }

    private static CommonFileDialogFilter? _pictureFilesFilter;
    /// <summary>
    /// Gets a value that specifies the filter for picture files.
    /// </summary>
    public static CommonFileDialogFilter? PictureFiles
    {
        get
        {
            if (_pictureFilesFilter == null)
            {
                _pictureFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersPicture,
                    "*.bmp, *.jpg, *.jpeg, *.png, *.ico");
            }
            return _pictureFilesFilter;
        }

    }
    private static CommonFileDialogFilter? _officeFilesFilter;
    /// <summary>
    /// Gets a value that specifies the filter for Microsoft Office files.
    /// </summary>
    public static CommonFileDialogFilter? OfficeFiles
    {
        get
        {
            if (_officeFilesFilter == null)
            {
                _officeFilesFilter = new CommonFileDialogFilter(LocalizedMessages.CommonFiltersOffice,
                    "*.doc, *.docx, *.xls, *.xlsx, *.ppt, *.pptx");
            }
            return _officeFilesFilter;
        }
    }
}