namespace QuantumCryptoCram.Domain.Quantum
{
    /// <summary>
    /// An inteface for performing measurements on a photon.
    /// </summary>
    public interface IPhotonMeasurement
    {
        /// <summary>
        /// Perform a measurement on a photon. When choosing the wrong polarisation for measurement the returned state
        /// could be an altered version of the current state.
        /// </summary>
        /// <param name="currentState">The current state of the photon.</param>
        /// <param name="polarisation">The polarisation for the measurement.</param>
        /// <returns>A possibly altered state of the photon.</returns>
        (DataBit, PhotonState) PerformMeasurement(in PhotonState currentState, Polarisation polarisation);
    }
}