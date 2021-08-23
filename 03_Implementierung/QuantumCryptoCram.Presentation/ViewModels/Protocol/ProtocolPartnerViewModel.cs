using System;
using System.Linq;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    public abstract class ProtocolPartnerViewModel<TNotebookEntry, TNotebookEntryViewModel>
        : ProtocolRoleViewModel<TNotebookEntry, TNotebookEntryViewModel>
        where TNotebookEntry : ProtocolPartnerNotebookEntry, new()
        where TNotebookEntryViewModel : ProtocolPartnerNotebookEntryViewModel<TNotebookEntry>, new()
    {
        private readonly IProtocolPartner<TNotebookEntry> _protocolPartnerModel;
        private bool _canFinalKeyFinishedCommand;
        private Polarisation _selectedPolarisation;

        /// <summary>
        /// Stylet automatically dis-/enables button that bind on the matching command.
        /// </summary>
        public bool CanFinalKeyFinishedCommand
        {
            get => _canFinalKeyFinishedCommand;
            set => SetAndNotify(ref _canFinalKeyFinishedCommand, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolPartnerViewModel{TNotebookEntry, TNotebookEntryViewModel}"/> class.
        /// </summary>
        /// <param name="protocolPartnerModel">The protocol partner model.</param>
        protected ProtocolPartnerViewModel(IProtocolPartner<TNotebookEntry> protocolPartnerModel)
            : base(protocolPartnerModel)
        {
            _protocolPartnerModel = protocolPartnerModel ?? throw new ArgumentNullException(nameof(protocolPartnerModel));
        }

        #region Public Channel

        /// <summary>
        /// Gets or sets the selected polarisation.
        /// </summary>
        /// <value>
        /// The selected polarisation.
        /// </value>
        public Polarisation SelectedPolarisation
        {
            get => _selectedPolarisation;
            set => SetAndNotify(ref _selectedPolarisation, value);
        }

        public void SelectRandomDatabitsAsPreKeyCommand(string amountStr)
        {
            if (int.TryParse(amountStr, out int amount))
                _protocolPartnerModel.SelectRandomBitsForPreKey(amount);
        }

        /// <summary>
        /// Sends the used polarisations via the quantum pipe to the protocol partner.
        /// </summary>
        public void SendPolarisationsCommand()
        {
            TriggerPublicChannelAnimation();
            _protocolPartnerModel.SendPolarisations();
        }

        /// <summary>
        /// Sends the partner back all matches of their used polarisation that were send by them before.
        /// </summary>
        public void SendPolarisationMatchesCommand()
        {
            TriggerPublicChannelAnimation();
            _protocolPartnerModel.SendPolarisationMatches();
        }

        /// <summary>
        /// Sends selected PreKey bits that are currently selected by the user for the prekey comparison.
        /// </summary>
        public void SendPreKeySelectionForComparisonCommand()
        {
            TriggerPublicChannelAnimation();
            _protocolPartnerModel.SendPrekeySelectionForComparison();
        }

        /// <summary>
        /// Sends the protocol partner back all matches of the PreKeys that were selected by them for prekey comparison.
        /// </summary>
        public void SendPreKeyMatchesCommand()
        {
            TriggerPublicChannelAnimation();
            _protocolPartnerModel.SendPrekeyMatches();
        }

        #endregion Public Channel

        /// <summary>
        /// Automatically check all received partner polarisations that match with our own.
        /// </summary>
        public void AutoCheckPolarisationCommand()
        {
            foreach (TNotebookEntryViewModel entry in ProtocolNotebook)
            {
                ProtocolPartnerNotebookEntry aliceNotebookEntry = entry.InternalNotebookEntry;
                if (aliceNotebookEntry.WasPreKeySelectionSentOrReceived) continue;
                if (aliceNotebookEntry.PolarisationPartner.HasValue)
                {
                    if (aliceNotebookEntry.MyPolarisation == aliceNotebookEntry.PolarisationPartner)
                    {
                        aliceNotebookEntry.IsPolarisationMatching = true;
                    }
                    else
                    {
                        aliceNotebookEntry.IsPolarisationMatching = false;
                    }
                }
                else
                {
                    aliceNotebookEntry.IsPolarisationMatching = false;
                }
            }
        }

        /// <summary>
        /// Automatically check all received partner prekey bits that match with our own.
        /// </summary>
        public void AutoCheckPreKeyBitsCommand()
        {
            foreach (TNotebookEntryViewModel entry in ProtocolNotebook)
            {
                ProtocolPartnerNotebookEntry aliceNotebookEntry = entry.InternalNotebookEntry;

                if (aliceNotebookEntry.PreKeyPartner.HasValue)
                {
                    if (aliceNotebookEntry.MyData == aliceNotebookEntry.PreKeyPartner)
                    {
                        aliceNotebookEntry.IsPreKeyMatching = true;
                    }
                    else
                    {
                        aliceNotebookEntry.IsPreKeyMatching = false;
                    }
                }
                else
                {
                    aliceNotebookEntry.IsPreKeyMatching = false;
                }
            }
        }

        /// <summary>
        /// Wraps a notebook entry where protocol data is noted down by the protocol role and add a PropertyChanged event to each notebook entry.
        /// </summary>
        /// <param name="entryToWrap">The notebook entry to wrap.</param>
        protected override void WrapNotebookEntry(TNotebookEntry entryToWrap)
        {
            base.WrapNotebookEntry(entryToWrap);
            entryToWrap.PropertyChanged += WrappedEntry_PropertyChanged;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the WrappedEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void WrappedEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FinalKey")
            {
                UpdateCanFinalKeyFinished();
            }
        }

        /// <summary>
        /// Evaluates if the user currently can press the FinalKeyFinished Button.
        /// </summary>
        protected void UpdateCanFinalKeyFinished()
        {
            CanFinalKeyFinishedCommand = CanFinalKeyFinished();
        }

        /// <summary>
        /// Command that updates the CanFinalKeyFinished Property depends on if there is at least one entry that is used as final key.
        /// </summary>
        /// <returns>Whether the FinalKeyFinished button can be Pressed. </returns>
        protected virtual bool CanFinalKeyFinished()
        {
            return ProtocolNotebook.FirstOrDefault(entry => entry.InternalNotebookEntry.FinalKey.HasValue) != null;
        }
    }
}