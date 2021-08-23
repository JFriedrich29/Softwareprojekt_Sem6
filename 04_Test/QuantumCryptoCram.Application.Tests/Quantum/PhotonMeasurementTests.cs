using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Quantum

{
    public static class PhotonMeasurementTests
    {
        private const Polarisation _diagonalPolarisation = Polarisation.Diagonal;
        private const Polarisation _rectilinearPolarisation = Polarisation.Rectilinear;
        private static readonly PhotonState _zeroDiagonalState = new PhotonState(DataBit.Zero, Polarisation.Diagonal);
        private static readonly PhotonState _oneDiagonalState = new PhotonState(DataBit.One, Polarisation.Diagonal);
        private static readonly PhotonState _zeroRectilinearState = new PhotonState(DataBit.Zero, Polarisation.Rectilinear);
        private static readonly PhotonState _oneRectilinearState = new PhotonState(DataBit.One, Polarisation.Rectilinear);

        [TestFixture]
        private class PhotonMeasurementTestsWithoutCloning
        {
            private IRandomGenerator _randomGenerator;
            private RealPhotonMeasurement _realPhotonMeasurement;

            [SetUp]
            public void Setup()
            {
                // ### Common Arrange
                _randomGenerator = Substitute.For<IRandomGenerator>();
                // Remove randomness by faking (mocking) the return value to be fixed.
                // For this reason we decoupled the random function into a own class with an own interface, that can be mocked easily.
                // This technique is often useful, if you want to fake .NET framework stuff like System.Random or System.DateTime.
                _randomGenerator.GetRandomBool().Returns(true);

                // Create an instance of the system under test (SUT) with fake services and test data.
                _realPhotonMeasurement = new RealPhotonMeasurement(_randomGenerator);
            }

            [Test]
            public void PerformMeasurement_ShouldAffectPolarisationCorrectly()
            {
                // ### Arrange

                // ### Act and Assert    - We combine the two blocks here since it is easier to understand here if we act and immediately assert

                // Measuring with same polarisation should not affect the polarisation
                _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _diagonalPolarisation)
                    .Item2.Polarisation.Should().Be(_diagonalPolarisation);

                // Measuring again with a different polarisation should affect the polarisation
                _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _rectilinearPolarisation)
                    .Item2.Polarisation.Should().Be(_rectilinearPolarisation);

                // Measuring again with the original polarisation should affect the polarisation
                _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _diagonalPolarisation)
                    .Item2.Polarisation.Should().Be(_diagonalPolarisation);
            }

            [Test]
            public void PerformMeasurement_ShouldReturnUnchangedDataBit_WhenMeasuredWithCorrectPolarisation()
            {
                // ### Arrange

                // ### Act and Assert    - We combine the two blocks here since it is easier to understand here if we act and immediately assert

                // Since we eliminated the randomness both measurements with the CORRECT polarisation MUST NOT change the data bit.

                (DataBit data, PhotonState newState) = _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _diagonalPolarisation);
                Assert.AreEqual(expected: DataBit.Zero, actual: newState.Data);

                (data, newState) = _realPhotonMeasurement.PerformMeasurement(_oneDiagonalState, _diagonalPolarisation);
                Assert.AreEqual(expected: DataBit.One, actual: newState.Data);

                // The same two assertions can be written much more cleanly with the Should() method of the package "Fluent Assertions"
                // that works for many different types and values.
                // In this way you also never can confuse the expected and actual parameter
                // what happens quite often, if you dont write the parameter names out as above.

                _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _diagonalPolarisation)
                    .Item2.Data.Should().Be(DataBit.Zero);

                _realPhotonMeasurement.PerformMeasurement(_oneDiagonalState, _diagonalPolarisation)
                    .Item2.Data.Should().Be(DataBit.One);
            }

            [Test]
            public void PerformMeasurement_ShouldReturnChangedDataBit_WhenMeasuredWithIncorrectPolarisation()
            {
                // ### Arrange

                // ### Act

                // Since we eliminated the randomness when measured with the INCORRECT polarisation, exactly ONE MEASUREMENT MUST FAIL.

                int failedMeasurements = 0;
                (DataBit data, PhotonState newState) = _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _rectilinearPolarisation);
                if (DataBit.Zero != newState.Data)
                    failedMeasurements++;

                (data, newState) = _realPhotonMeasurement.PerformMeasurement(_oneDiagonalState, _rectilinearPolarisation);
                if (DataBit.One != newState.Data)
                    failedMeasurements++;

                // ### Assert
                failedMeasurements.Should().Be(1);
            }
        }

        [TestFixture]
        private class PhotonMeasurementTestsWithCloning
        {
            private IRandomGenerator _randomGenerator;
            private PhotonMeasurementWithCloning _realPhotonMeasurement;

            [SetUp]
            public void Setup()
            {
                // ### Common Arrange
                _randomGenerator = Substitute.For<IRandomGenerator>();
                // Remove randomness by faking (mocking) the return value to be fixed.
                // For this reason we decoupled the random function into a own class with an own interface, that can be mocked easily.
                // This technique is often useful, if you want to fake .NET framework stuff like System.Random or System.DateTime.
                _randomGenerator.GetRandomBool().Returns(true);

                // Create an instance of the system under test (SUT) with fake services and test data.
                _realPhotonMeasurement = new PhotonMeasurementWithCloning(_randomGenerator);
            }

            [Test]
            public void PerformMeasurement_ShouldReturnUnaffectedPolarisation_WhenMeasuredWithIncorrectPolarisation()
            {
                // ### Act and Assert

                // Since we eliminated the randomness and cloning is active, both measurements must not affect the polarisation.
                _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _rectilinearPolarisation)
                    .Item2.Polarisation.Should().Be(_diagonalPolarisation);

                _realPhotonMeasurement.PerformMeasurement(_oneDiagonalState, _rectilinearPolarisation)
                    .Item2.Polarisation.Should().Be(_diagonalPolarisation);
            }

            [Test]
            public void PerformMeasurement_ShouldReturnChangedDataBit_WhenMeasuredWithIncorrectPolarisation()
            {
                // ### Arrange

                // ### Act

                // Since we eliminated the randomness when measured with the INCORRECT polarisation, exactly ONE MEASUREMENT MUST FAIL.

                int failedMeasurements = 0;
                (DataBit data, PhotonState newState) = _realPhotonMeasurement.PerformMeasurement(_zeroDiagonalState, _rectilinearPolarisation);
                if (DataBit.Zero != data)
                    failedMeasurements++;

                (data, newState) = _realPhotonMeasurement.PerformMeasurement(_oneDiagonalState, _rectilinearPolarisation);
                if (DataBit.One != data)
                    failedMeasurements++;

                // ### Assert
                failedMeasurements.Should().Be(1);
            }
        }
    }
}