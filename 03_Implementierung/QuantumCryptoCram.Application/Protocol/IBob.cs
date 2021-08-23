using System;
using System.Collections;
using System.Collections.ObjectModel;

using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Interface for view-invoked methods specific to alice.
    /// </summary>
    public interface IBob : IProtocolPartner<BobNotebookEntry>
    {
        /// <summary>
        /// Gets the cipher message received over the public channel.
        /// </summary>
        BitArray Cipher
        {
            get;
        }

        /// <summary>
        /// Add a polarisation to use for reading on the quantum channel to the notebook.
        /// </summary>
        /// <param name="polarisation">What polarisation should be noted.</param>
        void NoteDownPolarisation(Polarisation polarisation);

        /// <summary>
        /// Add a given amount of random polarisations to use for reading on the quantum channel to the notebook.
        /// </summary>
        /// <param name="count">How many random photon configurations should be noted.</param>
        void NoteDownRandomPolarisations(int count);

        /// <summary>
        /// The update event is invoked whenever a Photon is enqueue or dequeue on the quantum pipe.
        /// </summary>
        event EventHandler PendingPhotonsUpdated;

        /// <summary>
        /// Returns the number of Photons pending on the QuantumPipe.
        /// </summary>
        int PendingPhotonsCount { get; }
    }
}