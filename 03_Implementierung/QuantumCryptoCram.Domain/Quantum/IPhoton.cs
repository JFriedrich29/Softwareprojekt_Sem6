namespace QuantumCryptoCram.Domain.Quantum
{
    /// <summary>
    /// An interface for describing a photon.
    /// </summary>
    public interface IPhoton
    {
        /// <summary>
        /// Perform a measurement on the photon. When the caller does not know the polarisation of this photon, the
        /// measurement might yield a wrong result and will alter the state of the photon.
        /// </summary>
        /// <param name="polarisation">The polarisation for the measurement.</param>
        /// <returns>The data bit measured.</returns>
        DataBit Measure(Polarisation polarisation);
    }
}