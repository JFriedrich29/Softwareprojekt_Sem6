using System.Collections;

using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Interface for view-invoked methods specific to alice.
    /// </summary>
    public interface IAlice : IProtocolPartner<AliceNotebookEntry>
    {
        /// <summary>
        /// Gets or sets the last plain text that was encrypted.
        /// </summary>
        string PlainText { get; set; }

        /// <summary>
        /// Adds a new photon to the notebook.
        /// </summary>
        /// <param name="dataBit">What databit should be noted.</param>
        /// <param name="polarisation">What polarisation should be noted.</param>
        void NoteDownPhoton(DataBit dataBit, Polarisation polarisation);

        /// <summary>
        /// Randomly creates a given amount of photons.
        /// </summary>
        /// <param name="count">How many random photon configurations should be noted.</param>
        void NoteDownRandomPhotons(int count);

        /// <summary>
        /// Send all the photons generated so far.
        /// </summary>
        void SendPhotons();

        /// <summary>
        /// Send the cipher message over the public channel.
        /// </summary>
        /// <param name="cipher">The cipher to be sent.</param>
        void SendCipherMessage(BitArray cipher);
    }
}