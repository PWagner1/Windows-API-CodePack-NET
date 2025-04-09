// Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable InlineOutVariableDeclaration
#pragma warning disable CS8631
namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Manages the sensors conected to the system.
    /// </summary>
    public static class SensorManager
    {
        #region Public Methods
        /// <summary>
        /// Retireves a collection of all sensors.
        /// </summary>
        /// <returns>A list of all sensors.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static SensorList<Sensor?> GetAllSensors() => GetSensorsByCategoryId(SensorCategories.All);

        /// <summary>
        /// Retrieves a collection of sensors filtered by category ID.
        /// </summary>
        /// <param name="category">The category ID of the requested sensors.</param>
        /// <returns>A list of sensors of the specified category ID.</returns>
        public static SensorList<Sensor?> GetSensorsByCategoryId(Guid category)
        {
            ISensorCollection? sensorCollection;
            HResult hr = _sensorManager.GetSensorsByCategory(category, out sensorCollection);
            if (hr == HResult.ElementNotFound)
                throw new SensorPlatformException(LocalizedMessages.SensorsNotFound);

            return NativeSensorCollectionToSensorCollection<Sensor>(sensorCollection);
        }

        /// <summary>
        /// Returns a collection of sensors filtered by type ID.
        /// </summary>
        /// <param name="typeId">The type ID of the sensors requested.</param>
        /// <returns>A list of sensors of the spefified type ID.</returns>
        public static SensorList<Sensor?> GetSensorsByTypeId(Guid typeId)
        {
            ISensorCollection? sensorCollection;
            HResult hr = _sensorManager.GetSensorsByType(typeId, out sensorCollection);
            if (hr == HResult.ElementNotFound)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorsNotFound);
            }
            return NativeSensorCollectionToSensorCollection<Sensor>(sensorCollection);
        }

        /// <summary>
        /// Returns a strongly typed collection of specific sensor types.
        /// </summary>
        /// <typeparam name="T">The type of the sensors to retrieve.</typeparam>
        /// <returns>A strongly types list of sensors.</returns>        
        public static SensorList<T?> GetSensorsByTypeId<T>() where T : Sensor
        {
            object?[] attrs = typeof(T).GetCustomAttributes(typeof(SensorDescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                SensorDescriptionAttribute? sda = attrs[0] as SensorDescriptionAttribute;

                ISensorCollection? nativeSensorCollection = null;
                if (sda != null)
                {
                    HResult hr = _sensorManager.GetSensorsByType(sda.SensorTypeGuid, out nativeSensorCollection);
                    if (hr == HResult.ElementNotFound)
                    {
                        throw new SensorPlatformException(LocalizedMessages.SensorsNotFound);
                    }
                }

                return NativeSensorCollectionToSensorCollection<T>(nativeSensorCollection);
            }

            return [];
        }

        /// <summary>
        /// Returns a specific sensor by sensor ID.
        /// </summary>
        /// <typeparam name="T">A strongly typed sensor.</typeparam>
        /// <param name="sensorId">The unique identifier of the sensor.</param>
        /// <returns>A particular sensor.</returns>        
        public static T? GetSensorBySensorId<T>(Guid sensorId) where T : Sensor
        {
            ISensor? nativeSensor;
            HResult hr = _sensorManager.GetSensorByID(sensorId, out nativeSensor);
            if (hr == HResult.ElementNotFound)
            {
                throw new SensorPlatformException(LocalizedMessages.SensorsNotFound);
            }

            if (nativeSensor != null)
            {
                return GetSensorWrapperInstance<T>(nativeSensor);
            }

            return null;
        }

        /// <summary>
        /// Opens a system dialog box to request user permission to access sensor data.
        /// </summary>
        /// <param name="parentWindowHandle">The handle to a window that can act as a parent to the permissions dialog box.</param>
        /// <param name="modal">Specifies whether the window should be modal.</param>
        /// <param name="sensors">The sensors for which to request permission.</param>
        public static void RequestPermission(IntPtr parentWindowHandle, bool modal, SensorList<Sensor> sensors)
        {
            if (sensors == null || sensors.Count == 0)
            {
                throw new ArgumentException(LocalizedMessages.SensorManagerEmptySensorsCollection, nameof(sensors));
            }

            ISensorCollection sensorCollection = new SensorCollection();

            foreach (Sensor sensor in sensors)
            {
                sensorCollection.Add(sensor.InternalObject);
            }

            _sensorManager.RequestPermissions(parentWindowHandle, sensorCollection, modal);
        }

        #endregion

        #region Public Events
        /// <summary>
        /// Occurs when the system's list of sensors changes.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "The event is raised from a static method, and so providing the instance of the sender is not possible")]
        public static event SensorsChangedEventHandler? SensorsChanged;
        #endregion

        #region implementation
        private static NativeISensorManager _sensorManager = new NativeSensorManager();
        private static nativeSensorManagerEventSink _sensorManagerEventSink = new();

        /// <summary>
        /// Sensor type GUID -> .NET Type + report type
        /// </summary>
        private static Dictionary<Guid, SensorTypeData> _guidToSensorDescr = new();

        /// <summary>
        /// .NET type -> type GUID.
        /// </summary>      
        private static readonly Dictionary<Type, Guid> _sensorTypeToGuid = new();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static SensorManager()
        {
            CoreHelpers.ThrowIfNotWin7OrHigher();

            BuildSensorTypeMap();
            Thread.MemoryBarrier();
            _sensorManager.SetEventSink(_sensorManagerEventSink);
        }

        internal static SensorList<TS?> NativeSensorCollectionToSensorCollection<TS>(ISensorCollection? nativeCollection) where TS : Sensor
        {
            SensorList<TS?> sensors = [];

            if (nativeCollection != null)
            {
                uint sensorCount;
                nativeCollection.GetCount(out sensorCount);

                for (uint i = 0; i < sensorCount; i++)
                {
                    ISensor? iSensor;
                    nativeCollection.GetAt(i, out iSensor);
                    TS? sensor = GetSensorWrapperInstance<TS>(iSensor);
                    if (sensor != null)
                    {
                        sensor.InternalObject = iSensor;
                        sensors.Add(sensor);
                    }
                }
            }

            return sensors;
        }

        /// <summary>
        /// Notification that the list of sensors has changed
        /// </summary>
        internal static void OnSensorsChanged(Guid sensorId, SensorAvailabilityChange change)
        {
            if (SensorsChanged != null)
            {
                SensorsChanged.Invoke(new(sensorId, change));
            }
        }

        /// <summary>
        /// Interrogates assemblies currently loaded into the AppDomain for classes deriving from <see cref="Sensor"/>.
        /// Builds data structures which map those types to sensor type GUIDs. 
        /// </summary>
        private static void BuildSensorTypeMap()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in loadedAssemblies)
            {
                try
                {
                    Type[] exportedTypes = asm.GetExportedTypes();
                    foreach (Type t in exportedTypes)
                    {
                        if (t.IsSubclassOf(typeof(Sensor)) && t.IsPublic && !t.IsAbstract && !t.IsGenericType)
                        {
                            object[] attrs = t.GetCustomAttributes(typeof(SensorDescriptionAttribute), true);
                            if (attrs != null && attrs.Length > 0)
                            {
                                SensorDescriptionAttribute sda = (SensorDescriptionAttribute)attrs[0];
                                SensorTypeData stm = new(t, sda);

                                _guidToSensorDescr.Add(sda.SensorTypeGuid, stm);
                                _sensorTypeToGuid.Add(t, sda.SensorTypeGuid);
                            }
                        }
                    }
                }
                catch (NotSupportedException)
                {
                    // GetExportedTypes can throw this if dynamic assemblies are loaded 
                    // via Reflection.Emit.
                }
                catch (FileNotFoundException)
                {
                    // GetExportedTypes can throw this if a loaded asembly is not in the 
                    // current directory or path.
                }
            }
        }

        /// <summary>
        /// Returns an instance of a sensor wrapper appropritate for the given sensor COM interface.
        /// If no appropriate sensor wrapper type could be found, the object created will be of the base-class type <see cref="Sensor"/>.
        /// </summary>
        /// <param name="nativeISensor">The underlying sensor COM interface.</param>
        /// <returns>A wrapper instance.</returns>
        private static TS? GetSensorWrapperInstance<TS>(ISensor? nativeISensor) where TS : Sensor
        {
            Guid sensorTypeGuid;
            if (nativeISensor != null)
            {
                nativeISensor.GetType(out sensorTypeGuid);

                SensorTypeData stm;
                Type sensorClassType =
                    _guidToSensorDescr.TryGetValue(sensorTypeGuid, out stm) ? stm.SensorType : typeof(UnknownSensor);

                try
                {
                    if (Activator.CreateInstance(sensorClassType) is TS sensor)
                    {
                        sensor.InternalObject = nativeISensor;
                        return sensor;
                    }
                }
                catch (InvalidCastException)
                {
                    return null;
                }
            }

            throw new NullReferenceException();
        }

        #endregion
    }

    #region helper classes

    #endregion
}
