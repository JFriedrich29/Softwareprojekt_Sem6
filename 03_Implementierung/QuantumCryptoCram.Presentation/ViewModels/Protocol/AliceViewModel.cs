using System;
using System.Collections.ObjectModel;

using QuantumCryptoCram.Application;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    /// <summary>
    /// This is the ViewModel for the AliceView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class AliceViewModel : ProtocolPartnerViewModel<AliceNotebookEntry, AliceNotebookEntryViewModel>
    {
        private readonly INavigationController _navigationController;
        private readonly IAlice _alice;
        private readonly IDialogViewModelFactory _dialogFactory;
        private bool _equalPolarisation;
        private DataBit _selectedBit;

        /// <summary>
        /// Gets the AliceNotebookEntryViewModels.
        /// </summary>
        /// <value>
        /// The AliceNotebookEntryViewModels.
        /// </value>
        public ObservableCollection<AliceNotebookEntryViewModel> AliceNotebookEntryViewModels { get; }

        /// <summary>
        /// Gets the data table the holds the data for the encryption testing.
        /// </summary>
        public ObservableCollection<EncryptionTestEntry> EncryptionTestNotebook { get; }

        /// <summary>
        /// Gets or sets the selected bit.
        /// </summary>
        /// <value>
        /// The selected bit.
        /// </value>
        public DataBit SelectedBit
        {
            get => _selectedBit;
            set => SetAndNotify(ref _selectedBit, value);
        }

        /// <summary>
        /// Gets or Sets the value if Polarisation Bob is equal to Polarisation Alice.
        /// </summary>
        public bool EqualPolarisation
        {
            get => _equalPolarisation;
            set => SetAndNotify(ref _equalPolarisation, value);
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>
        /// The back command.
        /// </value>
        public Action BackCommand => Back;

        /// <summary>
        /// Gets the help command.
        /// </summary>
        /// <value>
        /// The help command.
        /// </value>
        public Action HelpCommand => Help;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliceViewModel"/> class.
        /// </summary>
        /// <param name="alice">The alice.</param>
        /// <param name="navigationController">INavigationController navigationController.</param>
        /// <param name="simulationManager">Reference to the simulation manager <see cref="ISimulationManager"/>.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        public AliceViewModel(
            INavigationController navigationController,
            IAlice alice,
            ISimulationManager simulationManager,
            IDialogViewModelFactory dialogFactory)
            : base(alice)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _alice = alice ?? throw new ArgumentNullException(nameof(alice));
            _dialogFactory = dialogFactory;
            IsProtocolInProgress = !simulationManager.IsAliceDone;
            UpdateCanFinalKeyFinished();
        }

        #region Quantum Channel

        /// <summary>
        /// Generates Photons based on the chosen configurations and sends them via the quantum channel.
        /// </summary>
        public void SendPhotonsCommand()
        {
            TriggerQuantumChannelAnimation();
            _alice.SendPhotons();
        }

        #endregion Quantum Channel

        /// <summary>
        /// Polarisations the changed.
        /// </summary>
        /// <param name="pol">The Polarisation.</param>
        public void PolarisationChangedCommand(Polarisation pol)
        {
            //SelectedPolarisation = (Polarisation)Enum.Parse(typeof(Polarisation), pol);
            SelectedPolarisation = pol;
        }

        /// <summary>
        /// Bits the changed.
        /// </summary>
        /// <param name="bit">The DataBit.</param>
        public void BitChangedCommand(DataBit bit)
        {
            SelectedBit = bit;
        }

        /// <summary>
        /// Notes down photon.
        /// </summary>
        public void NoteDownPhotonCommand()
        {
            _alice.NoteDownPhoton(SelectedBit, SelectedPolarisation);
        }

        public void NoteDownRandomPhotonsCommand(string amountStr)
        {
            if (int.TryParse(amountStr, out int amount))
                _alice.NoteDownRandomPhotons(amount);
        }

        /// <summary>
        /// Navigate back to the requested side.
        /// </summary>
        public void Back() => _navigationController.NavigateToSimulationOverviewView();

        /// <summary>
        /// Methode that gets called when the HelpButton is pressed.
        /// </summary>
        public void Help()
        {
            DocumentationDialogViewModel docuVM = _dialogFactory.CreateDocumentationDialogViewModel();
            docuVM.DisplayLernhilfe(@"Lernhilfe_Alice.md", @"AliceObEr.png");
        }

        /// <summary>
        /// Command that will be executed if the finalkey is ready.
        /// </summary>
        public void FinalKeyFinishedCommand()
        {
            _alice.ProtocolDone();
            IsProtocolInProgress = false;
        }
    }
}