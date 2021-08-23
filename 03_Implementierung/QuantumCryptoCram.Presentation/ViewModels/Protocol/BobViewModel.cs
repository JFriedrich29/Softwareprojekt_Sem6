using System;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    /// <summary>
    /// This is the ViewModel for the BobView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class BobViewModel : ProtocolPartnerViewModel<BobNotebookEntry, BobNotebookEntryViewModel>
    {
        private readonly INavigationController _navigationController;
        private readonly IBob _bob;
        private readonly ISimulationManager _simulationManager;
        private readonly SimulationOptions _options;
        private readonly IDialogViewModelFactory _dialogFactory;
        private string _randomPolarisations;
        private string _pendingPhotonsCount;

        /// <summary>
        /// Notes down a polarisation for measuring incoming photons.
        /// </summary>
        /// <param name="selectedPolarisation"> The selected polarisation.</param>
        public void NoteDownPolarisationCommand(Polarisation selectedPolarisation)
        {
            SelectedPolarisation = selectedPolarisation;
            _bob.NoteDownPolarisation(selectedPolarisation);
        }

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
        /// Gets or sets the amount of random polarisations.
        /// </summary>
        /// <value>
        /// The random polarisations.
        /// </value>
        public string AmountRandomPolarisations
        {
            get => _randomPolarisations;
            set => SetAndNotify(ref _randomPolarisations, value);
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
        /// Initializes a new instance of the <see cref="BobViewModel"/> class.
        /// </summary>
        /// <param name="navigationController"> Controller for navigation between views.</param>
        /// <param name="bob">Reference to the main application logic class <see cref="Bob"/>.</param>
        /// <param name="simulationManager">Reference to the simulation manager <see cref="ISimulationManager"/>.</param>
        /// <param name="options">Reference to the simulation options <see cref="SimulationOptions"/>.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        public BobViewModel(
            INavigationController navigationController,
            IBob bob,
            ISimulationManager simulationManager,
            SimulationOptions options,
            IDialogViewModelFactory dialogFactory)
            : base(bob)
        {
            _navigationController =
                navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _bob = bob ?? throw new ArgumentNullException(nameof(bob));
            _simulationManager = simulationManager ?? throw new ArgumentNullException(nameof(simulationManager));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dialogFactory = dialogFactory ?? throw new ArgumentNullException(nameof(dialogFactory));
            _bob.PendingPhotonsUpdated += PendingPhotonsUpdatedHandler;
            PendingPhotonsCount = _bob.PendingPhotonsCount.ToString();

            IsProtocolInProgress = !_simulationManager.IsBobDone;
            UpdateCanFinalKeyFinished();
        }

        /// <summary>
        /// Handle change of pendingPhotons.
        /// </summary>
        private void PendingPhotonsUpdatedHandler(object sender, EventArgs e)
        {
            PendingPhotonsCount = _bob.PendingPhotonsCount.ToString();
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
            docuVM.DisplayLernhilfe(@"Lernhilfe_Bob.md", @"BobObEr.png");
        }

        /// <summary>
        /// Generates the random polarisations.
        /// </summary>
        public void NoteDownRandomPolarisationsCommand()
        {
            if (int.TryParse(AmountRandomPolarisations, out int amount))
                _bob.NoteDownRandomPolarisations(amount);
        }

        /// <summary>
        /// Command that will be executed if the finalkey is ready.
        /// </summary>
        public void FinalKeyFinishedCommand()
        {
            _bob.ProtocolDone();
            IsProtocolInProgress = false;
        }

        /// <inheritdoc/>
        protected override bool CanFinalKeyFinished()
        {
            if (_options.IsEveActive && !_simulationManager.IsEveDone)
                return false;
            return _simulationManager.IsAliceDone && base.CanFinalKeyFinished();
        }
    }
}