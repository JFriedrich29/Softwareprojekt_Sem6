using System.Collections.Generic;

using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Quantum;

using static System.Linq.Enumerable;

namespace QuantumCryptoCram.Application.Quantum
{
    /// <summary>
    /// A factory class for creating photons.
    /// </summary>
    public class PhotonGenerator
    {
        private readonly IRandomGenerator _randomGenerator;
        private readonly IPhotonMeasurement _photonMeasurement;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotonGenerator"/> class.
        /// </summary>
        /// <param name="randomGenerator">A reference to an implementation of the <see cref="IRandomGenerator"/> interface.</param>
        /// <param name="photonMeasurement">A reference to an implementation of the <see cref="IPhotonMeasurement"/> interface.</param>
        public PhotonGenerator(IRandomGenerator randomGenerator, IPhotonMeasurement photonMeasurement)
        {
            _randomGenerator = randomGenerator;
            _photonMeasurement = photonMeasurement;
        }

        /// <summary>
        /// Creates a photon with the given polarisation and data.
        /// </summary>
        /// <param name="polarisation">The desired polarisation of the generated photon.</param>
        /// <param name="data">The data the photon should have.</param>
        /// <returns>A Photon with the given polarisation and data.</returns>
        public IPhoton GeneratePhoton(Polarisation polarisation, DataBit data)
        {
            var photon = new Photon(polarisation, data, _photonMeasurement);
            return photon;
        }

        /// <summary>
        /// Creates a list of photons with random polarisation and data.
        /// </summary>
        /// <param name="count">The amount of random photons to be generated.</param>
        /// <returns>A list of photons with random polarisation and data.</returns>
        public List<IPhoton> GenerateRandomPhotons(int count)
        {
            var photonList = new List<IPhoton>();

            foreach (int i in Range(0, count))
            {
                Polarisation polarisation = _randomGenerator.GetRandomBool() ? Polarisation.Diagonal : Polarisation.Rectilinear;

                DataBit data = _randomGenerator.GetRandomBool() ? DataBit.One : DataBit.Zero;

                IPhoton photon = GeneratePhoton(polarisation, data);

                photonList.Add(photon);
            }

            return photonList;
        }
    }
}