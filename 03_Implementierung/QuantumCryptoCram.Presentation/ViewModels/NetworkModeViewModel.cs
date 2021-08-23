using System;

using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// vThis is the ViewModel for the NetworkModeView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class NetworkModeViewModel : Screen
    {
        private readonly INavigationController _navigationController;
        private readonly IDialogViewModelFactory _dialogFactory;

        private bool _isPhotonCloneChecked;

        private string _iPServer;

        private string _connectionStatus;

        private bool _isSuccess;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess
        {
            get => _isSuccess;

            set => SetAndNotify(ref _isSuccess, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is photon clone checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is photon clone checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsPhotonCloneChecked
        {
            get => _isPhotonCloneChecked;

            set => SetAndNotify(ref _isPhotonCloneChecked, value);
        }

        /// <summary>
        /// Gets or sets the participant.
        /// </summary>
        /// <value>
        /// The participant.
        /// </value>
        public BindableCollection<string> Participant { get; set; }

        /// <summary>
        /// Gets or sets the ip server.
        /// </summary>
        /// <value>
        /// The ip server.
        /// </value>
        public string IPServer
        {
            get => _iPServer;

            set => SetAndNotify(ref _iPServer, value);
        }

        /// <summary>
        /// Gets or sets the connection status.
        /// </summary>
        /// <value>
        /// The connection status.
        /// </value>
        public string ConnectionStatus
        {
            get => _connectionStatus;

            set => SetAndNotify(ref _connectionStatus, value);
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>
        /// The back command.
        /// </value>
        public Action BackCommand
        {
            get => Back;
        }

        /// <summary>
        /// Gets the help command.
        /// </summary>
        /// <value>
        /// The help command.
        /// </value>
        public Action HelpCommand
        {
            get => Help;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkModeViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">The navigation controller.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">navigationController.</exception>
        public NetworkModeViewModel(INavigationController navigationController, IDialogViewModelFactory dialogFactory)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Navigate back to the requested site.
        /// </summary>
        public void Back() => _navigationController.NavigateToMain();

        /// <summary>
        /// Methode that gets called when the BackButton is pressed.
        /// </summary>
        public void Help()
        {
            DocumentationTextDialogViewModel docuVM = _dialogFactory.CreateDocumentationTextDialogViewModel();
            docuVM.DisplayTextLernhilfe(@"Lernhilfe_Netzwerk_Modus.md");
        }

        /// <summary>
        /// Connect to Server.
        /// </summary>
        public void ConnectCommand()
        {
            //Verbinden/Server starten
        }
    }
}