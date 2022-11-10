namespace Microsoft.WindowsAPICodePack.Net.ExtendedLinguisticServices
{
    /// <summary>
    /// Contains options used to enumerate ELS services.
    /// </summary>
    public class MappingEnumOptions
    {
        internal Nullable<Guid> _guid;
        internal Win32EnumOptions _win32EnumOption;

        /// <summary>
        /// Public constructor. Initializes an empty instance of <see cref="MappingEnumOptions">MappingEnumOptions</see>.
        /// </summary>
        public MappingEnumOptions()
        {
            _win32EnumOption._size = InteropTools.SizeOfWin32EnumOptions;
        }

        /// <summary>
        /// Optional. A service category, for example, "Transliteration". The application must set this member to null
        /// if the service category is not a search criterion.
        /// </summary>
        public string Category
        {
            get
            {
                return _win32EnumOption._category;
            }
            set
            {
                _win32EnumOption._category = value;
            }
        }

        /// <summary>
        /// Optional. An input language string, following the IETF naming convention, that identifies the input language
        /// that services should accept. The application can set this member to null if the supported input language is
        /// not a search criterion.
        /// </summary>
        public string InputLanguage
        {
            get
            {
                return _win32EnumOption._inputLanguage;
            }
            set
            {
                _win32EnumOption._inputLanguage = value;
            }
        }

        /// <summary>
        /// Optional. An output language string, following the IETF naming convention, that identifies the output language
        /// that services use to retrieve results. The application can set this member to null if the output language is
        /// not a search criterion.
        /// </summary>
        public string OutputLanguage
        {
            get
            {
                return _win32EnumOption._outputLanguage;
            }
            set
            {
                _win32EnumOption._outputLanguage = value;
            }
        }

        /// <summary>
        /// Optional. A standard Unicode script name that can be accepted by services. The application set this member to
        /// null if the input script is not a search criterion.
        /// </summary>
        public string InputScript
        {
            get
            {
                return _win32EnumOption._inputScript;
            }
            set
            {
                _win32EnumOption._inputScript = value;
            }
        }

        /// <summary>
        /// Optional. A standard Unicode script name used by services. The application can set this member to
        /// null if the output script is not a search criterion.
        /// </summary>
        public string OutputScript
        {
            get
            {
                return _win32EnumOption._outputScript;
            }
            set
            {
                _win32EnumOption._outputScript = value;
            }
        }

        /// <summary>
        /// Optional. A string, following the format of the MIME content types, that identifies the format that the
        /// services should be able to interpret when the application passes data. Examples of content types are
        /// "text/plain", "text/html", and "text/css". The application can set this member to null if the input content
        /// type is not a search criterion.
        ///
        /// <note>In Windows 7, the ELS services support only the content type "text/plain". A content type specification
        /// can be found at the IANA website: http://www.iana.org/assignments/media-types/text/ </note>
        /// </summary>
        public string InputContentType
        {
            get
            {
                return _win32EnumOption._inputContentType;
            }
            set
            {
                _win32EnumOption._inputContentType = value;
            }
        }

        /// <summary>
        /// Optional. A string, following the format of the MIME content types, that identifies the format in which the
        /// services retrieve data. The application can set this member to null if the output content type is not a search
        /// criterion.
        /// </summary>
        public string OutputContentType
        {
            get
            {
                return _win32EnumOption._outputContentType;
            }
            set
            {
                _win32EnumOption._outputContentType = value;
            }
        }

        /// <summary>
        /// Optional. A globally unique identifier (guid) structure for a specific service. The application must
        /// avoid setting this member at all if the guid is not a search criterion.
        /// </summary>
        public Nullable<Guid> Guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }
    }
}
