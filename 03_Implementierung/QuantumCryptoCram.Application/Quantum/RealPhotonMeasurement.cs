using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Quantum
{
    /// <summary>
    /// A factory class for measuring photons.
    /// </summary>
    public class RealPhotonMeasurement : IPhotonMeasurement
    {
        private readonly IRandomGenerator _randomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealPhotonMeasurement"/> class.
        /// </summary>
        /// <param name="randomGenerator">A reference to an implementation of the <see cref="IRandomGenerator"/> interface.</param>
        public RealPhotonMeasurement(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        /// <inheritdoc/>
        public (DataBit, PhotonState) PerformMeasurement(in PhotonState currentState, Polarisation polarisation)
        {
            if (currentState.Polarisation == polarisation)
            {
                return (currentState.Data, currentState);
            }
            else
            {
                DataBit data = _randomGenerator.GetRandomBool() ? DataBit.One : DataBit.Zero;
                return (data,
                    new PhotonState(
                    data: data,
                    polarisation: polarisation));
            }
        }
    }
}