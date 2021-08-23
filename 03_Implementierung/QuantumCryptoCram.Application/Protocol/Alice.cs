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
    /// The role first sending on the quantum channel.
    /// </summary>
    public class Alice : ProtocolPartner<AliceNotebookEntry>, IAlice
    {
        /// <summary>
        /// The <see cref="PhotonGenerator"/> Alice uses to make photons.
        /// </summary>
        private readonly PhotonGenerator _photonGenerator;

        /// <summary>
        /// The <see cref="IQuantumPipeSendEndpoint"/> over which Alice sends photons.
        /// </summary>
        private readonly IQuantumPipeSendEndpoint _quantumPipeSendEndpoint;

        /// <inheritdoc/>
        public string PlainText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Alice"/> class.
        /// </summary>
        /// <param name="notebook">A list of <see cref="ProtocolRoleNotebookEntry"/> a protocol participant uses to keep track of information.</param>
        /// <param name="photonGenerator">The <see cref="PhotonGenerator"/> Alice uses to make photons.</param>
        /// <param name="publicNetwork">The <see cref="IPublicNetwork"/> over which Alice sends and receives unencrypted messages.</param>
        /// <param name="pipeSendEndpoint">The <see cref="IQuantumPipeSendEndpoint"/> over which Alice sends photons.</param>
        /// <param name="randomGenerator">A <see cref="IRandomGenerator"/> which Alice uses to perform random operations.</param>
        public Alice(
            ObservableCollection<AliceNotebookEntry> notebook,
            IPublicNetwork publicNetwork,
            IQuantumPipeSendEndpoint pipeSendEndpoint,
            IRandomGenerator randomGenerator,
            PhotonGenerator photonGenerator)
            : base(notebook, publicNetwork, randomGenerator)
        {
            _photonGenerator = photonGenerator;
            _quantumPipeSendEndpoint = pipeSendEndpoint;
            Role = ProtocolRoleType.Alice;

            PlainText = string.Empty;
        }

        /// <inheritdoc/>
        public void NoteDownPhoton(DataBit dataBit, Polarisation polarisation)
        {
            AliceNotebookEntry entry = MyNotebook.GetFirstInvalidEntryOrAddNew();
            entry.MyData = dataBit;
            entry.MyPolarisation = polarisation;
        }

        /// <inheritdoc/>
        public void NoteDownRandomPhotons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Polarisation polarisation = RandomGenerator.GetRandomBool() ? Polarisation.Diagonal : Polarisation.Rectilinear;

                DataBit dataBit = RandomGenerator.GetRandomBool() ? DataBit.One : DataBit.Zero;

                NoteDownPhoton(dataBit, polarisation);
            }
        }

        /// <inheritdoc/>
        public void SendPhotons()
        {
            IEnumerable<AliceNotebookEntry> entriesToSend = MyNotebook.Where(entry =>
                !entry.WasPhotonSent &&
                entry.MyData.HasValue &&
                entry.MyPolarisation.HasValue);

            foreach (AliceNotebookEntry entry in entriesToSend)
            {
                if (entry.WasPhotonSent == false && entry.IsValidEntry())
                {
                    IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(entry.MyPolarisation.Value, entry.MyData.Value);
                    entry.WasPhotonSent = true;
                    _quantumPipeSendEndpoint.SendPhoton(photonToBeSent);
                }
            }
        }

        /// <inheritdoc/>
        public void SendCipherMessage(BitArray cipher)
        {
            PublicNetwork.PublishMessage(this, new CipherMessage(cipher));
        }
    }
}