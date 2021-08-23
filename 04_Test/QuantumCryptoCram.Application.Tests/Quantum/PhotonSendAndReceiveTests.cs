using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Events;
using System;
using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Application.Tests.Protocol;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;
using System.Collections.ObjectModel;

namespace QuantumCryptoCram.Application.Tests.Quantum
{
    [TestFixture]
    public class PhotonSendAndReceiveTests : ProtocolTestBase
    {
        [Test]
        public void SendPhotons_ShouldNotSendEmptyPhotonConfigurations()
        {
            // ### Arrange

            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = null,
                MyPolarisation = null,
                WasPhotonSent = false
            });

            using (IMonitor<LocalQuantumPipe> monitoredPipe = QuantumPipeAe.Monitor())
            {
                // ### Act

                _alice.SendPhotons();

                // ### Assert

                _aliceNotebookEntries[0].WasPhotonSent.Should().Be(false);

                // This assert guarantees that the PhotonReceived handler of Eve is called since it gets triggered by this event and thus the sending was successful.
                monitoredPipe.Should().NotRaise("PhotonReceived");
            }
        }

        [Test]
        public void SendPhotons_ShouldSendPhotonsFromAliceToEve()
        {
            // ### Arrange

            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);


            using (FluentAssertions.Events.IMonitor<LocalQuantumPipe> monitoredPipe = QuantumPipeAe.Monitor())
            {
                // ### Act

                _alice.SendPhotons();

                // ### Assert

                _aliceNotebookEntries[0].WasPhotonSent.Should().Be(true);

                // This assert guarantees that the PhotonReceived handler of Eve is called since it gets triggered by this event and thus the sending was successful.
                monitoredPipe.Should().Raise("PhotonReceived");
            }

        }

        [Test]
        public void SendPhotons_ShouldNotSendPhotonsTwice()
        {
            // ### Arrange

            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);
            _alice.NoteDownPhoton(DataBit.Zero, Polarisation.Rectilinear);

            using (FluentAssertions.Events.IMonitor<LocalQuantumPipe> monitoredPipe = QuantumPipeAe.Monitor())
            {
                // ### Act

                _alice.SendPhotons();

                _alice.SendPhotons();

                // ### Assert

                _aliceNotebookEntries[0].WasPhotonSent.Should().Be(true);
                _aliceNotebookEntries[1].WasPhotonSent.Should().Be(true);

                // If more than one event occurred then the photon has been sent twice
                monitoredPipe.OccurredEvents.Length.Should().Be(2);
            }
        }

        [Test]
        public void SendPhotons_ShouldSetBobPreKeyAndFinalKey_WhenPolarisationMatchingIsChecked()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = DataBit.One,
                MyPolarisation = Polarisation.Diagonal
            }) ;
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true
            });
            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Diagonal
            });

            // ### Act
            _alice.SendPhotons();

            // ### Assert
            _bobNotebookEntries[0].PreKey.Should().Be(DataBit.One);
            _bobNotebookEntries[0].FinalKey.Should().Be(DataBit.One);
        }
    }
}