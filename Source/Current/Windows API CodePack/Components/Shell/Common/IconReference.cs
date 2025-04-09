//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// A refence to an icon resource 
    /// </summary>    
    public struct IconReference
    {
        #region Private members

        private string? _moduleName;
        private string _referencePath;
        static private readonly char[] CommaSeparator = new char[] { ',' };

        #endregion

        /// <summary>
        /// Overloaded constructor takes in the module name and resource id for the icon reference.
        /// </summary>
        /// <param name="moduleName">String specifying the name of an executable file, DLL, or icon file</param>
        /// <param name="resourceId">Zero-based index of the icon</param>
        public IconReference(string? moduleName, int resourceId)
            : this()
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentNullException(nameof(moduleName));
            }

            _moduleName = moduleName;
            ResourceId = resourceId;
            _referencePath = string.Format(CultureInfo.InvariantCulture,
                "{0},{1}", moduleName, resourceId);
        }

        /// <summary>
        /// Overloaded constructor takes in the module name and resource id separated by a comma.
        /// </summary>
        /// <param name="refPath">Reference path for the icon consiting of the module name and resource id.</param>
        public IconReference(string refPath)
            : this()
        {
            if (string.IsNullOrEmpty(refPath))
            {
                throw new ArgumentNullException(nameof(refPath));
            }

            string?[] refParams = refPath.Split(CommaSeparator);

            if (refParams.Length != 2 || string.IsNullOrEmpty(refParams[0]) || string.IsNullOrEmpty(refParams[1]))
            {
                throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(refPath));
            }

            _moduleName = refParams[0];
            if (!string.IsNullOrEmpty(refParams[1]))
            {
                ResourceId = int.Parse(refParams[1], CultureInfo.InvariantCulture);
            }
            else
            {
                throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(refParams));
            }


            _referencePath = refPath;
        }

        /// <summary>
        /// String specifying the name of an executable file, DLL, or icon file
        /// </summary>
        public string? ModuleName
        {
            get => _moduleName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _moduleName = value;
            }
        }

        /// <summary>
        /// Zero-based index of the icon
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Reference to a specific icon within a EXE, DLL or icon file.
        /// </summary>
        public string ReferencePath
        {
            get => _referencePath;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                string?[] refParams = value.Split(CommaSeparator);

                if (refParams.Length != 2 || string.IsNullOrEmpty(refParams[0]) || string.IsNullOrEmpty(refParams[1]))
                {
                    throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(value));
                }

                ModuleName = refParams[0];
                if (refParams != null && !string.IsNullOrEmpty(refParams[1]))
                {
                    ResourceId = int.Parse(refParams[1], CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new ArgumentException(LocalizedMessages.InvalidReferencePath, nameof(refParams));
                }


                _referencePath = value;
            }
        }

        /// <summary>
        /// Implements the == (equality) operator.
        /// </summary>
        /// <param name="icon1">First object to compare.</param>
        /// <param name="icon2">Second object to compare.</param>
        /// <returns>True if icon1 equals icon1; false otherwise.</returns>
        public static bool operator ==(IconReference icon1, IconReference icon2)
        {
            return (icon1._moduleName == icon2._moduleName) &&
                (icon1._referencePath == icon2._referencePath) &&
                (icon1.ResourceId == icon2.ResourceId);
        }

        /// <summary>
        /// Implements the != (unequality) operator.
        /// </summary>
        /// <param name="icon1">First object to compare.</param>
        /// <param name="icon2">Second object to compare.</param>
        /// <returns>True if icon1 does not equals icon1; false otherwise.</returns>
        public static bool operator !=(IconReference icon1, IconReference icon2)
        {
            return !(icon1 == icon2);
        }

        /// <summary>
        /// Determines if this object is equal to another.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true if the objects are equal; false otherwise.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is IconReference)) { return false; }
            return (this == (IconReference)obj);
        }

        /// <summary>
        /// Generates a nearly unique hashcode for this structure.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            if (_moduleName != null)
            {
                int hash = _moduleName.GetHashCode();
                hash = hash * 31 + _referencePath.GetHashCode();
                hash = hash * 31 + ResourceId.GetHashCode();
                return hash;
            }
            else
            {
                return 0;
            }
        }

    }

}
