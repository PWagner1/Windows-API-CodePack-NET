// Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CA2013
namespace Microsoft.WindowsAPICodePack.ExtendedLinguisticServices
{

    /// <summary>
    /// This class serves as the result status of asynchronous calls to ELS and
    /// as the result status of linguistic exceptions.
    /// </summary>
    public struct MappingResultState
    {
        private int _hResult;
        private string _errorMessage;

        internal MappingResultState(int hResult, string errorMessage)
        {
            _hResult = hResult;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a human-readable description of the HResult error code,
        /// as constructed by <see cref="System.ComponentModel.Win32Exception">Win32Exception</see>.
        /// </summary>
        public string ErrorMessage => _errorMessage;

        /// <summary>
        /// Gets the HResult error code associated with this structure.
        /// </summary>
        public int HResult => _hResult;

        /// <summary>
        /// Uses the HResult param as the object hashcode.
        /// </summary>
        /// <returns>An integer hashcode</returns>
        public override int GetHashCode()
        {
            return _hResult;
        }

        /// <summary>
        /// Compares an Object for value equality.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if obj is equal to this instance, false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            // ReSharper disable once ReferenceEqualsWithValueType
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            // ReSharper disable MergeCastWithTypeCheck
            // ReSharper disable ConvertIfStatementToReturnStatement
            if (obj is MappingResultState)
                // ReSharper restore ConvertIfStatementToReturnStatement
                // ReSharper restore MergeCastWithTypeCheck
            {
                return Equals((MappingResultState)obj);
            }

            return false;
        }

        /// <summary>
        /// Compares a <see cref="MappingResultState">MappingResultState</see> obj for value equality.
        /// </summary>
        /// <param name="obj"><see cref="MappingResultState">MappingResultState</see> to compare.</param>
        /// <returns>True if obj is equal to this instance, false otherwise.</returns>
        public bool Equals(MappingResultState obj)
        {
            return obj._hResult == _hResult;
        }

        /// <summary>
        /// Compares two <see cref="MappingResultState">MappingResultState</see> objs for value equality.
        /// </summary>
        /// <param name="one">First <see cref="MappingResultState">MappingResultState</see> to compare.</param>
        /// <param name="two">Second <see cref="MappingResultState">MappingResultState</see> to compare.</param>
        /// <returns>True if the two <see cref="MappingResultState">MappingResultStates</see> are equal, false otherwise.</returns>
        public static bool operator ==(MappingResultState one, MappingResultState two)
        {
            return one.Equals(two);
        }

        /// <summary>
        /// Compares two <see cref="MappingResultState">MappingResultState</see> objs against value equality.
        /// </summary>
        /// <param name="one">First <see cref="MappingResultState">MappingResultState</see> to compare.</param>
        /// <param name="two">Second <see cref="MappingResultState">MappingResultState</see> to compare.</param>
        /// <returns>True if the two <see cref="MappingResultState">MappingResultStates</see> are not equal, false otherwise.</returns>
        public static bool operator !=(MappingResultState one, MappingResultState two)
        {
            return !one.Equals(two);
        }
    }

}
#pragma warning restore CA2013