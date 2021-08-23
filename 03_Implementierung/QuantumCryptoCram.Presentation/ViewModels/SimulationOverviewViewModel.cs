using System;
using System.Windows;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the SimulationOverviewView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class SimulationOverviewViewModel : Screen
    {
        private readonly INavigationController _navigationController;
        private readonly IWindowManager _windowManager;
        private readonly ISimulationManager _simulationManager;
        private readonly SimulationOptions _options;
        private readonly CredentialsManager _credentialsManager;
        private readonly IDialogViewModelFactory _dialogFactory;
        private bool _isAliceDone;
        private bool _isBobDone;
        private bool _isEveDone;

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
        /// Gets or sets a value indicating whether Alice is done with the key exchange.
        /// </summary>
        public bool IsAliceDone
        {
            get => _isAliceDone;

            set
            {
                if (value == _isAliceDone) return;
                _isAliceDone = value;
                NotifyOfPropertyChange(nameof(CanNavigateToProtocolAnalysisCommand));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Bob is done with the key exchange.
        /// </summary>
        public bool IsBobDone
        {
            get => _isBobDone;

            set
            {
                if (value == _isBobDone) return;
                _isBobDone = value;
                NotifyOfPropertyChange(nameof(CanNavigateToProtocolAnalysisCommand));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Eve is done.
        /// </summary>
        public bool IsEveDone
        {
            get => _isEveDone;

            set
            {
                if (value == _isEveDone) return;
                _isEveDone = value;
                NotifyOfPropertyChange(nameof(CanNavigateToProtocolAnalysisCommand));
            }
        }

        /// <summary>
        /// Determines if the user can navigate to the protocol analysis view.
        /// </summary>
        public bool CanNavigateToProtocolAnalysisCommand
        {
            get
            {
                if (_options.IsEveActive)
                    return IsAliceDone && IsEveDone && IsBobDone;
                return IsAliceDone && IsBobDone;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationOverviewViewModel"/> class.
        /// </summary>
        /// <param name="simulationManager">The simulationManager.</param>
        /// <param name="options">The options.</param>
        /// <param name="credentialsManager">CredentialsManager holding the passwords.</param>
        /// <param name="navigationController">Interface that Stylet provides to navigate between views.</param>
        /// <param name="windowManager">Interface that Stylet provides to display windows.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        public SimulationOverviewViewModel(
            ISimulationManager simulationManager,
            SimulationOptions options,
            CredentialsManager credentialsManager,
            INavigationController navigationController,
            IWindowManager windowManager,
            IDialogViewModelFactory dialogFactory)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _dialogFactory = dialogFactory;
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));
            _simulationManager = simulationManager ?? throw new ArgumentNullException(nameof(simulationManager));

            _simulationManager.StartSimulation();

            IsAliceDone = _simulationManager.IsAliceDone;
            IsBobDone = _simulationManager.IsBobDone;
            IsEveDone = _simulationManager.IsEveDone;
        }

        /// <summary>
        /// Navigate back to the requested side.
        /// </summary>
        public void Back()
        {
            MessageBoxViewModel.ButtonLabels[MessageBoxResult.Yes] = "Ja";
            MessageBoxViewModel.ButtonLabels[MessageBoxResult.No] = "Nein";
            MessageBoxResult result = _windowManager.ShowMessageBox(
                "Wollen Sie die aktuelle Simulation beenden? Alle Fortschritte werden verworfen und es kann eine neue Simulation gestartet werden.",
                "Sind Sie sicher?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _simulationManager.StopSimulation();

                _navigationController.NavigateToMain();
            }
        }

        /// <summary>
        /// Displays the Lernhilfe for this View when pressing the Help-button.
        /// </summary>
        public void Help()
        {
            DocumentationTextDialogViewModel docuVM = _dialogFactory.CreateDocumentationTextDialogViewModel();
            docuVM.DisplayTextLernhilfe(@"Lernhilfe_Simulationserbersicht.md");
        }

        public void NavigateToProtocolAnalysisCommand()
        {
            _navigationController.NavigateToProtocolAnalysisView();
        }

        /// <summary>
        /// Propertiy if Eve is active in this simulation.
        /// </summary>
        public bool IsEveAcitve
        {
            get
            {
                return _options.IsEveActive;
            }
        }

        /// <summary>
        /// Navigates to Alice.
        /// </summary>
        public void NavigateToAliceCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Alice, () => _navigationController.NavigateToAlice());

        /// <summary>
        /// Navigates to Eve.
        /// </summary>
        public void NavigateToEveCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Eve, () => _navigationController.NavigateToEve());

        /// <summary>
        /// Navigates to Bob.
        /// </summary>
        public void NavigateToBobCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Bob, () => _navigationController.NavigateToBob());

        /// <summary>
        /// Sends the message.
        /// </summary>
        public void SendMessageCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Alice, () => _navigationController.NavigateToAliceEncryptionTest());

        /// <summary>
        /// Cracks the message.
        /// </summary>
        public void CrackMessageCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Eve, () => _navigationController.NavigateToEveEncryption());

        /// <summary>
        /// Receives the message.
        /// </summary>
        public void ReceiveMessageCommand() => DoOnPasswordCheckForRole(ProtocolRoleType.Bob, () => _navigationController.NavigateToBobEncryptionTest());

        /// <summary>
        /// Inform the user of having entered a false password.
        /// </summary>
        private void FalsePasswordMessageBox()
        {
            _windowManager.ShowMessageBox("Falsches Passwort!");
        }

        /// <summary>
        /// Executes a given function after checking for credentials of the given role.
        /// </summary>
        /// <param name="roleType">The role whose credentials should be checked.</param>
        /// <param name="action">The function to execute after authorisation.</param>
        private void DoOnPasswordCheckForRole(ProtocolRoleType roleType, Action action)
        {
            bool roleHasPassword = false;
            bool correctPassword;

            switch (roleType)
            {
                case ProtocolRoleType.Alice:
                    roleHasPassword = _credentialsManager.IsAlicePasswordSet;
                    break;

                case ProtocolRoleType.Eve:
                    roleHasPassword = _credentialsManager.IsEvePasswordSet;
                    break;

                case ProtocolRoleType.Bob:
                    roleHasPassword = _credentialsManager.IsBobPasswordSet;
                    break;
            }

            if (roleHasPassword)
            {
                correctPassword = _credentialsManager.EqualsRolePassword(roleType, _dialogFactory.CreatePasswordDialogViewModel().RequestPasswordInput());
            }
            else
            {
                correctPassword = true;
            }

            if (correctPassword)
            {
                action();
            }
            else
            {
                FalsePasswordMessageBox();
            }
        }
    }
}