using System.Collections.Generic;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    /// <summary>
    /// Test cases for the use case UC-14.
    /// </summary>
    [TestFixture]
    public class SendPolarisationTest : ProtocolTestBase
    {
        [Test]
        public void SendPolarisation_EveAndBobShouldReceivePolarisationsFromAlice()
        {
            // ### Arrange
            const Polarisation firstPolarisation = Polarisation.Diagonal;
            const Polarisation secondPolarisation = Polarisation.Rectilinear;

            _alice.NoteDownPhoton(DataBit.One, firstPolarisation);
            _alice.NoteDownPhoton(DataBit.Zero, secondPolarisation);
            _bobNotebookEntries.Add(new BobNotebookEntry(0));
            _bobNotebookEntries.Add(new BobNotebookEntry(1));
            _eveNotebookEntries.Add(new EveNotebookEntry(0));
            _eveNotebookEntries.Add(new EveNotebookEntry(1));

            // ### Act
            _alice.SendPolarisations();

            // ### Assert
            _aliceNotebookEntries[0].WasPolarisationSent.Should().Be(true);
            _aliceNotebookEntries[1].WasPolarisationSent.Should().Be(true);
            _bobNotebookEntries[0].PolarisationPartner.Should().Be(firstPolarisation);
            _bobNotebookEntries[1].PolarisationPartner.Should().Be(secondPolarisation);
            _eveNotebookEntries[0].PolarisationAlice.Should().Be(firstPolarisation);
            _eveNotebookEntries[1].PolarisationAlice.Should().Be(secondPolarisation);
        }

        [Test]
        public void SendPolarisation_EveAndAliceShouldReceivePolarisationsFromBob()
        {
            // ### Arrange
            const Polarisation firstPolarisation = Polarisation.Diagonal;
            const Polarisation secondPolarisation = Polarisation.Rectilinear;

            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.Zero, Polarisation.Diagonal);

            _bob.NoteDownPolarisation(firstPolarisation);
            _bob.NoteDownPolarisation(secondPolarisation);

            // ### Act
            _bob.SendPolarisations();

            // ### Assert
            _bobNotebookEntries[0].WasPolarisationSent.Should().Be(true);
            _bobNotebookEntries[1].WasPolarisationSent.Should().Be(true);
            _aliceNotebookEntries[0].PolarisationPartner.Should().Be(firstPolarisation);
            _aliceNotebookEntries[1].PolarisationPartner.Should().Be(secondPolarisation);
            _eveNotebookEntries[0].PolarisationBob.Should().Be(firstPolarisation);
            _eveNotebookEntries[1].PolarisationBob.Should().Be(secondPolarisation);
        }

        [Test]
        public void SendPolarisation_ShouldSendEachPolarisationOnlyOnce()
        {
            // ### Arrange
            int secondPublishNumPolarisations = 0;

            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);
            _alice.NoteDownPhoton(DataBit.Zero, Polarisation.Rectilinear);

            // ### Act
            _alice.SendPolarisations();
            _publicNetwork.Subscribe<PolarisationUsedMessage>(this,
                message => secondPublishNumPolarisations = message.Polarisations.Count);
            _alice.SendPolarisations();

            // ### Assert
            secondPublishNumPolarisations.Should().Be(0);
        }

        [Test]
        public void SendPolarisation_ShouldNotSendEmptyPolarisations()
        {
            // ### Arrange
            const Polarisation firstPolarisation = Polarisation.Diagonal;
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1) { MyPolarisation = null, WasPolarisationSent = false, });

            _bobNotebookEntries.Add(new BobNotebookEntry(0));
            _bobNotebookEntries.Add(new BobNotebookEntry(1));
            _eveNotebookEntries.Add(new EveNotebookEntry(0));
            _eveNotebookEntries.Add(new EveNotebookEntry(1));

            // ### Act
            _alice.SendPolarisations();

            // ### Assert
            _aliceNotebookEntries[0].WasPolarisationSent.Should().Be(true);
            _aliceNotebookEntries[1].WasPolarisationSent.Should().Be(false);
            _bobNotebookEntries[0].PolarisationPartner.Should().Be(firstPolarisation);
            _bobNotebookEntries[1].PolarisationPartner.Should().Be(null);
            _eveNotebookEntries[0].PolarisationAlice.Should().Be(firstPolarisation);
            _eveNotebookEntries[1].PolarisationAlice.Should().Be(null);
        }
    }
}