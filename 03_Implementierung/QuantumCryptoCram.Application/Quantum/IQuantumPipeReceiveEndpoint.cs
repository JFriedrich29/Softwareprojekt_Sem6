using System;

using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Quantum
{
    /// <summary>
    /// An interface describing the sending endpoint of a quantum pipe.
    /// </summary>
    public interface IQuantumPipeReceiveEndpoint
    {
        /// <summary>
        /// The receive event is invoked whenever a Photon is sent on the quantum pipe.
        /// </summary>
        event EventHandler PhotonReceived;

        /// <summary>
        /// The number of photons pending on quantumPipe.
        /// </summary>
        int PendingPhotonsCount { get; }

        /// <summary>
        /// Dequeue a photon from the photon buffer.
        /// </summary>
        /// <returns>The dequeued photon or null if no photon is buffered.</returns>
        IPhoton DequeuePhoton();
    }
}