namespace Microsoft.WindowsAPICodePack.Sensors;

/// <summary>
/// Defines a structure that contains the property ID (key) and value.
/// </summary>
public struct DataFieldInfo : IEquatable<DataFieldInfo>
{
    private PropertyKey _propKey;
    private object _value;

    /// <summary>
    /// Initializes the structure.
    /// </summary>
    /// <param name="propKey">A property ID (key).</param>
    /// <param name="value">A property value. The type must be valid for the property ID.</param>
    public DataFieldInfo(PropertyKey propKey, object value)
    {
        _propKey = propKey;
        _value = value;
    }

    /// <summary>
    /// Gets the property's key.
    /// </summary>
    public PropertyKey Key => _propKey;

    /// <summary>
    /// Gets the property's value.
    /// </summary>
    public object Value => _value;

    /// <summary>
    /// Returns the hash code for a particular DataFieldInfo structure.
    /// </summary>
    /// <returns>A hash code.</returns>
    public override int GetHashCode()
    {
        int valHashCode = _value != null ? _value.GetHashCode() : 0;
        return _propKey.GetHashCode() ^ valHashCode;
    }

    /// <summary>
    /// Determines if this object and another object are equal.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns><b>true</b> if this instance and another object are equal; otherwise <b>false</b>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) { return false; }

        if (!(obj is DataFieldInfo)) { return false; }

        DataFieldInfo other = (DataFieldInfo)obj;
        return _value.Equals(other._value) && _propKey.Equals(other._propKey);
    }

    #region IEquatable<DataFieldInfo> Members

    /// <summary>
    /// Determines if this key and value pair and another key and value pair are equal.
    /// </summary>
    /// <param name="other">The item to compare.</param>
    /// <returns><b>true</b> if equal; otherwise <b>false</b>.</returns>
    public bool Equals(DataFieldInfo other)
    {
        return _value.Equals(other._value) && _propKey.Equals(other._propKey);
    }

    #endregion

    /// <summary>
    /// DataFieldInfo == operator overload
    /// </summary>
    /// <param name="first">The first item to compare.</param>
    /// <param name="second">The second item to compare.</param>
    /// <returns><b>true</b> if equal; otherwise <b>false</b>.</returns>
    public static bool operator ==(DataFieldInfo first, DataFieldInfo second)
    {
        return first.Equals(second);
    }

    /// <summary>
    /// DataFieldInfo != operator overload
    /// </summary>
    /// <param name="first">The first item to compare.</param>
    /// <param name="second">The second item to comare.</param>
    /// <returns><b>true</b> if not equal; otherwise <b>false</b>.</returns>
    public static bool operator !=(DataFieldInfo first, DataFieldInfo second)
    {
        return !first.Equals(second);
    }
}