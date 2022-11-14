// Copyright (c) Microsoft Corporation.  All rights reserved.

using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Represents all the data from a single sensor data report.
    /// </summary>
    public class SensorReport
    {
        /// <summary>
        /// Gets the time when the data report was generated.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public DateTime TimeStamp => _timeStamp;

        /// <summary>
        /// Gets the data values in the report.
        /// </summary>
        public SensorData? Values => _sensorData;

        /// <summary>
        /// Gets the sensor that is the source of this data report.
        /// </summary>
        public Sensor? Source => _originator;

        #region implementation
        private SensorData? _sensorData;
        private Sensor? _originator;
        private DateTime _timeStamp = new();

        internal static SensorReport FromNativeReport(Sensor? originator, ISensorDataReport iReport)
        {

            SystemTime systemTimeStamp = new SystemTime();
            iReport.GetTimestamp(out systemTimeStamp);
            FILETIME ftTimeStamp = new FILETIME();
            SensorNativeMethods.SystemTimeToFileTime(ref systemTimeStamp, out ftTimeStamp);
            long lTimeStamp = (((long)ftTimeStamp.dwHighDateTime) << 32) + (long)ftTimeStamp.dwLowDateTime;
            DateTime timeStamp = DateTime.FromFileTime(lTimeStamp);

            SensorReport sensorReport = new SensorReport();
            sensorReport._originator = originator;
            sensorReport._timeStamp = timeStamp;
            if (originator != null)
                sensorReport._sensorData = SensorData.FromNativeReport(originator.InternalObject, iReport);

            return sensorReport;
        }
        #endregion

    }
}
