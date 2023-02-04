//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
    /// <summary>
    /// Defines the shell property description information for a property.
    /// </summary>
    public class ShellPropertyDescription : IDisposable
    {
        #region Private Fields

        private IPropertyDescription _nativePropertyDescription;
        private string _canonicalName;
        private PropertyKey _propertyKey;
        private string? _displayName;
        private string? _editInvitation;
        private VarEnum? _varEnumType = null;
        private PropertyDisplayType? _displayType;
        private PropertyAggregationType? _aggregationTypes;
        private uint? _defaultColumWidth;
        private PropertyTypeOptions? _propertyTypeFlags;
        private PropertyViewOptions? _propertyViewFlags;
        private Type _valueType;
        private ReadOnlyCollection<ShellPropertyEnumType> _propertyEnumTypes;
        private PropertyColumnStateOptions? _columnState;
        private PropertyConditionType? _conditionType;
        private PropertyConditionOperation? _conditionOperation;
        private PropertyGroupingRange? _groupingRange;
        private PropertySortDescription? _sortDescription;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the case-sensitive name of a property as it is known to the system, 
        /// regardless of its localized name.
        /// </summary>
        public string CanonicalName
        {
            get
            {
                if (_canonicalName == null)
                {
                    PropertySystemNativeMethods.PSGetNameFromPropertyKey(ref _propertyKey, out _canonicalName);
                }

                return _canonicalName;
            }
        }

        /// <summary>
        /// Gets the property key identifying the underlying property.
        /// </summary>
        public PropertyKey PropertyKey => _propertyKey;

        /// <summary>
        /// Gets the display name of the property as it is shown in any user interface (UI).
        /// </summary>
        public string? DisplayName
        {
            get
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (NativePropertyDescription != null && _displayName == null)
                {
                    IntPtr dispNameptr = IntPtr.Zero;

                    HResult hr = NativePropertyDescription.GetDisplayName(out dispNameptr);

                    if (CoreErrorHelper.Succeeded(hr) && dispNameptr != IntPtr.Zero)
                    {
                        _displayName = Marshal.PtrToStringUni(dispNameptr) ?? string.Empty;

                        // Free the string
                        Marshal.FreeCoTaskMem(dispNameptr);
                    }
                }

                return _displayName;
            }
        }

        /// <summary>
        /// Gets the text used in edit controls hosted in various dialog boxes.
        /// </summary>
        public string? EditInvitation
        {
            get
            {
                if (NativePropertyDescription != null && _editInvitation == null)
                {
                    // EditInvitation can be empty, so ignore the HR value, but don't throw an exception
                    IntPtr ptr = IntPtr.Zero;

                    HResult hr = NativePropertyDescription.GetEditInvitation(out ptr);

                    if (CoreErrorHelper.Succeeded(hr) && ptr != IntPtr.Zero)
                    {
                        _editInvitation = Marshal.PtrToStringUni(ptr) ?? string.Empty;
                        // Free the string
                        Marshal.FreeCoTaskMem(ptr);
                    }
                }

                return _editInvitation;
            }
        }

        /// <summary>
        /// Gets the VarEnum OLE type for this property.
        /// </summary>
        public VarEnum VarEnumType
        {
            get
            {
                if (NativePropertyDescription != null && _varEnumType == null)
                {
                    VarEnum tempType;

                    HResult hr = NativePropertyDescription.GetPropertyType(out tempType);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _varEnumType = tempType;
                    }
                }

                return _varEnumType.HasValue ? _varEnumType.Value : default(VarEnum);
            }
        }

        /// <summary>
        /// Gets the .NET system type for a value of this property, or
        /// null if the value is empty.
        /// </summary>
        public Type ValueType
        {
            get
            {
                if (_valueType == null)
                {
                    _valueType = ShellPropertyFactory.VarEnumToSystemType(VarEnumType);
                }

                return _valueType;
            }
        }

        /// <summary>
        /// Gets the current data type used to display the property.
        /// </summary>
        public PropertyDisplayType DisplayType
        {
            get
            {
                if (NativePropertyDescription != null && _displayType == null)
                {
                    PropertyDisplayType tempDisplayType;

                    HResult hr = NativePropertyDescription.GetDisplayType(out tempDisplayType);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _displayType = tempDisplayType;
                    }
                }

                return _displayType.HasValue ? _displayType.Value : default(PropertyDisplayType);
            }
        }

        /// <summary>
        /// Gets the default user interface (UI) column width for this property.
        /// </summary>
        public uint DefaultColumWidth
        {
            get
            {
                if (NativePropertyDescription != null && !_defaultColumWidth.HasValue)
                {
                    uint tempDefaultColumWidth;

                    HResult hr = NativePropertyDescription.GetDefaultColumnWidth(out tempDefaultColumWidth);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _defaultColumWidth = tempDefaultColumWidth;
                    }
                }

                return _defaultColumWidth.HasValue ? _defaultColumWidth.Value : default(uint);
            }
        }

        /// <summary>
        /// Gets a value that describes how the property values are displayed when 
        /// multiple items are selected in the user interface (UI).
        /// </summary>
        public PropertyAggregationType AggregationTypes
        {
            get
            {
                if (NativePropertyDescription != null && _aggregationTypes == null)
                {
                    PropertyAggregationType tempAggregationTypes;

                    HResult hr = NativePropertyDescription.GetAggregationType(out tempAggregationTypes);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _aggregationTypes = tempAggregationTypes;
                    }
                }

                return _aggregationTypes.HasValue ? _aggregationTypes.Value : default(PropertyAggregationType);
            }
        }

        /// <summary>
        /// Gets a list of the possible values for this property.
        /// </summary>
        public ReadOnlyCollection<ShellPropertyEnumType> PropertyEnumTypes
        {
            get
            {
                if (NativePropertyDescription != null && _propertyEnumTypes == null)
                {
                    List<ShellPropertyEnumType> propEnumTypeList = new();

                    Guid guid = new Guid(ShellIIDGuid.IPropertyEnumTypeList);
                    IPropertyEnumTypeList nativeList;
                    HResult hr = NativePropertyDescription.GetEnumTypeList(ref guid, out nativeList);

                    if (nativeList != null && CoreErrorHelper.Succeeded(hr))
                    {

                        uint count;
                        nativeList.GetCount(out count);
                        guid = new Guid(ShellIIDGuid.IPropertyEnumType);

                        for (uint i = 0; i < count; i++)
                        {
                            IPropertyEnumType nativeEnumType;
                            nativeList.GetAt(i, ref guid, out nativeEnumType);
                            propEnumTypeList.Add(new ShellPropertyEnumType(nativeEnumType));
                        }
                    }

                    _propertyEnumTypes = new ReadOnlyCollection<ShellPropertyEnumType>(propEnumTypeList);
                }

                return _propertyEnumTypes;

            }
        }

        /// <summary>
        /// Gets the column state flag, which describes how the property 
        /// should be treated by interfaces or APIs that use this flag.
        /// </summary>
        public PropertyColumnStateOptions ColumnState
        {
            get
            {
                // If default/first value, try to get it again, otherwise used the cached one.
                if (NativePropertyDescription != null && _columnState == null)
                {
                    PropertyColumnStateOptions state;

                    HResult hr = NativePropertyDescription.GetColumnState(out state);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _columnState = state;
                    }
                }

                return _columnState.HasValue ? _columnState.Value : default(PropertyColumnStateOptions);
            }
        }

        /// <summary>
        /// Gets the condition type to use when displaying the property in 
        /// the query builder user interface (UI). This influences the list 
        /// of predicate conditions (for example, equals, less than, and 
        /// contains) that are shown for this property.
        /// </summary>
        /// <remarks>For more information, see the <c>conditionType</c> attribute 
        /// of the <c>typeInfo</c> element in the property's .propdesc file.</remarks>
        public PropertyConditionType ConditionType
        {
            get
            {
                // If default/first value, try to get it again, otherwise used the cached one.
                if (NativePropertyDescription != null && _conditionType == null)
                {
                    PropertyConditionType tempConditionType;
                    PropertyConditionOperation tempConditionOperation;

                    HResult hr = NativePropertyDescription.GetConditionType(out tempConditionType, out tempConditionOperation);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _conditionOperation = tempConditionOperation;
                        _conditionType = tempConditionType;
                    }
                }

                return _conditionType.HasValue ? _conditionType.Value : default(PropertyConditionType);
            }
        }

        /// <summary>
        /// Gets the default condition operation to use 
        /// when displaying the property in the query builder user 
        /// interface (UI). This influences the list of predicate conditions 
        /// (for example, equals, less than, and contains) that are shown 
        /// for this property.
        /// </summary>
        /// <remarks>For more information, see the <c>conditionType</c> attribute of the 
        /// <c>typeInfo</c> element in the property's .propdesc file.</remarks>
        public PropertyConditionOperation ConditionOperation
        {
            get
            {
                // If default/first value, try to get it again, otherwise used the cached one.
                if (NativePropertyDescription != null && _conditionOperation == null)
                {
                    PropertyConditionType tempConditionType;
                    PropertyConditionOperation tempConditionOperation;

                    HResult hr = NativePropertyDescription.GetConditionType(out tempConditionType, out tempConditionOperation);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _conditionOperation = tempConditionOperation;
                        _conditionType = tempConditionType;
                    }
                }

                return _conditionOperation.HasValue ? _conditionOperation.Value : default(PropertyConditionOperation);
            }
        }

        /// <summary>
        /// Gets the method used when a view is grouped by this property.
        /// </summary>
        /// <remarks>The information retrieved by this method comes from 
        /// the <c>groupingRange</c> attribute of the <c>typeInfo</c> element in the 
        /// property's .propdesc file.</remarks>
        public PropertyGroupingRange GroupingRange
        {
            get
            {
                // If default/first value, try to get it again, otherwise used the cached one.
                if (NativePropertyDescription != null && _groupingRange == null)
                {
                    PropertyGroupingRange tempGroupingRange;

                    HResult hr = NativePropertyDescription.GetGroupingRange(out tempGroupingRange);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _groupingRange = tempGroupingRange;
                    }
                }

                return _groupingRange.HasValue ? _groupingRange.Value : default(PropertyGroupingRange);
            }
        }

        /// <summary>
        /// Gets the current sort description flags for the property, 
        /// which indicate the particular wordings of sort offerings.
        /// </summary>
        /// <remarks>The settings retrieved by this method are set 
        /// through the <c>sortDescription</c> attribute of the <c>labelInfo</c> 
        /// element in the property's .propdesc file.</remarks>
        public PropertySortDescription SortDescription
        {
            get
            {
                // If default/first value, try to get it again, otherwise used the cached one.
                if (NativePropertyDescription != null && _sortDescription == null)
                {
                    PropertySortDescription tempSortDescription;

                    HResult hr = NativePropertyDescription.GetSortDescription(out tempSortDescription);

                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        _sortDescription = tempSortDescription;
                    }
                }

                return _sortDescription.HasValue ? _sortDescription.Value : default(PropertySortDescription);
            }
        }

        /// <summary>
        /// Gets the localized display string that describes the current sort order.
        /// </summary>
        /// <param name="descending">Indicates the sort order should 
        /// reference the string "Z on top"; otherwise, the sort order should reference the string "A on top".</param>
        /// <returns>The sort description for this property.</returns>
        /// <remarks>The string retrieved by this method is determined by flags set in the 
        /// <c>sortDescription</c> attribute of the <c>labelInfo</c> element in the property's .propdesc file.</remarks>
        public string GetSortDescriptionLabel(bool descending)
        {
            IntPtr ptr = IntPtr.Zero;
            string label = string.Empty;

            if (NativePropertyDescription != null)
            {
                HResult hr = NativePropertyDescription.GetSortDescriptionLabel(descending, out ptr);

                if (CoreErrorHelper.Succeeded(hr) && ptr != IntPtr.Zero)
                {
                    label = Marshal.PtrToStringUni(ptr);
                    // Free the string
                    Marshal.FreeCoTaskMem(ptr);
                }
            }

            return label;
        }

        /// <summary>
        /// Gets a set of flags that describe the uses and capabilities of the property.
        /// </summary>
        public PropertyTypeOptions TypeFlags
        {
            get
            {
                if (NativePropertyDescription != null && _propertyTypeFlags == null)
                {
                    PropertyTypeOptions tempFlags;

                    HResult hr = NativePropertyDescription.GetTypeFlags(PropertyTypeOptions.MaskAll, out tempFlags);

                    _propertyTypeFlags = CoreErrorHelper.Succeeded(hr) ? tempFlags : default(PropertyTypeOptions);
                }

                return _propertyTypeFlags.HasValue ? _propertyTypeFlags.Value : default(PropertyTypeOptions);
            }
        }

        /// <summary>
        /// Gets the current set of flags governing the property's view.
        /// </summary>
        public PropertyViewOptions ViewFlags
        {
            get
            {
                if (NativePropertyDescription != null && _propertyViewFlags == null)
                {
                    PropertyViewOptions tempFlags;
                    HResult hr = NativePropertyDescription.GetViewFlags(out tempFlags);

                    _propertyViewFlags = CoreErrorHelper.Succeeded(hr) ? tempFlags : default(PropertyViewOptions);
                }

                return _propertyViewFlags.HasValue ? _propertyViewFlags.Value : default(PropertyViewOptions);
            }
        }

        /// <summary>
        /// Gets a value that determines if the native property description is present on the system.
        /// </summary>
        public bool HasSystemDescription => NativePropertyDescription != null;

        #endregion

        #region Internal Constructor

        internal ShellPropertyDescription(PropertyKey key)
        {
            _propertyKey = key;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Get the native property description COM interface
        /// </summary>
        internal IPropertyDescription NativePropertyDescription
        {
            get
            {
                if (_nativePropertyDescription == null)
                {
                    Guid guid = new Guid(ShellIIDGuid.IPropertyDescription);
                    PropertySystemNativeMethods.PSGetPropertyDescription(ref _propertyKey, ref guid, out _nativePropertyDescription);
                }

                return _nativePropertyDescription;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release the native objects
        /// </summary>
        /// <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_nativePropertyDescription != null)
            {
                Marshal.ReleaseComObject(_nativePropertyDescription);
                _nativePropertyDescription = null;
            }

            if (disposing)
            {
                // and the managed ones
                _canonicalName = null;
                _displayName = null;
                _editInvitation = null;
                _defaultColumWidth = null;
                _valueType = null;
                _propertyEnumTypes = null;
            }
        }

        /// <summary>
        /// Release the native objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release the native objects
        /// </summary>
        ~ShellPropertyDescription()
        {
            Dispose(false);
        }

        #endregion
    }
}
