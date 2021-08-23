using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Representation of correspondents in the BB84 protocol.
    /// </summary>
    /// <typeparam name="TEntry"> Defines which concrete type of a notebook entry the child class works with. </typeparam>
    public abstract class ProtocolPartner<TEntry> : ProtocolRole<TEntry>, IProtocolPartner<TEntry>
        where TEntry : ProtocolPartnerNotebookEntry, new()
    {
        private bool _isDisposed;

        /// <inheritdoc/>
        public bool DetectedEve { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolPartner{TEntry}"/> class.
        /// </summary>
        /// <param name="notebook">A List <see cref="ProtocolRoleNotebookEntry"/> a protocol participant uses to keep track of information. Here it is passed to <see cref="ProtocolRole{TEntry}"/>.</param>
        /// <param name="publicNetwork">A public unencrypted network that protocol participants can use to communicate.</param>
        /// <param name="randomGenerator">A <see cref="IRandomGenerator"/> which is used by all inheriting classes to do random operations. Here it is passed to <see cref="ProtocolRole{TEntry}"/>.</param>
        protected ProtocolPartner(ObservableCollection<TEntry> notebook, IPublicNetwork publicNetwork, IRandomGenerator randomGenerator)
           : base(notebook, publicNetwork, randomGenerator)
        {
            // Subscribe to public network messages
            PublicNetwork.Subscribe<PolarisationMatchesMessage>(this, ReceivePolarisationMatches);
            PublicNetwork.Subscribe<PreKeySelectionForComparisonMessage>(this, ReceivePreKeySelectionForComparison);
            PublicNetwork.Subscribe<PreKeyComparisonResultMessage>(this, ReceivePreKeyComparisonResult);
            PublicNetwork.Subscribe<PolarisationUsedMessage>(this, ReceivePolarisationUsed);
        }

        /// <inheritdoc/>
        public Key GetFinalKey()
        {
            return new Key(MyNotebook.Where(e => e.IsValidEntry() && e.FinalKey.HasValue).Select(e => e.FinalKey.Value));
        }

        /// <inheritdoc/>
        public void SendPolarisations()
        {
            // Get the entries whose polarisation has not been published yet.
            var toSend = MyNotebook.Where(entry =>
                !entry.WasPolarisationSent &&
                entry.IsValidEntry())
                .ToList();

            if (toSend.Count == 0) return;

            // Mark all the polarisations as sent.
            foreach (TEntry entry in toSend)
            {
                entry.WasPolarisationSent = true;
            }

            // Build a dictionary with the ids as keys and polarisations as values.
            // This dictionary will then be sent over the network.
            var polarisations = toSend
                .ToDictionary(entry => entry.Id, entry => entry.MyPolarisation.Value);
            PublicNetwork.PublishMessage(this, new PolarisationUsedMessage(Role, polarisations));
        }

        /// <inheritdoc/>
        public void SendPolarisationMatches()
        {
            var matches = new List<int>();
            foreach (TEntry entry in MyNotebook)
            {
                if (entry.IsPolarisationMatching)
                {
                    matches.Add(entry.Id);
                }
            }

            //since there could be no matching entries between the noted down polarisations,
            //it's possible to have to send a message with an empty match list
            var polarisationMatchesMessage = new PolarisationMatchesMessage(Role, matches);
            PublicNetwork.PublishMessage(this, polarisationMatchesMessage);
        }

        /// <inheritdoc/>
        public void SendPrekeySelectionForComparison()
        {
            var selectionInfo = new Dictionary<int, DataBit>();
            IEnumerable<TEntry> selectedUnsentEntries = MyNotebook.Where(entry =>
                entry.IsValidEntry() &&
                entry.IsPreKeySelectedForComparison &&
                entry.WasPreKeySelectionSentOrReceived == false);

            if (selectedUnsentEntries.Count() == 0) return;

            foreach (TEntry entry in selectedUnsentEntries)
            {
                entry.WasPreKeySelectionSentOrReceived = true;
                entry.FinalKey = null;
                if (entry.PreKey.HasValue)
                    selectionInfo.Add(entry.Id, entry.PreKey.Value);
                else
                    throw new ProtocolException("Prekey bit is selected for prekey comparison, but it has no value!");
            }

            PublicNetwork.PublishMessage(this, new PreKeySelectionForComparisonMessage(selectionInfo));
        }

        /// <inheritdoc/>
        public void SendPrekeyMatches()
        {
            var matches = MyNotebook
                .Where(entry => entry.IsPreKeyMatching)
                .Select(entry => entry.Id).ToList();

            if (matches.Count > 0)
            {
                var preKeyComparisonResultMessage = new PreKeyComparisonResultMessage(matches);
                PublicNetwork.PublishMessage(this, preKeyComparisonResultMessage);
            }
        }

        /// <inheritdoc/>
        public void SelectRandomBitsForPreKey(int count)
        {
            foreach (TEntry entry in MyNotebook)
            {
                if (entry.IsPreKeySelectedForComparison == true && entry.WasPreKeySelectionSentOrReceived == false) entry.IsPreKeySelectedForComparison = false;
            }

            IEnumerable<TEntry> entriesToSelect = MyNotebook.Where(entry =>
                entry.MyData.HasValue &&
                entry.MyPolarisation.HasValue &&
                entry.IsPolarisationMatching &&
                entry.IsPreKeySelectedForComparison == false &&
                entry.WasPreKeySelectionSentOrReceived == false);

            if (count > 0 && count <= entriesToSelect.Count())
            {
                IList<TEntry> randomlyPickedPreKeyBits = RandomGenerator.PickRandomItems(entriesToSelect, count);

                foreach (TEntry entry in randomlyPickedPreKeyBits)
                {
                    entry.IsPreKeySelectedForComparison = true;

                    MyNotebook[entry.Id] = entry;
                }
            }
        }

        /// <summary>
        /// Handle the polarisation used message and note down the polarisation of the partner.
        /// </summary>
        /// <param name="message">The public message containing the polarisations.</param>
        protected void ReceivePolarisationUsed(PolarisationUsedMessage message)
        {
            foreach (KeyValuePair<int, Polarisation> item in message.Polarisations)
            {
                MyNotebook.SafelyGetEntry(item.Key).PolarisationPartner = item.Value;
            }
        }

        /// <summary>
        /// Sets the data bit as PreKey, when the partner answers, that previously sent polarisations are matching with their own.
        /// </summary>
        /// <param name="message">Message, which polarisations are matching.</param>
        protected virtual void ReceivePolarisationMatches(PolarisationMatchesMessage message)
        {
            foreach (TEntry entry in MyNotebook)
            {
                if (entry.WasPolarisationSent)
                {
                    if (message.Matches.Contains(entry.Id))
                    {
                        entry.IsPolarisationMatching = true;
                        entry.PolarisationPartner = entry.MyPolarisation;
                    }
                    else
                    {
                        entry.IsPolarisationMatching = false;
                        entry.PolarisationPartner = entry.MyPolarisation == Polarisation.Diagonal ? Polarisation.Rectilinear : Polarisation.Diagonal;
                    }
                }
            }
        }

        /// <summary>
        /// Inserts prekey comparison bits of partner to notebook, which where sent for prekey comparison.
        /// </summary>
        /// <param name="message">List of Entry Id and it's DataBit (one or zero).</param>
        protected virtual void ReceivePreKeySelectionForComparison(PreKeySelectionForComparisonMessage message)
        {
            foreach (KeyValuePair<int, DataBit> selectedPreKeyBit in message.SelectedPreKeyBits)
            {
                TEntry notebookEntry = MyNotebook.SafelyGetEntry(selectedPreKeyBit.Key);
                notebookEntry.PreKeyPartner = selectedPreKeyBit.Value;
                notebookEntry.FinalKey = null;
                notebookEntry.WasPreKeySelectionSentOrReceived = true;
            }
        }

        /// <summary>
        /// When the prekey comparison of one entry with the corresponding entry of the partner was successful,
        /// the prekey bit of the partner gets the same value as the own prekey bit.
        /// Otherwise the partner has the opposite prekey bit in the corresponding Notebook entry,
        /// so it'll get the opposite bit of own prekey bit of entry.
        /// </summary>
        /// <param name="message">Contains entry Ids where the prekey comparison was successful.</param>
        protected virtual void ReceivePreKeyComparisonResult(PreKeyComparisonResultMessage message)
        {
            IEnumerable<TEntry> sentEntries = MyNotebook.Where(entry => entry.WasPreKeySelectionSentOrReceived);
            foreach (TEntry entry in sentEntries)
            {
                entry.IsPreKeyMatching = message.Matches.Contains(entry.Id);
                if (entry.IsPreKeyMatching)
                {
                    entry.PreKeyPartner = entry.MyData;
                }
                else
                {
                    entry.PreKeyPartner = entry.MyData == DataBit.One ? DataBit.Zero : DataBit.One;
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Clear managed resources.

                if (PublicNetwork != null)
                {
                    PublicNetwork.Unsubscribe<PolarisationMatchesMessage>(this, ReceivePolarisationMatches);
                    PublicNetwork.Unsubscribe<PreKeySelectionForComparisonMessage>(this, ReceivePreKeySelectionForComparison);
                    PublicNetwork.Unsubscribe<PreKeyComparisonResultMessage>(this, ReceivePreKeyComparisonResult);
                    PublicNetwork.Unsubscribe<PolarisationUsedMessage>(this, ReceivePolarisationUsed);
                }
            }

            _isDisposed = true;

            base.Dispose(disposing);
        }
    }
}