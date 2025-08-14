//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8600, CS8604
namespace Microsoft.WindowsAPICodePack.Net;

/// <summary>
/// An enumerable collection of <see cref="NetworkConnection"/> objects.
/// </summary>
public class NetworkConnectionCollection : IEnumerable<NetworkConnection>
{
    #region Private Fields

    IEnumerable _networkConnectionEnumerable;

    #endregion // Private Fields

    internal NetworkConnectionCollection(IEnumerable networkConnectionEnumerable) => _networkConnectionEnumerable = networkConnectionEnumerable;

    #region IEnumerable<NetworkConnection> Members

    /// <summary>
    /// Returns the strongly typed enumerator for this collection.
    /// </summary>
    /// <returns>A <see cref="System.Collections.Generic.IEnumerator{T}"/> object.</returns>
    public IEnumerator<NetworkConnection> GetEnumerator()
    {
        if (_networkConnectionEnumerable != null)
        {
            foreach (INetworkConnection networkConnection in _networkConnectionEnumerable)
            {
                yield return new(networkConnection);
            }
        }
    }

    #endregion

    #region IEnumerable Members

    /// <summary>
    /// Returns the enumerator for this collection.
    /// </summary>
    ///<returns>A <see cref="System.Collections.IEnumerator"/> object.</returns> 
    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (INetworkConnection networkConnection in _networkConnectionEnumerable)
        {
            yield return new NetworkConnection(networkConnection);
        }
    }

    #endregion
}