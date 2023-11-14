// Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CS8603
#pragma warning disable CS8600
namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Represents a generic ambient light sensor.
    /// </summary>
    [SensorDescription("97F115C8-599A-4153-8894-D2D12899918A")]
    public class AmbientLightSensor : Sensor
    {
        /// <summary>
        /// Gets an array representing the light response curve.
        /// </summary>
        /// <returns>Array representing the light response curve.</returns>
        public uint[] GetLightResponseCurve()
        {
            return (uint[])GetProperty(SensorPropertyKeys.SensorPropertyLightResponseCurve);
        }

        /// <summary>
        /// Gets the current luminous intensity of the sensor.
        /// </summary>
        public LuminousIntensity CurrentLuminousIntensity => new(DataReport);
    }
}
