using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// The role last receiving on the quantum channel.
    /// </summary>
    public class Bob : ProtocolPartner<BobNotebookEntry>, IBob
    {
        /// <summary>
        /// The <see cref="IQuantumPipeReceiveEndpoint"/> over which Bob receives photons.
        /// </summary>
        private readonly IQuantumPipeReceiveEndpoint _quantumPipeReceiveEndpoint;

        private bool _isDisposed;
        private int _pendingPhotonsCount;

        public event EventHandler PendingPhotonsUpdated;

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

        /// <summary>
        /// Gets the cipher message received over the public channel.
        /// </summary>
        public BitArray Cipher { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bob"/> class.
        /// </summary>
        /// <param name="notebook">A list of <see cref="ProtocolRoleNotebookEntry"/> a protocol participant uses to keep track of information.</param>
        /// <param name="publicNetwork">The <see cref="IPublicNetwork"/> over which Bob sends and receives unencrypted messages.</param>
        /// <param name="pipeReceiveEndpoint">The <see cref="IQuantumPipeSendEndpoint"/> over which Bob receives photons.</param>
        /// <param name="randomGenerator">A <see cref="IRandomGenerator"/> which Bob uses to perform random operations.</param>
        public Bob(
            ObservableCollection<BobNotebookEntry> notebook,
            IPublicNetwork publicNetwork,
            IQuantumPipeReceiveEndpoint pipeReceiveEndpoint,
            IRandomGenerator randomGenerator)
            : base(notebook, publicNetwork, randomGenerator)
        {
            Role = ProtocolRoleType.Bob;
            _quantumPipeReceiveEndpoint = pipeReceiveEndpoint;
            Cipher = new BitArray(0);

            _quantumPipeReceiveEndpoint.PhotonReceived += PhotonReceivedHandler;
            PublicNetwork.Subscribe<CipherMessage>(this, CipherMessageHandler);
        }

        /// <inheritdoc/>
        public void NoteDownPolarisation(Polarisation polarisation)
        {
            BobNotebookEntry entry = MyNotebook.GetFirstInvalidEntryOrAddNew();
            entry.MyPolarisation = polarisation;
            MeasurePhotons();
        }

        /// <inheritdoc/>
        public void NoteDownRandomPolarisations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Polarisation polarisation =
                    RandomGenerator.GetRandomBool() ? Polarisation.Diagonal : Polarisation.Rectilinear;

                NoteDownPolarisation(polarisation);
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
        /// Measures as many pending photons as it can with the polarisations given in notebook entries of MyNotebook that don't contain a value in MyData.
        /// </summary>
        private void MeasurePhotons()
        {
            // Stop as soon as an entry without polarisation is found
            IEnumerable<BobNotebookEntry> entries = MyNotebook.TakeWhile(e => e.MyPolarisation.HasValue);

            foreach (BobNotebookEntry notebookEntry in entries)
            {
                if (notebookEntry.MyData == null)
                {
                    IPhoton photon = _quantumPipeReceiveEndpoint.DequeuePhoton();

                    if (photon == null)
                        break;

                    notebookEntry.MyData = photon.Measure(notebookEntry.MyPolarisation.Value);
                    notebookEntry.UpdatePreAndFinalKey();
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
                    PublicNetwork.Unsubscribe<CipherMessage>(this, CipherMessageHandler);
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }
    }
}