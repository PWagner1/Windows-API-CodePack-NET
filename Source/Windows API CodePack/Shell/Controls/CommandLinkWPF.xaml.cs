
using UserControl = System.Windows.Controls.UserControl;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation
{
    /// <summary>
    /// Interaction logic for CommandLinkWPF.xaml
    /// </summary>
    public partial class CommandLink : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public CommandLink()
        {
            // Throw PlatformNotSupportedException if the user is not running Vista or beyond
            CoreHelpers.ThrowIfNotVista();

            DataContext = this;
            InitializeComponent();
            button.Click += button_Click;
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            e.Source = this;
            if (Click != null)
            {
                Click(sender, e);
            }
        }

        /// <summary>
        /// Routed UI command to use for this button
        /// </summary>
        public RoutedUICommand? Command { get; set; }

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        public event RoutedEventHandler? Click;

        private string? _link;

        /// <summary>
        /// Specifies the main instruction text
        /// </summary>
        public string? Link
        {
            get => _link;
            set
            {
                _link = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Link"));
                }
            }
        }
        private string? _note;

        /// <summary>
        /// Specifies the supporting note text
        /// </summary>
        public string? Note
        {
            get => _note;
            set
            {
                _note = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Note"));
                }
            }
        }
        private ImageSource? _icon;

        /// <summary>
        /// Icon to set for the command link button
        /// </summary>
        public ImageSource? Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Icon"));
                }
            }
        }

        /// <summary>
        /// Indicates if the button is in a checked state
        /// </summary>
        public bool? IsCheck
        {
            get => button.IsChecked;
            set => button.IsChecked = value;
        }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        /// <summary>
        /// Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;
    }
}