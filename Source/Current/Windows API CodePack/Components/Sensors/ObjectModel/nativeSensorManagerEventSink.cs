namespace Microsoft.WindowsAPICodePack.Sensors;

internal class nativeSensorManagerEventSink : ISensorManagerEvents
{
    #region nativeISensorManagerEvents Members

    public void OnSensorEnter(ISensor nativeSensor, NativeSensorState state)
    {
        if (state == NativeSensorState.Ready)
        {
            Guid sensorId;
            HResult hr = nativeSensor.GetID(out sensorId);
            if (hr == HResult.Ok)
            {
                SensorManager.OnSensorsChanged(sensorId, SensorAvailabilityChange.Addition);
            }
        }
    }

    #endregion
}