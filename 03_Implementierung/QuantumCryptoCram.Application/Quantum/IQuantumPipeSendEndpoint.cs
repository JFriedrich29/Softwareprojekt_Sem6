using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Quantum
{
    /// <summary>
    /// An interface describing the sending endpoint of a quantum pipe.
    /// </summary>
    public interface IQuantumPipeSendEndpoint
    {
        /// <summary>
        /// Send a photon over a quantum pipe. The receive event of a the
        /// corresponding receiving role will be called either over the
        /// network or locally inside the application.
        /// </summary>
        /// <param name="photon">The photon to be sent.</param>
        void SendPhoton(IPhoton photon);
    }
}