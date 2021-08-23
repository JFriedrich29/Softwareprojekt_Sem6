using System;
using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    /// <summary>
    /// This is the ViewModel for the EveView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class EveViewModel : ProtocolRoleViewModel<EveNotebookEntry, EveNotebookEntryViewModel>
    {
        private readonly INavigationController _navigationController;
        private readonly IEve _eve;
        private readonly IDialogViewModelFactory _dialogFactory;
        private string _amountRandomPhotonMeasurement;
        private bool _canMeasurementCompletedCommand;

        private string _pendingPhotonsCount;

        /// <summary>
        /// Gets or sets the number of photons pending.
        /// </summary>
        /// <value>
        /// The number of photons pending.
        /// </value>
        public string PendingPhotonsCount
        {
            get => _pendingPhotonsCount;
            set => SetAndNotify(ref _pendingPhotonsCount, "Anstehende Photonen: " + value);
        }

        /// <summary>
        /// Gets or sets the amount random photon measurement.
        /// </summary>
        /// <value>
        /// The amount random photon measurement.
        /// </value>
        public string AmountRandomPolarisations
        {
            get => _amountRandomPhotonMeasurement;
            set => SetAndNotify(ref _amountRandomPhotonMeasurement, value);
        }

        /// <summary>
        /// Notes down a polarisation for measuring incoming photons.
        /// </summary>
        /// <param name="selectedPolarisation">The polarisation the user selected.</param>
        public void NoteDownPolarisationCommand(Polarisation selectedPolarisation)
        {
            _eve.NoteDownPolarisation(selectedPolarisation);
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
        /// Stylet automatically dis-/enables button that bind on the matching command.
        /// </summary>
        public bool CanMeasurementCompletedCommand
        {
            get => _canMeasurementCompletedCommand;

            set => SetAndNotify(ref _canMeasurementCompletedCommand, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EveViewModel"/> class.
        /// </summary>
        /// <param name="navigationController"> Interface that Stylet provides to navigate between views.</param>
        /// <param name="eve">Reference to the main application logic class <see cref="Eve"/>.</param>
        /// <param name="simulationManager">Reference to the simulation manager <see cref="ISimulationManager"/>.</param>
        /// <param name="dialogViewModelFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        public EveViewModel(
            INavigationController navigationController,
            IEve eve,
            ISimulationManager simulationManager,
            IDialogViewModelFactory dialogViewModelFactory)
            : base(eve)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _eve = eve ?? throw new ArgumentNullException(nameof(eve));
            _dialogFactory = dialogViewModelFactory ?? throw new ArgumentNullException(nameof(dialogViewModelFactory));
            _eve.PendingPhotonsUpdated += PendingPhotonsUpdatedHandler;
            PendingPhotonsCount = _eve.PendingPhotonsCount.ToString();

            IsProtocolInProgress = !simulationManager.IsEveDone;
            CanMeasurementCompletedCommand = simulationManager.IsAliceDone;
        }

        /// <summary>
        /// Handle change of pendingPhotons.
        /// </summary>
        private void PendingPhotonsUpdatedHandler(object sender, EventArgs e)
        {
            PendingPhotonsCount = _eve.PendingPhotonsCount.ToString();
            TriggerQuantumChannelAnimation();
        }

        /// <summary>
        /// Navigate back to the requested site.
        /// </summary>
        public void Back() => _navigationController.NavigateToSimulationOverviewView();

        /// <summary>
        /// Method that gets called when the BackButton is pressed.
        /// </summary>
        public void Help()
        {
            DocumentationDialogViewModel docuVM = _dialogFactory.CreateDocumentationDialogViewModel();
            docuVM.DisplayLernhilfe(@"Lernhilfe_Eve.md", @"EveObEr.png");
        }

        /// <summary>
        /// Generates the random polarisations.
        /// </summary>
        public void NoteDownRandomPolarisationsCommand()
        {
            if (int.TryParse(AmountRandomPolarisations, out int amount))
                _eve.NoteDownRandomPolarisations(amount);
        }

        /// <summary>
        /// Copies the polarisations from the given role.
        /// </summary>
        /// <param name="roleType">The role to copy from.</param>
        public void CopyPolarisationsFromRole(ProtocolRoleType roleType)
        {
            foreach (EveNotebookEntryViewModel entry in ProtocolNotebook)
            {
                Polarisation? rolePolarisation;
                switch (roleType)
                {
                    case ProtocolRoleType.Alice:
                        rolePolarisation = entry.InternalNotebookEntry.PolarisationAlice;
                        break;
                    case ProtocolRoleType.Bob:
                        rolePolarisation = entry.InternalNotebookEntry.PolarisationBob;
                        break;
                    case ProtocolRoleType.Eve:
                    default:
                        throw new ArgumentException("Invalid roleType: " + roleType);
                }

                if (rolePolarisation.HasValue
                    && !entry.InternalNotebookEntry.MyData.HasValue)
                {
                    _eve.NoteDownPolarisation(rolePolarisation.Value, entry.InternalNotebookEntry);
                }
            }
        }

        /// <summary>
        /// Measurements the completed.
        /// </summary>
        public void MeasurementCompletedCommand()
        {
            _eve.ProtocolDone();
            IsProtocolInProgress = false;
        }
    }
}