//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable AssignNullToNotNullAttribute
namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Internal class to represent the KnownFolder settings/properties
    /// </summary>
    internal class KnownFolderSettings
    {
        private FolderProperties _knownFolderProperties;

        internal KnownFolderSettings(IKnownFolderNative? knownFolderNative)
        {
            GetFolderProperties(knownFolderNative);
        }

        #region Private Methods

        /// <summary>
        /// Populates a structure that contains 
        /// this known folder's properties.
        /// </summary>
        private void GetFolderProperties(IKnownFolderNative? knownFolderNative)
        {
            Debug.Assert(knownFolderNative != null);

            KnownFoldersSafeNativeMethods.NativeFolderDefinition nativeFolderDefinition;
            if (knownFolderNative != null)
            {
                knownFolderNative.GetFolderDefinition(out nativeFolderDefinition);

                try
                {
                    _knownFolderProperties.category = nativeFolderDefinition.category;
                    // ReSharper disable once AssignNullToNotNullAttribute
                    _knownFolderProperties.canonicalName = Marshal.PtrToStringUni(nativeFolderDefinition.name);
                    _knownFolderProperties.description = Marshal.PtrToStringUni(nativeFolderDefinition.description);
                    _knownFolderProperties.parentId = nativeFolderDefinition.parentId;
                    _knownFolderProperties.relativePath = Marshal.PtrToStringUni(nativeFolderDefinition.relativePath);
                    _knownFolderProperties.parsingName = Marshal.PtrToStringUni(nativeFolderDefinition.parsingName);
                    _knownFolderProperties.tooltipResourceId = Marshal.PtrToStringUni(nativeFolderDefinition.tooltip);
                    _knownFolderProperties.localizedNameResourceId =
                        Marshal.PtrToStringUni(nativeFolderDefinition.localizedName);
                    _knownFolderProperties.iconResourceId = Marshal.PtrToStringUni(nativeFolderDefinition.icon);
                    _knownFolderProperties.security = Marshal.PtrToStringUni(nativeFolderDefinition.security);
                    _knownFolderProperties.fileAttributes = (FileAttributes)nativeFolderDefinition.attributes;
                    _knownFolderProperties.definitionOptions = nativeFolderDefinition.definitionOptions;
                    _knownFolderProperties.folderTypeId = nativeFolderDefinition.folderTypeId;
                    _knownFolderProperties.folderType = FolderTypes.GetFolderType(_knownFolderProperties.folderTypeId);

                    bool pathExists;
                    _knownFolderProperties.path = GetPath(out pathExists, knownFolderNative);
                    _knownFolderProperties.pathExists = pathExists;

                    _knownFolderProperties.redirection = knownFolderNative.GetRedirectionCapabilities();

                    // Turn tooltip, localized name and icon resource IDs 
                    // into the actual resources.
                    _knownFolderProperties.tooltip =
                        CoreHelpers.GetStringResource(_knownFolderProperties.tooltipResourceId);
                    _knownFolderProperties.localizedName =
                        CoreHelpers.GetStringResource(_knownFolderProperties.localizedNameResourceId);

                    _knownFolderProperties.folderId = knownFolderNative.GetId();
                }
                finally
                {
                    // Clean up memory. 
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.name);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.description);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.relativePath);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.parsingName);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.tooltip);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.localizedName);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.icon);
                    Marshal.FreeCoTaskMem(nativeFolderDefinition.security);
                }
            }
        }

        /// <summary>
        /// Gets the path of this this known folder.
        /// </summary>
        /// <param name="fileExists">
        /// Returns false if the folder is virtual, or a boolean
        /// value that indicates whether this known folder exists.
        /// </param>
        /// <param name="knownFolderNative">Native IKnownFolder reference</param>
        /// <returns>
        /// A <see cref="System.String"/> containing the path, or <see cref="System.String.Empty"/> if this known folder does not exist.
        /// </returns>
        private string? GetPath(out bool fileExists, IKnownFolderNative? knownFolderNative)
        {
            Debug.Assert(knownFolderNative != null);

            string? kfPath = string.Empty;
            fileExists = true;

            // Virtual folders do not have path.
            if (_knownFolderProperties.category == FolderCategory.Virtual)
            {
                fileExists = false;
                return kfPath;
            }

            try
            {
                if (knownFolderNative != null)
                {
                    kfPath = knownFolderNative.GetPath(0);
                }
            }
            catch (FileNotFoundException)
            {
                fileExists = false;
            }
            catch (DirectoryNotFoundException)
            {
                fileExists = false;
            }

            return kfPath;
        }

        #endregion

        #region KnownFolder Properties

        /// <summary>
        /// Gets the path for this known folder.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? Path => _knownFolderProperties.path;


        /// <summary>
        /// Gets the category designation for this known folder.
        /// </summary>
        /// <value>A <see cref="FolderCategory"/> value.</value>
        public FolderCategory Category => _knownFolderProperties.category;

        /// <summary>
        /// Gets this known folder's canonical name.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? CanonicalName => _knownFolderProperties.canonicalName;

        /// <summary>
        /// Gets this known folder's description.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? Description => _knownFolderProperties.description;

        /// <summary>
        /// Gets the unique identifier for this known folder's parent folder.
        /// </summary>
        /// <value>A <see cref="System.Guid"/> value.</value>
        public Guid ParentId => _knownFolderProperties.parentId;

        /// <summary>
        /// Gets this known folder's relative path.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? RelativePath => _knownFolderProperties.relativePath;

        /// <summary>
        /// Gets this known folder's tool tip text.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? Tooltip => _knownFolderProperties.tooltip;

        /// <summary>
        /// Gets the resource identifier for this 
        /// known folder's tool tip text.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? TooltipResourceId => _knownFolderProperties.tooltipResourceId;

        /// <summary>
        /// Gets this known folder's localized name.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? LocalizedName => _knownFolderProperties.localizedName;

        /// <summary>
        /// Gets the resource identifier for this 
        /// known folder's localized name.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? LocalizedNameResourceId => _knownFolderProperties.localizedNameResourceId;

        /// <summary>
        /// Gets this known folder's security attributes.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? Security => _knownFolderProperties.security;

        /// <summary>
        /// Gets this known folder's file attributes, 
        /// such as "read-only".
        /// </summary>
        /// <value>A <see cref="System.IO.FileAttributes"/> value.</value>
        public FileAttributes FileAttributes => _knownFolderProperties.fileAttributes;

        /// <summary>
        /// Gets an value that describes this known folder's behaviors.
        /// </summary>
        /// <value>A <see cref="DefinitionOptions"/> value.</value>
        public DefinitionOptions DefinitionOptions => _knownFolderProperties.definitionOptions;

        /// <summary>
        /// Gets the unique identifier for this known folder's type.
        /// </summary>
        /// <value>A <see cref="System.Guid"/> value.</value>
        public Guid FolderTypeId => _knownFolderProperties.folderTypeId;

        /// <summary>
        /// Gets a string representation of this known folder's type.
        /// </summary>
        /// <value>A <see cref="System.String"/> object.</value>
        public string? FolderType => _knownFolderProperties.folderType;

        /// <summary>
        /// Gets the unique identifier for this known folder.
        /// </summary>
        /// <value>A <see cref="System.Guid"/> value.</value>
        public Guid FolderId => _knownFolderProperties.folderId;

        /// <summary>
        /// Gets a value that indicates whether this known folder's path exists on the computer. 
        /// </summary>
        /// <value>A bool<see cref="System.Boolean"/> value.</value>
        /// <remarks>If this property value is <b>false</b>, 
        /// the folder might be a virtual folder (<see cref="Category"/> property will
        /// be <see cref="FolderCategory.Virtual"/> for virtual folders)</remarks>
        public bool PathExists => _knownFolderProperties.pathExists;

        /// <summary>
        /// Gets a value that states whether this known folder 
        /// can have its path set to a new value, 
        /// including any restrictions on the redirection.
        /// </summary>
        /// <value>A <see cref="RedirectionCapability"/> value.</value>
        public RedirectionCapability Redirection => _knownFolderProperties.redirection;

        #endregion
    }
}
