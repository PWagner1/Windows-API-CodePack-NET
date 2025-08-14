namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Represents the method that will handle the StatChanged event.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public delegate void StateChangedEventHandler(Sensor sender, EventArgs e);