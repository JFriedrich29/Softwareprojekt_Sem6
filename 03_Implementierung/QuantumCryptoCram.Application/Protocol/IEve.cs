using System;
using System.Collections;
using System.Collections.Generic;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Interface for view-invoked methods specific to eve.
    /// </summary>
    public interface IEve : IProtocolRole<EveNotebookEntry>
    {
        /// <summary>
        /// Gets or sets the key edited by the eve role.
        /// </summary>
        List<bool?> EditedKey { get; set; } 

        /// <summary>
        /// Gets the cipher message received over the public channel.
        /// </summary>
        BitArray Cipher { get; }

        /// <summary>
        /// Gets or sets number of polarisations sent by Alice before actually sending the photons.
        /// </summary>
        int AliceLeakedPolarisationsCount { get; set; }

        /// <summary>
        /// Gets or sets number of polarisations sent by Bob before ve measured the photons.
        /// </summary>
        int BobLeakedPolarisationsCount { get; set; }

        /// <summary>
        /// Gets or sets number of polarisations sent by Alice before sending the photons that
        /// were used by Eve for measuring the corresponding photons.
        /// </summary>
        int ExploitedLeakedPolarisationsCount { get; set; }

        /// <summary>
        /// Add a polarisation to use for reading on the quantum channel to the notebook.
        /// </summary>
        /// <param name="polarisation">What polarisation should be noted.</param>
        /// <param name="entry">The entry to write into.</param>
        void NoteDownPolarisation(Polarisation polarisation, EveNotebookEntry entry = null);

        /// <summary>
        /// Adds a given amount of random polarisations that are used for measuring photons which are received over the quantum channel.
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