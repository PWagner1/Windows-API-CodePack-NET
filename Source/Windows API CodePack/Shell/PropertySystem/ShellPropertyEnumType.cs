//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
    /// <summary>
    /// Defines the enumeration values for a property type.
    /// </summary>
    public class ShellPropertyEnumType
    {
        #region Private Properties

        private string? _displayText;
        private PropEnumType? _enumType;
        private object? _minValue;
        private object? _setValue;
        private object? _enumerationValue;

        private IPropertyEnumType NativePropertyEnumType
        {
            set;
            get;
        }

        #endregion

        #region Internal Constructor

        internal ShellPropertyEnumType(IPropertyEnumType nativePropertyEnumType)
        {
            NativePropertyEnumType = nativePropertyEnumType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets display text from an enumeration information structure. 
        /// </summary>
        public string? DisplayText
        {
            get
            {
                if (_displayText == null)
                {
                    NativePropertyEnumType.GetDisplayText(out _displayText);
                }
                return _displayText;
            }
        }

        /// <summary>
        /// Gets an enumeration type from an enumeration information structure. 
        /// </summary>
        public PropEnumType EnumType
        {
            get
            {
                if (!_enumType.HasValue)
                {
                    PropEnumType tempEnumType;
                    NativePropertyEnumType.GetEnumType(out tempEnumType);
                    _enumType = tempEnumType;
                }
                return _enumType.Value;
            }
        }

        /// <summary>
        /// Gets a minimum value from an enumeration information structure. 
        /// </summary>
        public object? RangeMinValue
        {
            get
            {
                if (_minValue == null)
                {
                    using (PropVariant propVar = new PropVariant())
                    {
                        NativePropertyEnumType.GetRangeMinValue(propVar);
                        _minValue = propVar.Value;
                    }
                }
                return _minValue;

            }
        }

        /// <summary>
        /// Gets a set value from an enumeration information structure. 
        /// </summary>
        public object? RangeSetValue
        {
            get
            {
                if (_setValue == null)
                {
                    using (PropVariant propVar = new PropVariant())
                    {
                        NativePropertyEnumType.GetRangeSetValue(propVar);
                        _setValue = propVar.Value;
                    }
                }
                return _setValue;

            }
        }

        /// <summary>
        /// Gets a value from an enumeration information structure. 
        /// </summary>
        public object? RangeValue
        {
            get
            {
                if (_enumerationValue == null)
                {
                    using (PropVariant propVar = new PropVariant())
                    {
                        NativePropertyEnumType.GetValue(propVar);
                        _enumerationValue = propVar.Value;
                    }
                }
                return _enumerationValue;
            }
        }

        #endregion
    }
}
