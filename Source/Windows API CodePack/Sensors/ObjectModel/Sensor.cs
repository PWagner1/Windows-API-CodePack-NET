// Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CS8605
#pragma warning disable CS8602
#pragma warning disable CS8600
namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Defines a general wrapper for a sensor.
    /// </summary>
    public class Sensor : ISensorEvents
    {
        /// <summary>
        /// Occurs when the DataReport member changes.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "The event is raised by a static method, so passing in the sender instance is not possible")]
        public event DataReportChangedEventHandler? DataReportChanged;

        /// <summary>
        /// Occurs when the State member changes.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "The event is raised by a static method, so passing in the sender instance is not possible")]
        public event StateChangedEventHandler? StateChanged;


        #region Public properties

        /// <summary>
        /// Gets a value that specifies the most recent data reported by the sensor.
        /// </summary>        
        public SensorReport? DataReport { get; private set; }

        /// <summary>
        /// Gets a value that specifies the GUID for the sensor instance.
        /// </summary>
        public Guid? SensorId
        {
            get
            {
                if (_sensorId == null)
                {
                    Guid id;
                    HResult hr = _nativeISensor.GetID(out id);
                    if (hr == HResult.Ok)
                    {
                        _sensorId = id;
                    }
                }
                return _sensorId;
            }
        }
        private Guid? _sensorId;

        /// <summary>
        /// Gets a value that specifies the GUID for the sensor category.
        /// </summary>
        public Guid? CategoryId
        {
            get
            {
                if (_categoryId == null)
                {
                    Guid id;
                    HResult hr = _nativeISensor.GetCategory(out id);
                    if (hr == HResult.Ok)
                    {
                        _categoryId = id;
                    }
                }

                return _categoryId;
            }
        }
        private Guid? _categoryId;

        /// <summary>
        /// Gets a value that specifies the GUID for the sensor type.
        /// </summary>
        public Guid? TypeId
        {
            get
            {
                if (_typeId == null)
                {
                    Guid id;
                    HResult hr = _nativeISensor.GetType(out id);
                    if (hr == HResult.Ok)
                        _typeId = id;
                }

                return _typeId;
            }
        }
        private Guid? _typeId;

        /// <summary>
        /// Gets a value that specifies the sensor's friendly name.
        /// </summary>
        public string? FriendlyName
        {
            get
            {
                if (_friendlyName == null)
                {
                    string? name;
                    HResult hr = _nativeISensor.GetFriendlyName(out name);
                    if (hr == HResult.Ok)
                        _friendlyName = name;
                }
                return _friendlyName;
            }
        }
        private string? _friendlyName;

        /// <summary>
        /// Gets a value that specifies the sensor's current state.
        /// </summary>
        public SensorState State
        {
            get
            {
                NativeSensorState state;
                _nativeISensor.GetState(out state);
                return (SensorState)state;
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies the report interval.
        /// </summary>
        public uint ReportInterval
        {
            get => (uint)GetProperty(SensorPropertyKeys.SensorPropertyCurrentReportInterval);
            set
            {
                SetProperties(new DataFieldInfo[] { new(SensorPropertyKeys.SensorPropertyCurrentReportInterval, value) });
            }
        }

        /// <summary>
        /// Gets a value that specifies the minimum report interval.
        /// </summary>
        public uint MinimumReportInterval => (uint)GetProperty(SensorPropertyKeys.SensorPropertyMinReportInterval);

        /// <summary>
        /// Gets a value that specifies the manufacturer of the sensor.
        /// </summary>
        public string Manufacturer
        {
            get
            {
                if (_manufacturer == null)
                {
                    _manufacturer = (string)GetProperty(SensorPropertyKeys.SensorPropertyManufacturer);
                }
                return _manufacturer;
            }
        }
        private string? _manufacturer;

        /// <summary>
        /// Gets a value that specifies the sensor's model.
        /// </summary>
        public string? Model
        {
            get
            {
                if (_model == null)
                {
                    _model = (string)GetProperty(SensorPropertyKeys.SensorPropertyModel);
                }
                return _model;
            }
        }
        private string? _model;

        /// <summary>
        /// Gets a value that specifies the sensor's serial number.
        /// </summary>
        public string? SerialNumber
        {
            get
            {
                if (_serialNumber == null)
                {
                    _serialNumber = (string)GetProperty(SensorPropertyKeys.SensorPropertySerialNumber);
                }
                return _serialNumber;
            }
        }
        private string? _serialNumber;

        /// <summary>
        /// Gets a value that specifies the sensor's description.
        /// </summary>
        public string? Description
        {
            get
            {
                if (_description == null)
                {
                    _description = (string)GetProperty(SensorPropertyKeys.SensorPropertyDescription);
                }

                return _description;
            }
        }
        private string? _description;

        /// <summary>
        /// Gets a value that specifies the sensor's connection type.
        /// </summary>
        public SensorConnectionType? ConnectionType
        {
            get
            {
                if (_connectionType == null)
                {
                    _connectionType = (SensorConnectionType)GetProperty(SensorPropertyKeys.SensorPropertyConnectionType);
                }
                return _connectionType;
            }
        }
        private SensorConnectionType? _connectionType;

        /// <summary>
        /// Gets a value that specifies the sensor's device path.
        /// </summary>
        public string? DevicePath
        {
            get
            {
                if (_devicePath == null)
                {
                    _devicePath = (string)GetProperty(SensorPropertyKeys.SensorPropertyDeviceId);
                }

                return _devicePath;
            }
        }
        private string? _devicePath;

        /// <summary>
        /// Gets or sets a value that specifies whether the data should be automatically updated.   
        /// If the value is not set, call TryUpdateDataReport or UpdateDataReport to update the DataReport member.
        /// </summary>        
        public bool AutoUpdateDataReport
        {
            get => IsEventInterestSet(EventInterestTypes.DataUpdated);
            set
            {
                if (value)
                    SetEventInterest(EventInterestTypes.DataUpdated);
                else
                    ClearEventInterest(EventInterestTypes.DataUpdated);
            }
        }

        #endregion

        #region public methods
        /// <summary>
        /// Attempts a synchronous data update from the sensor.
        /// </summary>
        /// <returns><b>true</b> if the request was successful; otherwise <b>false</b>.</returns>
        public bool TryUpdateData()
        {
            HResult hr = InternalUpdateData();
            return (hr == HResult.Ok);
        }

        /// <summary>
        /// Requests a synchronous data update from the sensor. The method throws an exception if the request fails.
        /// </summary>
        public void UpdateData()
        {
            HResult hr = InternalUpdateData();
            if (hr != HResult.Ok)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorsNotFound, Marshal.GetExceptionForHR((int)hr));
            }
        }

        internal HResult InternalUpdateData()
        {

            ISensorDataReport iReport;
            HResult hr = _nativeISensor.GetData(out iReport);
            if (hr == HResult.Ok)
            {
                try
                {
                    DataReport = SensorReport.FromNativeReport(this, iReport);
                    if (DataReportChanged != null)
                    {
                        DataReportChanged.Invoke(this, EventArgs.Empty);
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(iReport);
                }
            }
            return hr;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                LocalizedMessages.SensorGetString,
                SensorId,
                TypeId,
                CategoryId,
                FriendlyName);
        }


        /// <summary>
        /// Retrieves a property value by the property key.
        /// </summary>
        /// <param name="propKey">A property key.</param>
        /// <returns>A property value.</returns>        
        public object? GetProperty(PropertyKey propKey)
        {
            using (PropVariant pv = new())
            {
                HResult hr = _nativeISensor.GetProperty(ref propKey, pv);
                if (hr != HResult.Ok)
                {
                    Exception e = Marshal.GetExceptionForHR((int)hr);
                    if (hr == HResult.ElementNotFound)
                    {
                        throw new ArgumentOutOfRangeException(LocalizedMessages.SensorPropertyNotFound, e);
                    }
                    else
                    {
                        throw e;
                    }
                }
                return pv.Value;
            }
        }

        /// <summary>
        /// Retrieves a property value by the property index.
        /// Assumes the GUID component in the property key takes the sensor's type GUID.
        /// </summary>
        /// <param name="propIndex">A property index.</param>
        /// <returns>A property value.</returns>
        public object? GetProperty(int propIndex)
        {
            PropertyKey propKey = new(SensorPropertyKeys.SensorPropertyCommonGuid, propIndex);
            return GetProperty(propKey);
        }

        /// <summary>
        /// Retrieves the values of multiple properties by property key.
        /// </summary>
        /// <param name="propKeys">An array of properties to retrieve.</param>
        /// <returns>A dictionary that contains the property keys and values.</returns>
        public IDictionary<PropertyKey, object?> GetProperties(PropertyKey[] propKeys)
        {
            if (propKeys == null || propKeys.Length == 0)
            {
                throw new ArgumentException(LocalizedMessages.SensorEmptyProperties, "propKeys");
            }

            IPortableDeviceKeyCollection keyCollection = new PortableDeviceKeyCollection();
            try
            {
                IPortableDeviceValues valuesCollection;

                for (int i = 0; i < propKeys.Length; i++)
                {
                    PropertyKey propKey = propKeys[i];
                    keyCollection.Add(ref propKey);
                }

                Dictionary<PropertyKey, object?> data = new();
                HResult hr = _nativeISensor.GetProperties(keyCollection, out valuesCollection);
                if (CoreErrorHelper.Succeeded(hr) && valuesCollection != null)
                {
                    try
                    {

                        uint count = 0;
                        valuesCollection.GetCount(ref count);

                        for (uint i = 0; i < count; i++)
                        {
                            PropertyKey propKey = new();
                            using (PropVariant propVal = new())
                            {
                                valuesCollection.GetAt(i, ref propKey, propVal);
                                data.Add(propKey, propVal.Value);
                            }
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(valuesCollection);
                        valuesCollection = null;
                    }
                }

                return data;
            }
            finally
            {
                Marshal.ReleaseComObject(keyCollection);
                keyCollection = null;
            }
        }

        /// <summary>
        /// Returns a list of supported properties for the sensor.
        /// </summary>
        /// <returns>A strongly typed list of supported properties.</returns>        
        public IList<PropertyKey> GetSupportedProperties()
        {
            if (_nativeISensor == null)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorNotInitialized);
            }

            List<PropertyKey> list = new();
            IPortableDeviceKeyCollection? collection;
            HResult hr = _nativeISensor.GetSupportedDataFields(out collection);
            if (hr == HResult.Ok)
            {
                try
                {
                    uint elements = 0;
                    collection.GetCount(out elements);
                    if (elements == 0) { return null; }

                    for (uint element = 0; element < elements; element++)
                    {
                        PropertyKey key;
                        hr = collection.GetAt(element, out key);
                        if (hr == HResult.Ok)
                        {
                            list.Add(key);
                        }
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(collection);
                    collection = null;
                }
            }
            return list;
        }


        /// <summary>
        /// Retrieves the values of multiple properties by their index.
        /// Assumes that the GUID component of the property keys is the sensor's type GUID.
        /// </summary>
        /// <param name="propIndexes">The indexes of the properties to retrieve.</param>
        /// <returns>An array that contains the property values.</returns>
        /// <remarks>
        /// The returned array will contain null values for some properties if the values could not be retrieved.
        /// </remarks>        
        public object?[] GetProperties(params int[] propIndexes)
        {
            if (propIndexes == null || propIndexes.Length == 0)
            {
                throw new ArgumentNullException("propIndexes");
            }

            IPortableDeviceKeyCollection keyCollection = new PortableDeviceKeyCollection();
            try
            {
                IPortableDeviceValues valuesCollection;
                Dictionary<PropertyKey, int> propKeyToIdx = new();

                for (int i = 0; i < propIndexes.Length; i++)
                {
                    PropertyKey propKey = new(TypeId.Value, propIndexes[i]);
                    keyCollection.Add(ref propKey);
                    propKeyToIdx.Add(propKey, i);
                }

                object?[] data = new object[propIndexes.Length];
                HResult hr = _nativeISensor.GetProperties(keyCollection, out valuesCollection);
                if (hr == HResult.Ok)
                {
                    try
                    {
                        if (valuesCollection == null) { return data; }

                        uint count = 0;
                        valuesCollection.GetCount(ref count);

                        for (uint i = 0; i < count; i++)
                        {
                            PropertyKey propKey = new();
                            using (PropVariant propVal = new())
                            {
                                valuesCollection.GetAt(i, ref propKey, propVal);

                                int idx = propKeyToIdx[propKey];
                                data[idx] = propVal.Value;
                            }
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(valuesCollection);
                        valuesCollection = null;
                    }
                }
                return data;
            }
            finally
            {
                Marshal.ReleaseComObject(keyCollection);
            }
        }

        /// <summary>
        /// Sets the values of multiple properties.
        /// </summary>
        /// <param name="data">An array that contains the property keys and values.</param>
        /// <returns>A dictionary of the new values for the properties. Actual values may not match the requested values.</returns>                
        public IDictionary<PropertyKey, object?> SetProperties(DataFieldInfo[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException(LocalizedMessages.SensorEmptyData, "data");
            }

            IPortableDeviceValues pdv = new PortableDeviceValues();

            for (int i = 0; i < data.Length; i++)
            {
                PropertyKey propKey = data[i].Key;
                object value = data[i].Value;
                if (value == null)
                {
                    throw new ArgumentException(
                        string.Format(CultureInfo.InvariantCulture,
                            LocalizedMessages.SensorNullValueAtIndex, i),
                        "data");
                }

                try
                {
                    // new PropVariant will throw an ArgumentException if the value can 
                    // not be converted to an appropriate PropVariant.
                    using (PropVariant pv = PropVariant.FromObject(value))
                    {
                        pdv.SetValue(ref propKey, pv);
                    }
                }
                catch (ArgumentException)
                {
                    byte[] buffer;
                    if (value is Guid)
                    {
                        Guid guid = (Guid)value;
                        pdv.SetGuidValue(ref propKey, ref guid);
                    }
                    else if ((buffer = value as byte[]) != null)
                    {
                        pdv.SetBufferValue(ref propKey, buffer, (uint)buffer.Length);
                    }
                    else
                    {
                        pdv.SetIUnknownValue(ref propKey, value);
                    }
                }
            }

            Dictionary<PropertyKey, object?> results = new();
            IPortableDeviceValues pdv2 = null;
            HResult hr = _nativeISensor.SetProperties(pdv, out pdv2);
            if (hr == HResult.Ok)
            {
                try
                {
                    uint count = 0;
                    pdv2.GetCount(ref count);

                    for (uint i = 0; i < count; i++)
                    {
                        PropertyKey propKey = new();
                        using (PropVariant propVal = new())
                        {
                            pdv2.GetAt(i, ref propKey, propVal);
                            results.Add(propKey, propVal.Value);
                        }
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(pdv2);
                    pdv2 = null;
                }
            }

            return results;
        }
        #endregion

        #region overridable methods
        /// <summary>
        /// Initializes the Sensor wrapper after it has been bound to the native ISensor interface
        /// and is ready for subsequent initialization.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        #endregion

        #region ISensorEvents Members

        void ISensorEvents.OnStateChanged(ISensor sensor, NativeSensorState state)
        {
            if (StateChanged != null)
            {
                StateChanged.Invoke(this, EventArgs.Empty);
            }
        }

        void ISensorEvents.OnDataUpdated(ISensor sensor, ISensorDataReport newData)
        {
            DataReport = SensorReport.FromNativeReport(this, newData);
            if (DataReportChanged != null)
            {
                DataReportChanged.Invoke(this, EventArgs.Empty);
            }
        }

        void ISensorEvents.OnEvent(ISensor sensor, Guid eventId, ISensorDataReport newData)
        {
        }

        void ISensorEvents.OnLeave(Guid sensorIdArgs)
        {
            SensorManager.OnSensorsChanged(sensorIdArgs, SensorAvailabilityChange.Removal);
        }

        #endregion

        #region Implementation
        private ISensor? _nativeISensor;
        internal ISensor? InternalObject
        {
            get => _nativeISensor;
            set
            {
                _nativeISensor = value;
                SetEventInterest(EventInterestTypes.StateChanged);
                _nativeISensor.SetEventSink(this);
                Initialize();
            }
        }

        /// <summary>
        /// Informs the sensor driver of interest in a specific type of event.
        /// </summary>
        /// <param name="eventType">The type of event of interest.</param>        
        protected void SetEventInterest(Guid eventType)
        {
            if (_nativeISensor == null)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorNotInitialized);
            }

            Guid[] interestingEvents = GetInterestingEvents();

            if (interestingEvents.Any(g => g == eventType)) { return; }

            int interestCount = interestingEvents.Length;

            Guid[] newEventInterest = new Guid[interestCount + 1];
            interestingEvents.CopyTo(newEventInterest, 0);
            newEventInterest[interestCount] = eventType;

            HResult hr = _nativeISensor.SetEventInterest(newEventInterest, (uint)(interestCount + 1));
            if (hr != HResult.Ok)
            {
                throw Marshal.GetExceptionForHR((int)hr);
            }
        }

        /// <summary>
        ///  Informs the sensor driver to clear a specific type of event.
        /// </summary>
        /// <param name="eventType">The type of event of interest.</param>
        protected void ClearEventInterest(Guid eventType)
        {
            if (_nativeISensor == null)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorNotInitialized);
            }

            if (IsEventInterestSet(eventType))
            {
                Guid[] interestingEvents = GetInterestingEvents();
                int interestCount = interestingEvents.Length;

                Guid[] newEventInterest = new Guid[interestCount - 1];

                int eventIndex = 0;
                foreach (Guid g in interestingEvents)
                {
                    if (g != eventType)
                    {
                        newEventInterest[eventIndex] = g;
                        eventIndex++;
                    }
                }

                _nativeISensor.SetEventInterest(newEventInterest, (uint)(interestCount - 1));
            }

        }

        /// <summary>
        /// Determines whether the sensor driver will file events for a particular type of event.
        /// </summary>
        /// <param name="eventType">The type of event, as a GUID.</param>
        /// <returns><b>true</b> if the sensor will report interest in the specified event.</returns>
        protected bool IsEventInterestSet(Guid eventType)
        {
            if (_nativeISensor == null)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorNotInitialized);
            }

            return GetInterestingEvents()
                .Any(g => g.CompareTo(eventType) == 0);
        }

        private Guid[] GetInterestingEvents()
        {
            IntPtr values;
            uint interestCount;
            _nativeISensor.GetEventInterest(out values, out interestCount);
            Guid[] interestingEvents = new Guid[interestCount];
            for (int index = 0; index < interestCount; index++)
            {
                interestingEvents[index] = (Guid)Marshal.PtrToStructure(values, typeof(Guid));
                values = IncrementIntPtr(values, Marshal.SizeOf(typeof(Guid)));
            }
            return interestingEvents;
        }

        private static IntPtr IncrementIntPtr(IntPtr source, int increment)
        {
            if (IntPtr.Size == 8)
            {
                Int64 p = source.ToInt64();
                p += increment;
                return new(p);
            }
            else if (IntPtr.Size == 4)
            {
                Int32 p = source.ToInt32();
                p += increment;
                return new(p);
            }
            else
            {
                throw new SensorPlatformException(LocalizedMessages.SensorUnexpectedPointerSize);
            }
        }

        #endregion
    }

    #region Helper types

    #endregion

}
