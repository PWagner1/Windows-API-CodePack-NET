#pragma warning disable CS8605
namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Creates an acceleration measurement from the data in the report.
/// </summary>
public class Acceleration3D
{
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="report">The sensor report to evaluate.</param>
    public Acceleration3D(SensorReport? report)
    {
        if (report == null) { throw new ArgumentNullException(nameof(report)); }

        if (report.Values != null)
        {
            _acceleration[(int)AccelerationAxis.XAxis] =
                (float)report.Values[SensorPropertyKeys.SensorDataTypeAccelerationXG.FormatId][0];
            _acceleration[(int)AccelerationAxis.YAxis] =
                (float)report.Values[SensorPropertyKeys.SensorDataTypeAccelerationYG.FormatId][1];
            _acceleration[(int)AccelerationAxis.ZAxis] =
                (float)report.Values[SensorPropertyKeys.SensorDataTypeAccelerationZG.FormatId][2];
        }
    }

    /// <summary>
    /// Gets the acceleration reported by the sensor.
    /// </summary>
    /// <param name="axis">The axis of the acceleration.</param>
    /// <returns></returns>
    public float this[AccelerationAxis axis] => _acceleration[(int)axis];

    private float[] _acceleration = new float[3];
}