//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Dialogs
{
    /// <summary>
    /// Abstract base class for all dialog controls
    /// </summary>
    public abstract class DialogControl
    {
        private static int _nextId = DialogsDefaults.MinimumDialogControlId;

        /// <summary>
        /// Creates a new instance of a dialog control
        /// </summary>
        protected DialogControl()
        {
            Id = _nextId;

            // Support wrapping of control IDs in case you create a lot of custom controls
            if (_nextId == Int32.MaxValue) { _nextId = DialogsDefaults.MinimumDialogControlId; }
            else { _nextId++; }
        }

        /// <summary>
        /// Creates a new instance of a dialog control with the specified name.
        /// </summary>
        /// <param name="name">The name for this dialog.</param>
        protected DialogControl(string? name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// The native dialog that is hosting this control. This property is null is
        /// there is not associated dialog
        /// </summary>
        public IDialogControlHost? HostingDialog { get; set; } = null!;

        private string? _name;
        /// <summary>
        /// Gets the name for this control.
        /// </summary>
        /// <value>A <see cref="System.String"/> value.</value>        
        public string? Name
        {
            get => _name;
            set
            { 
                // Names for controls need to be quite stable, 
                // as we are going to maintain a mapping between 
                // the names and the underlying Win32/COM control IDs.
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(LocalizedMessages.DialogControlNameCannotBeEmpty);
                }

                if (!string.IsNullOrEmpty(_name))
                {
                    throw new InvalidOperationException(LocalizedMessages.DialogControlsCannotBeRenamed);
                }

                // Note that we don't notify the hosting dialog of 
                // the change, as the initial set of name is (must be)
                // always legal, and renames are always illegal.
                _name = value;
            }
        }
                
        /// <summary>
        /// Gets the identifier for this control.
        /// </summary>
        /// <value>An <see cref="System.Int32"/> value.</value>
        public int Id { get; private set; }
        
        ///<summary>
        /// Calls the hosting dialog, if it exists, to check whether the 
        /// property can be set in the dialog's current state. 
        /// The host should throw an exception if the change is not supported.
        /// Note that if the dialog isn't set yet, 
        /// there are no restrictions on setting the property.
        /// </summary>
        /// <param name="propName">The name of the property that is changing</param>
        protected void CheckPropertyChangeAllowed(string propName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propName), "Property to change was not specified");

            if (HostingDialog != null)
            {
                // This will throw if the property change is not allowed.
                HostingDialog.IsControlPropertyChangeAllowed(propName, this);
            }
        }

        ///<summary>
        /// Calls the hosting dialog, if it exists, to
        /// to indicate that a property has changed, and that 
        /// the dialog should do whatever is necessary 
        /// to propagate the change to the native control.
        /// Note that if the dialog isn't set yet, 
        /// there are no restrictions on setting the property.
        /// </summary>
        /// <param name="propName">The name of the property that is changing.</param>
        protected void ApplyPropertyChange(string propName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propName), "Property changed was not specified");

            if (HostingDialog != null)
            {
                HostingDialog.ApplyControlPropertyChange(propName, this);
            }
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>A <see cref="System.Boolean"/> value.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is DialogControl control)
                return (Id == control.Id);

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>An <see cref="System.Int32"/> hash code for this control.</returns>
#pragma warning disable CS8602
        public override int GetHashCode() => Name == null ? ToString().GetHashCode() : Name.GetHashCode();
#pragma warning restore CS8602
    }
}