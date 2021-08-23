namespace QuantumCryptoCram.Domain.Quantum
{
    /// <summary>
    /// A class describing a photon. After initializing the photon, the internal state is hidden and can only be
    /// accessed using the measure operation.
    /// </summary>
    public class Photon : IPhoton
    {
        private readonly IPhotonMeasurement _photonMeasurement;
        private PhotonState _currentState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Photon"/> class.
        /// </summary>
        /// <param name="polarisation">The polarisation of the photon.</param>
        /// <param name="data">The data bit encoded by the photon.</param>
        /// <param name="photonMeasurement">An object for performing measurements on the photon.</param>
        public Photon(Polarisation polarisation, DataBit data, IPhotonMeasurement photonMeasurement)
        {
            _currentState = new PhotonState(data, polarisation);
            _photonMeasurement = photonMeasurement;
        }

        /// <inheritdoc/>
        public DataBit Measure(Polarisation polarisation)
        {
            DataBit data;
            (data, _currentState) = _photonMeasurement.PerformMeasurement(_currentState, polarisation);
            return data;
        }
    }
}