//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
    internal class ShellPropertyDescriptionsCache
    {
        private ShellPropertyDescriptionsCache()
        {
            _propsDictionary = new Dictionary<PropertyKey, ShellPropertyDescription?>();
        }

        private readonly IDictionary<PropertyKey, ShellPropertyDescription?> _propsDictionary;
        private static ShellPropertyDescriptionsCache? _cacheInstance;

        public static ShellPropertyDescriptionsCache? Cache
        {
            get
            {
                if (_cacheInstance == null)
                {
                    _cacheInstance = new ShellPropertyDescriptionsCache();
                }
                return _cacheInstance;
            }
        }

        public ShellPropertyDescription? GetPropertyDescription(PropertyKey key)
        {
            if (!_propsDictionary.ContainsKey(key))
            {
                _propsDictionary.Add(key, new ShellPropertyDescription(key));
            }
            return _propsDictionary[key];
        }
    }
}
