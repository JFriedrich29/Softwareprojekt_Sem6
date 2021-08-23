using FluentAssertions;
using NUnit.Framework;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    [TestFixture]
    class LeakedPolarisationsTest : ProtocolTestBase
    {

        [Test]
        public void LeakedPolarisationsTest_AliceLeakedPolarisationsCount()
        {
            // ### Arrange
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Act
            _alice.SendPolarisations();
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));

            _eve.NoteDownPolarisation(Polarisation.Diagonal);
            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert
            _eve.AliceLeakedPolarisationsCount.Should().Be(2);
            _eve.BobLeakedPolarisationsCount.Should().Be(0);
        }

        [Test]
        public void LeakedPolarisationsTest_LeakedPolarisationsCount()
        {
            // ### Arrange
            _bob.NoteDownPolarisation(Polarisation.Rectilinear);
            _bob.NoteDownPolarisation(Polarisation.Rectilinear);

            // ### Act
            _bob.SendPolarisations();
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));

            _eve.NoteDownPolarisation(Polarisation.Diagonal);
            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert
            _eve.AliceLeakedPolarisationsCount.Should().Be(0);
            _eve.BobLeakedPolarisationsCount.Should().Be(2);
        }
        
        [Test]
        public void LeakedPolarisationsTest_ExploitedLeakedPolarisationsCount()
        {
            // ### Arrange
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Act
            _alice.SendPolarisations();
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));

            _eve.NoteDownPolarisation(Polarisation.Rectilinear);
            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert
            _eve.ExploitedLeakedPolarisationsCount.Should().Be(1);
        }

        [Test]
        public void LeakedPolarisationsTest_AliceSendsBeforeEveMeasures()
        {
            // ### Arrange
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Act
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            _alice.SendPolarisations();

            _eve.NoteDownPolarisation(Polarisation.Rectilinear);
            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert
            _eve.AliceLeakedPolarisationsCount.Should().Be(2);
            _eve.ExploitedLeakedPolarisationsCount.Should().Be(1);
        }

        [Test]
        public void LeakedPolarisationsTest_NoLeaksNoCount()
        {
            // ### Arrange
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Act
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            QuantumPipeAe.SendPhoton(new Photon(Polarisation.Rectilinear, DataBit.One, _photonMeasurement));
            _eve.NoteDownPolarisation(Polarisation.Rectilinear);
            _eve.NoteDownPolarisation(Polarisation.Diagonal);
            _alice.SendPolarisations();

            // ### Assert
            _eve.AliceLeakedPolarisationsCount.Should().Be(0);
            _eve.ExploitedLeakedPolarisationsCount.Should().Be(0);
        }
    }
}
