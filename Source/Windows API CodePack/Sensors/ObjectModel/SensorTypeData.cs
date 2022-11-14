namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Data associated with a sensor type GUID.
/// </summary>
internal struct SensorTypeData
{
    private Type sensorType;
    private SensorDescriptionAttribute sda;

    public SensorTypeData(Type sensorClassType, SensorDescriptionAttribute sda)
    {
        sensorType = sensorClassType;
        this.sda = sda;
    }

    public Type SensorType
    {
        get { return sensorType; }
    }
        
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public SensorDescriptionAttribute Attr
    {
        get { return sda; }
    }
}