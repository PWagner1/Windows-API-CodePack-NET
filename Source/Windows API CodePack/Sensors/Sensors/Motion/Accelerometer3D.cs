// Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CS8605
namespace Microsoft.WindowsAPICodePack.Sensors
{
    /// <summary>
    /// Represents a 3D accelerometer.
    /// </summary>
    [SensorDescription("C2FB0F5F-E2D2-4C78-BCD0-352A9582819D")]
    public class Accelerometer3D : Sensor
    {
        /// <summary>
        /// Gets the current acceleration in G's. 
        /// </summary>
        public Acceleration3D CurrentAcceleration => new(DataReport);
    }
}
