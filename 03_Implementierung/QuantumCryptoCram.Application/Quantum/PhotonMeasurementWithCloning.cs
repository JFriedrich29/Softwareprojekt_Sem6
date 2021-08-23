using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Quantum
{
    public class PhotonMeasurementWithCloning : IPhotonMeasurement
    {
        private readonly IRandomGenerator _randomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotonMeasurementWithCloning"/> class.
        /// </summary>
        /// <param name="randomGenerator">A reference to an implementation of the <see cref="IRandomGenerator"/> interface.</param>
        /// <param name="simulationOptions">A reference to an implementation of the <see cref="SimulationOptions"/> interface.</param>
        public PhotonMeasurementWithCloning(IRandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        /// <inheritdoc/>
        public (DataBit, PhotonState) PerformMeasurement(in PhotonState currentState, Polarisation polarisation)
        {
            // Cloing a photon does not affect the current state

            if (currentState.Polarisation == polarisation)
            {
                return (currentState.Data, currentState);
            }
            else
            {
                return (_randomGenerator.GetRandomBool() ? DataBit.One : DataBit.Zero, currentState);
            }
        }
    }
}