//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable LoopCanBeConvertedToQuery

#pragma warning disable CS8600, CS8604
namespace Microsoft.WindowsAPICodePack.Net;

/// <summary>
/// An enumerable collection of <see cref="Network"/> objects.
/// </summary>
public class NetworkCollection : IEnumerable<Network>
{
    #region Private Fields

    IEnumerable _networkEnumerable;

    #endregion // Private Fields

    internal NetworkCollection(IEnumerable networkEnumerable) => _networkEnumerable = networkEnumerable;

    #region IEnumerable<Network> Members

    /// <summary>
    /// Returns the strongly typed enumerator for this collection.
    /// </summary>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/>  object.</returns>
    public IEnumerator<Network> GetEnumerator()
    {
        foreach (INetwork network in _networkEnumerable)
        {
            yield return new(network);
        }
    }

    #endregion

    #region IEnumerable Members

    /// <summary>
    /// Returns the enumerator for this collection.
    /// </summary>
    ///<returns>An <see cref="System.Collections.IEnumerator"/> object.</returns> 
    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (INetwork network in _networkEnumerable)
        {
            yield return new Network(network);
        }
    }

    #endregion
}