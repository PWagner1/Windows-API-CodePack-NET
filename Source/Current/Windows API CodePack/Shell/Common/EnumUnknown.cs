//Copyright (c) Microsoft Corporation.  All rights reserved.

#pragma warning disable CS8604
namespace Microsoft.WindowsAPICodePack.Shell
{
    internal class EnumUnknownClass : IEnumUnknown
    {
        readonly List<ICondition?> _conditionList = new();
        int _current = -1;

        internal EnumUnknownClass(ICondition?[] conditions)
        {
            _conditionList.AddRange(conditions);
        }

        #region IEnumUnknown Members

        public HResult Next(uint requestedNumber, ref IntPtr buffer, ref uint fetchedNumber)
        {
            _current++;

            if (_current < _conditionList.Count)
            {
                buffer = Marshal.GetIUnknownForObject(_conditionList[_current]);
                fetchedNumber = 1;
                return HResult.Ok;
            }

            return HResult.False;
        }

        public HResult Skip(uint number)
        {
            int temp = _current + (int)number;

            if (temp > (_conditionList.Count - 1))
            {
                return HResult.False;
            }

            _current = temp;
            return HResult.Ok;
        }

        public HResult Reset()
        {
            _current = -1;
            return HResult.Ok;
        }

        public HResult Clone(out IEnumUnknown result)
        {
            result = new EnumUnknownClass(_conditionList.ToArray());
            return HResult.Ok;
        }

        #endregion
    }
}