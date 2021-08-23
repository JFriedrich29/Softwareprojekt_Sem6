using System;

using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the LocalModeView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class LocalModeViewModel : Screen
    {
        private readonly Func<PasswordDialogViewModel> _passwordDialogCreator;
        private readonly INavigationController _navigationController;
        private readonly SimulationOptions _options;
        private readonly CredentialsManager _credentialsManager;
        private readonly IDialogViewModelFactory _dialogFactory;
        private bool _isPhotonCloningChecked;
        private bool _isEveChecked;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is photon cloning checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is photon cloning checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsPhotonCloningChecked
        {
            get
            {
                _isPhotonCloningChecked = _options.IsPhotonCloningEnabled;
                return _isPhotonCloningChecked;
            }

            set
            {
                SetAndNotify(ref _isPhotonCloningChecked, value);
                _options.IsPhotonCloningEnabled = _isPhotonCloningChecked;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is eve checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is eve checked; otherwise, <c>false</c>.
        /// </value>
        public bool IsEveChecked
        {
            get
            {
                _isEveChecked = _options.IsEveActive;
                return _isEveChecked;
            }

            set
            {
                SetAndNotify(ref _isEveChecked, value);
                _options.IsEveActive = _isEveChecked;
            }
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
        /// Gets or sets the help callback command.
        /// </summary>
        /// <value>
        /// The help callback command.
        /// </value>
        public Action HelpCallbackCommand
        {
            get => HelpCallback;

            set
            {
                if (value != null)
                {
                    HelpCallbackCommand = value;
                }
            }
        }

        public CredentialsManager CredentialsManager
        {
            get { return _credentialsManager; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalModeViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">The navigation controller.</param>
        /// <param name="options">The options.</param>
        /// <param name="credentialsManager">CredentialsManager storing passwords for the roles.</param>
        /// <param name="passwordDialogCreator">Function for creating the password input.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">navigationController.</exception>
        public LocalModeViewModel(
            INavigationController navigationController,
            SimulationOptions options,
            CredentialsManager credentialsManager,
            Func<PasswordDialogViewModel> passwordDialogCreator,
            IDialogViewModelFactory dialogFactory)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _passwordDialogCreator = passwordDialogCreator ?? throw new ArgumentNullException(nameof(passwordDialogCreator));
            _dialogFactory = dialogFactory;
            _credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));
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
            docuVM.DisplayTextLernhilfe(@"Lernhilfe_Lokaler_Modus.md");
        }

        /// <summary>
        /// Starts the simulation.
        /// </summary>
        public void StartSimulationCommand() => _navigationController.NavigateToSimulationOverviewView();

        /// <summary>
        /// Sets a Password for the Role specified by the <paramref name="roleType"/> parameter.
        /// </summary>
        /// <param name="roleType">The type of the targeted Role.</param>
        public void SetPasswordForRole(ProtocolRoleType roleType)
        {
            string newPassword = _passwordDialogCreator().RequestPasswordInput();

            if (newPassword == null)
            {
                return;
            }

            switch (roleType)
            {
                case ProtocolRoleType.Alice:
                    _credentialsManager.AlicePassword = newPassword;
                    break;

                case ProtocolRoleType.Eve:
                    _credentialsManager.EvePassword = newPassword;
                    break;

                case ProtocolRoleType.Bob:
                    _credentialsManager.BobPassword = newPassword;
                    break;

                default:
                    throw new InvalidOperationException("Trying to set password for invalid role.");
            }
        }

        /// <summary>
        /// Calls whatever should happen if the HelpButton is pressed.
        /// </summary>
        public void HelpCallback()
        {
            return;
        }
    }
}