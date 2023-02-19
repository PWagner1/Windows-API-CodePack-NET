namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Defines the data passed to the SensorsChangedHandler.
/// </summary>
public class SensorsChangedEventArgs : EventArgs
{
    private SensorAvailabilityChange _change;
    private Guid _sensorId;

    /// <summary>
    /// The type of change. 
    /// </summary>
    public SensorAvailabilityChange Change
    {
        get => _change;
        set => _change = value;
    }

    /// <summary>
    /// The ID of the sensor that changed.
    /// </summary>
    public Guid SensorId
    {
        get => _sensorId;
        set => _sensorId = value;
    }

    internal SensorsChangedEventArgs(Guid sensorId, SensorAvailabilityChange change)
    {
        SensorId = sensorId;
        Change = change;
    }
}