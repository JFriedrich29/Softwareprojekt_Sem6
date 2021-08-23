using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Quantum
{
    [TestFixture]
    public class PhotonGeneratorTests
    {
        private IRandomGenerator _randomGenerator;
        private IPhotonMeasurement _photonMeasurement;
        private PhotonGenerator _photonGenerator;

        [SetUp]
        public void Setup()
        {
            _randomGenerator = Substitute.For<IRandomGenerator>();
            _photonMeasurement = Substitute.For<IPhotonMeasurement>();
            _photonGenerator = new PhotonGenerator(_randomGenerator, _photonMeasurement);
        }

        [Test]
        public void GeneratePhoton_Should_Create_A_IPhoton()
        {
            // Act
            object photon = _photonGenerator.GeneratePhoton(Polarisation.Diagonal,DataBit.One);

            // Assert
            Assert. IsTrue(photon is IPhoton);
        }

        [Test]
        public void GeneratePhotons_ShouldCreateCorrectAmountOfPhotons()
        {
            // Act
            int expectedAmount = 10;
            List<IPhoton> photons = _photonGenerator.GenerateRandomPhotons(expectedAmount);

            // Assert
            photons.Count.Should().Be(expectedAmount);
        }
    }
}