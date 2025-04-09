namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
    /// <summary>
    /// Property Enumeration Types
    /// </summary>
    public enum PropEnumType
    {
        /// <summary>
        /// Use DisplayText and either RangeMinValue or RangeSetValue.
        /// </summary>
        DiscreteValue = 0,

        /// <summary>
        /// Use DisplayText and either RangeMinValue or RangeSetValue
        /// </summary>
        RangedValue = 1,

        /// <summary>
        /// Use DisplayText
        /// </summary>
        DefaultValue = 2,

        /// <summary>
        /// Use Value or RangeMinValue
        /// </summary>
        EndRange = 3
    };
}