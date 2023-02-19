//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
    /// <summary>
    /// Defines the menu controls for the Common File Dialog.
    /// </summary>
    [ContentProperty("Items")]
    public class CommonFileDialogMenu : CommonFileDialogProminentControl
    {
        private readonly Collection<CommonFileDialogMenuItem> items = new();
        /// <summary>
        /// Gets the collection of CommonFileDialogMenuItem objects.
        /// </summary>
        public Collection<CommonFileDialogMenuItem> Items => items;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public CommonFileDialogMenu() : base() { }

        /// <summary>
        /// Creates a new instance of this class with the specified text.
        /// </summary>
        /// <param name="text">The text to display for this control.</param>
        public CommonFileDialogMenu(string? text) : base(text) { }

        /// <summary>
        /// Creates a new instance of this class with the specified name and text.
        /// </summary>
        /// <param name="name">The name of this control.</param>
        /// <param name="text">The text to display for this control.</param>
        public CommonFileDialogMenu(string? name, string? text) : base(name, text) { }

        /// <summary>
        /// Attach the Menu control to the dialog object.
        /// </summary>
        /// <param name="dialog">the target dialog</param>
        internal override void Attach(IFileDialogCustomize? dialog)
        {
            Debug.Assert(dialog != null, "CommonFileDialogMenu.Attach: dialog parameter can not be null");

            // Add the menu control
            if (dialog != null)
            {
                dialog.AddMenu(Id, Text);

                // Add the menu items
                foreach (CommonFileDialogMenuItem item in items)
                    dialog.AddControlItem(Id, item.Id, item.Text);

                // Make prominent as needed
                if (IsProminent)
                    dialog.MakeProminent(Id);
            }

            // Sync unmanaged properties with managed properties
            SyncUnmanagedProperties();
        }
    }
}
