#pragma warning disable CS8605
namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Defines a luminous intensity measurement. 
/// </summary>
public class LuminousIntensity
{
    /// <summary>
    /// Initializes a sensor report to obtain a luminous intensity value.
    /// </summary>
    /// <param name="report">The report name.</param>
    /// <returns></returns>
    public LuminousIntensity(SensorReport? report)
    {
        if (report == null) { throw new ArgumentNullException(nameof(report)); }

        if (report.Values != null &&
            report.Values.ContainsKey(SensorPropertyKeys.SensorDataTypeLightLux.FormatId))
        {
            Intensity =
                (float)report.Values[SensorPropertyKeys.SensorDataTypeLightLux.FormatId][0];
        }
    }
    /// <summary>
    /// Gets the intensity of the light in lumens.
    /// </summary>
    public float Intensity { get; private set; }        
}