using System.Collections.ObjectModel;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol

{
    public static class NoteDownMeasuredPhotonTest
    {

        [TestFixture]
        private class PhotonMeasurementTestsWithoutCloning
        {
            private IRandomGenerator _randomGenerator;
            private RealPhotonMeasurement _realPhotonMeasurement;
            private Bob _bob;
            private Eve _eve;
            private IPublicNetwork _publicNetwork;
            private PhotonGenerator _photonGenerator;
            private LocalQuantumPipe _quantumPipeAe;
            private LocalQuantumPipe _quantumPipeEb;
            private ObservableCollection<BobNotebookEntry> _bobNotebookEntries;
            private ObservableCollection<EveNotebookEntry> _eveNotebookEntries;

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

                _photonGenerator = new PhotonGenerator(_randomGenerator, _realPhotonMeasurement);
                _publicNetwork = new PublicNetwork();

                _quantumPipeAe = new LocalQuantumPipe();
                _quantumPipeEb = new LocalQuantumPipe();

                _bobNotebookEntries = new ObservableCollection<BobNotebookEntry>();
                _eveNotebookEntries = new ObservableCollection<EveNotebookEntry>();

                _eve = new Eve(_eveNotebookEntries, _publicNetwork, _quantumPipeAe, _quantumPipeEb, _randomGenerator);
                _bob = new Bob(_bobNotebookEntries, _publicNetwork, _quantumPipeEb, _randomGenerator);
            }

            [Test]
            public void MeasurePhotons_WithCorrectPolarisationShouldNotAlterDatabit()
            {
                // ### Arrange
                IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(Polarisation.Diagonal, DataBit.Zero);
                _quantumPipeAe.SendPhoton(photonToBeSent);

                // ### Act
                _eve.NoteDownPolarisation(Polarisation.Diagonal);
                _bob.NoteDownPolarisation(Polarisation.Diagonal);

                // ### Assert
                _eveNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
                _bobNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
            }

            [Test]
            public void MeasurePhotons_WithIncorrectPolarisationShouldAlterDatabit()
            {
                // ### Arrange
                IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(Polarisation.Diagonal, DataBit.Zero);
                _quantumPipeAe.SendPhoton(photonToBeSent);

                // ### Act
                _eve.NoteDownPolarisation(Polarisation.Rectilinear);
                _bob.NoteDownPolarisation(Polarisation.Diagonal);

                // ### Assert
                _eveNotebookEntries[0].MyData.Should().Be(DataBit.One);
                _bobNotebookEntries[0].MyData.Should().Be(DataBit.One);
            }

            [Test]
            public void MeasurePhotons_ShouldMeasure_WhenPolarisationAreAlreadyNoteDown_AfterPhotonIsSent()
            {
                // ### Arrange
                IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(Polarisation.Diagonal, DataBit.Zero);

                // ### Act
                _eve.NoteDownPolarisation(Polarisation.Rectilinear);
                _bob.NoteDownPolarisation(Polarisation.Diagonal);
                _quantumPipeAe.SendPhoton(photonToBeSent);

                // ### Assert
                _eveNotebookEntries[0].MyData.Should().NotBeNull();
                _bobNotebookEntries[0].MyData.Should().NotBeNull();
            }
        }

        [TestFixture]
        private class PhotonMeasurementTestsWithCloning
        {
            private IRandomGenerator _randomGenerator;
            private PhotonMeasurementWithCloning _realPhotonMeasurement;
            private Bob _bob;
            private Eve _eve;
            private IPublicNetwork _publicNetwork;
            private PhotonGenerator _photonGenerator;
            private LocalQuantumPipe _quantumPipeAe;
            private LocalQuantumPipe _quantumPipeEb;
            private ObservableCollection<BobNotebookEntry> _bobNotebookEntries;
            private ObservableCollection<EveNotebookEntry> _eveNotebookEntries;

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

                _photonGenerator = new PhotonGenerator(_randomGenerator, _realPhotonMeasurement);
                _publicNetwork = new PublicNetwork();

                _quantumPipeAe = new LocalQuantumPipe();
                _quantumPipeEb = new LocalQuantumPipe();

                _bobNotebookEntries = new ObservableCollection<BobNotebookEntry>();
                _eveNotebookEntries = new ObservableCollection<EveNotebookEntry>();

                _eve = new Eve(_eveNotebookEntries, _publicNetwork, _quantumPipeAe, _quantumPipeEb, _randomGenerator);
                _bob = new Bob(_bobNotebookEntries, _publicNetwork, _quantumPipeEb, _randomGenerator);
            }

            [Test]
            public void MeasurePhotons_WithCorrectPolarisationShouldNotAlterDatabit()
            {
                // ### Arrange
                IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(Polarisation.Diagonal, DataBit.Zero);
                _quantumPipeAe.SendPhoton(photonToBeSent);

                // ### Act
                _eve.NoteDownPolarisation(Polarisation.Diagonal);
                _bob.NoteDownPolarisation(Polarisation.Diagonal);

                // ### Assert
                _eveNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
                _bobNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
            }

            [Test]
            public void MeasurePhotons_WithIncorrectPolarisationShouldNotAlterDatabit()
            {
                // ### Arrange
                IPhoton photonToBeSent = _photonGenerator.GeneratePhoton(Polarisation.Diagonal, DataBit.Zero);
                _quantumPipeAe.SendPhoton(photonToBeSent);

                // ### Act
                _eve.NoteDownPolarisation(Polarisation.Rectilinear);
                _bob.NoteDownPolarisation(Polarisation.Diagonal);

                // ### Assert
                _eveNotebookEntries[0].MyData.Should().Be(DataBit.One);
                _bobNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
            }
        }
    }
}