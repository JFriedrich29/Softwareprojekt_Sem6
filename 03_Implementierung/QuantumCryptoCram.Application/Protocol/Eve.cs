using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// The role intercepting on the quantum channel.
    /// </summary>
    public class Eve : ProtocolRole<EveNotebookEntry>, IEve
    {
        /// <summary>
        /// The <see cref="IQuantumPipeReceiveEndpoint"/> over which Eve receives photons.
        /// </summary>
        private readonly IQuantumPipeReceiveEndpoint _quantumPipeReceiveEndpoint;

        /// <summary>
        /// The <see cref="IQuantumPipeSendEndpoint"/> over which Eve sends photons.
        /// </summary>
        private readonly IQuantumPipeSendEndpoint _quantumPipeSendEndpoint;

        private bool _isDisposed;
        private int _pendingPhotonsCount;

        /// <summary>
        /// Gets or sets the number of pending Photons on the QuantumPipe.
        /// </summary>
        /// <value>
        /// The number of pending Photons on the QuantumPipe.
        /// </value>
        public int PendingPhotonsCount
        {
            get => _quantumPipeReceiveEndpoint.PendingPhotonsCount;

            set
            {
                _pendingPhotonsCount = value;
                PendingPhotonsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler PendingPhotonsUpdated;

        /// <inheritdoc/>
        public int ExploitedLeakedPolarisationsCount { get; set; }

        /// <summary>
        /// Gets cipher message received over the public channel.
        /// </summary>
        public BitArray Cipher { get; private set; }

        /// <inheritdoc/>
        public List<bool?> EditedKey { get; set; }

        /// <inheritdoc/>
        public int AliceLeakedPolarisationsCount { get; set; }

        /// <inheritdoc/>
        public int BobLeakedPolarisationsCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Eve"/> class.
        /// </summary>
        /// <param name="notebook">A list of <see cref="EveNotebookEntry"/> which Eve uses to store information.</param>
        /// <param name="encryptionNotebook">The <see cref="EncryptionTestEntry"/> Eve uses to keep track of encryption information for testing the final key.</param>
        /// <param name="publicNetwork">The <see cref="IPublicNetwork"/> over which Eve sends and receives unencrypted messages.</param>
        /// <param name="pipeReceiveEndpoint">The <see cref="IQuantumPipeReceiveEndpoint"/> over which Eve receives photons.</param>
        /// <param name="pipeSendEndpoint">The <see cref="IQuantumPipeSendEndpoint"/> over which Eve sends photons.</param>
        /// <param name="randomGenerator">A <see cref="IRandomGenerator"/> which Eve uses to perform random operations.</param>
        /// <param name="encryptionService">A <see cref="IEncryptionService"/> which Eve uses to decrypt the messages send on the public channel by Alice.</param>
        public Eve(
            ObservableCollection<EveNotebookEntry> notebook,
            IPublicNetwork publicNetwork,
            IQuantumPipeReceiveEndpoint pipeReceiveEndpoint,
            IQuantumPipeSendEndpoint pipeSendEndpoint,
            IRandomGenerator randomGenerator)
            : base(notebook, publicNetwork, randomGenerator)
        {
            Role = ProtocolRoleType.Eve;

            _quantumPipeReceiveEndpoint = pipeReceiveEndpoint;
            _quantumPipeSendEndpoint = pipeSendEndpoint;
            _quantumPipeReceiveEndpoint.PhotonReceived += PhotonReceivedHandler;
            Cipher = new BitArray(0);
            EditedKey = new List<bool?>();
            PublicNetwork.Subscribe<PolarisationUsedMessage>(this, ReceivePolarisationUsed);
            PublicNetwork.Subscribe<PolarisationMatchesMessage>(this, ReceivePolarisationMatches);
            PublicNetwork.Subscribe<PreKeySelectionForComparisonMessage>(this, ReceivePreKeySelectionForComparison);
            PublicNetwork.Subscribe<CipherMessage>(this, CipherMessageHandler);
        }

        /// <inheritdoc/>
        public void NoteDownPolarisation(Polarisation polarisation, EveNotebookEntry entry = null)
        {
            if (entry == null)
            {
                entry = MyNotebook.GetFirstInvalidEntryOrAddNew();
            }

            entry.MyPolarisation = polarisation;
            MeasurePhotons();
        }

        /// <inheritdoc/>
        public void NoteDownRandomPolarisations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Polarisation polarisation = RandomGenerator.GetRandomBool() ? Polarisation.Diagonal : Polarisation.Rectilinear;

                NoteDownPolarisation(polarisation);
            }
        }

        /// <summary>
        /// Handle the polarisation used message and note down which polarisations Alice or Bob
        /// shared over the public channel.
        /// </summary>
        /// <param name="message">The public message containing the polarisations.</param>
        protected void ReceivePolarisationUsed(PolarisationUsedMessage message)
        {
            foreach (KeyValuePair<int, Polarisation> item in message.Polarisations)
            {
                EveNotebookEntry eveNotebookEntry = MyNotebook.SafelyGetEntry(item.Key);
                if (message.PolarisationPublisher == ProtocolRoleType.Alice)
                {
                    eveNotebookEntry.PolarisationAlice = item.Value;
                }
                else
                {
                    eveNotebookEntry.PolarisationBob = item.Value;
                }

                UpdateMeasuredDataKeyRelevance(eveNotebookEntry);
            }
        }

        /// <summary>
        /// Handle the polarisation matches message and note down which polarisations matched between Alice, Bob an Eve.
        /// </summary>
        /// <param name="message">The public message containing the polarisation matches.</param>
        protected void ReceivePolarisationMatches(PolarisationMatchesMessage message)
        {
            if (message.PolarisationPublisher == ProtocolRoleType.Alice)
            {
                foreach (EveNotebookEntry entry in MyNotebook)
                {
                    if (entry.PolarisationBob.HasValue)
                    {
                        if (message.Matches.Contains(entry.Id))
                            entry.PolarisationAlice = entry.PolarisationBob;
                        else
                            entry.PolarisationAlice = entry.PolarisationBob == Polarisation.Diagonal ? Polarisation.Rectilinear : Polarisation.Diagonal;
                    }

                    UpdateMeasuredDataKeyRelevance(entry);
                }
            }
            else
            {
                foreach (EveNotebookEntry entry in MyNotebook)
                {
                    if (entry.PolarisationAlice.HasValue)
                    {
                        if (message.Matches.Contains(entry.Id))
                            entry.PolarisationBob = entry.PolarisationAlice;
                        else
                            entry.PolarisationBob = entry.PolarisationAlice == Polarisation.Diagonal ? Polarisation.Rectilinear : Polarisation.Diagonal;
                    }

                    UpdateMeasuredDataKeyRelevance(entry);
                }
            }
        }

        /// <summary>
        /// Handle the prekey comparison message and mark all contained entries as discarded for final key creation.
        /// </summary>
        /// <param name="message">The public message containing the prekey databits.</param>
        protected void ReceivePreKeySelectionForComparison(PreKeySelectionForComparisonMessage message)
        {
            foreach (KeyValuePair<int, DataBit> selectedPreKeyBit in message.SelectedPreKeyBits)
            {
                MyNotebook.SafelyGetEntry(selectedPreKeyBit.Key).RelevanceType = MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded;
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

                if (_quantumPipeReceiveEndpoint != null)
                {
                    _quantumPipeReceiveEndpoint.PhotonReceived -= PhotonReceivedHandler;
                }

                if (PublicNetwork != null)
                {
                    PublicNetwork.Unsubscribe<PolarisationMatchesMessage>(this, ReceivePolarisationMatches);
                    PublicNetwork.Unsubscribe<PreKeySelectionForComparisonMessage>(this, ReceivePreKeySelectionForComparison);
                    PublicNetwork.Unsubscribe<PolarisationUsedMessage>(this, ReceivePolarisationUsed);
                    PublicNetwork.Unsubscribe<CipherMessage>(this, CipherMessageHandler);
                }
            }

            _isDisposed = true;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Marks polarisation matches between alice and bob and own polarisations with a <see cref="MeasuredDataKeyRelevanceType"/>.
        /// </summary>
        /// <param name="entry">The entry that gets checked.</param>
        private void UpdateMeasuredDataKeyRelevance(EveNotebookEntry entry)
        {
            if (!entry.PolarisationAlice.HasValue ||
                !entry.PolarisationBob.HasValue ||
                !entry.MyData.HasValue ||
                entry.RelevanceType == MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded)
                return;

            if (entry.PolarisationBob == entry.PolarisationAlice)
            {
                entry.RelevanceType = MeasuredDataKeyRelevanceType.AliceBobMatch;

                if (entry.PolarisationAlice == entry.MyPolarisation)
                    entry.RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch;
            }
            else
            {
                entry.RelevanceType = MeasuredDataKeyRelevanceType.NoMatch;
            }
        }

        /// <summary>
        /// Handle reception of a photon.
        /// </summary>
        private void PhotonReceivedHandler(object sender, EventArgs args)
        {
            MeasurePhotons();
        }

        /// <summary>
        /// Measures each incoming photon with polarisations written down to the notebook.
        /// </summary>
        private void MeasurePhotons()
        {
            // Stop as soon as an entry without polarisation is found
            IEnumerable<EveNotebookEntry> entries = MyNotebook.TakeWhile(e => e.MyPolarisation.HasValue);

            foreach (EveNotebookEntry notebookEntry in entries)
            {
                if (notebookEntry.MyData == null)
                {
                    IPhoton photon = _quantumPipeReceiveEndpoint.DequeuePhoton();
                    if (photon == null)
                        break;

                    // If we know the polarisation from alice before actually measuring, alice
                    // leaked her polarisation.
                    if (notebookEntry.PolarisationAlice.HasValue)
                    {
                        AliceLeakedPolarisationsCount++;
                    }

                    // If we know the polarisation from alice before actually measuring, bob
                    // leaked his polarisation.
                    if (notebookEntry.PolarisationBob.HasValue)
                    {
                        BobLeakedPolarisationsCount++;
                    }

                    if ((notebookEntry.PolarisationAlice.HasValue && notebookEntry.PolarisationAlice == notebookEntry.MyPolarisation.Value)
                        || (notebookEntry.PolarisationBob.HasValue && notebookEntry.PolarisationBob == notebookEntry.MyPolarisation.Value))
                    {
                        ExploitedLeakedPolarisationsCount++;
                    }

                    notebookEntry.MyData = photon.Measure(notebookEntry.MyPolarisation.Value);
                    UpdateMeasuredDataKeyRelevance(notebookEntry);
                    // Send the photon to bob.
                    _quantumPipeSendEndpoint.SendPhoton(photon);
                }
            }

            PendingPhotonsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handle the cipher message.
        /// </summary>
        /// <param name="message">The cipher message.</param>
        private void CipherMessageHandler(CipherMessage message)
        {
            Cipher = (BitArray)message.Cipher.Clone();
        }
    }
}