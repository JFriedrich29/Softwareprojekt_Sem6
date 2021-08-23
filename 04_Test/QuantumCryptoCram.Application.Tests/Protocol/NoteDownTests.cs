using System.Collections.Generic;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    [TestFixture]
    public class NoteDownTest : ProtocolTestBase
    {
        [Test]
        public void NoteDownPolarisation_ShouldAppendCorrectlyToInternalList()
        {
            // ### Act

            _bob.NoteDownPolarisation(Polarisation.Diagonal);
            _bob.NoteDownPolarisation(Polarisation.Rectilinear);

            // ### Assert

            _bobNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);

            _bobNotebookEntries[1].MyPolarisation.Should().Be(Polarisation.Rectilinear);
        }

        [Test]
        public void NoteDownPolarisationRandom_ShouldAppendCorrectlyToInternalList()
        {
            // ### Arrange

            bool notIdenticalEntryDetected = false;

            // ### Act

            _bob.NoteDownRandomPolarisations(10);

            // See below why this is commented out
            //BobNotebookEntry firstNotebookEntry = _bobNotebookEntries[0];

            // ### Assert

            for (int i = 0; i < _bobNotebookEntries.Count; i++)
            {
                if (i == 0) continue;
                else if (_bobNotebookEntries[i].MyPolarisation != _bobNotebookEntries[i - 1].MyPolarisation) notIdenticalEntryDetected = true;

                // Alternative way to test this but i dont know if it is valid to test like this
                //_bobNotebookEntries[i].MyPolarisation.Should().Be(firstNotebookEntry.MyPolarisation);
            }

            notIdenticalEntryDetected.Should().Be(false);
        }

        [Test]
        public void NoteDownPhoton_ShouldAppendCorrectlyToInternalList()
        {
            // ### Act

            _alice.NoteDownPhoton(DataBit.Zero, Polarisation.Diagonal);
            _alice.NoteDownPhoton(DataBit.Zero, Polarisation.Rectilinear);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Assert

            _aliceNotebookEntries[0].MyData.Should().Be(DataBit.Zero);
            _aliceNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);

            _aliceNotebookEntries[1].MyData.Should().Be(DataBit.Zero);
            _aliceNotebookEntries[1].MyPolarisation.Should().Be(Polarisation.Rectilinear);

            _aliceNotebookEntries[2].MyData.Should().Be(DataBit.One);
            _aliceNotebookEntries[2].MyPolarisation.Should().Be(Polarisation.Diagonal);

            _aliceNotebookEntries[3].MyData.Should().Be(DataBit.One);
            _aliceNotebookEntries[3].MyPolarisation.Should().Be(Polarisation.Rectilinear);
        }

        [Test]
        public void NoteDownPhotonRandom_ShouldAppendCorrectlyToInternalList()
        {
            // ### Arrange

            bool notIdenticalEntryDetected = false;

            // ### Act

            _alice.NoteDownRandomPhotons(10);

            //See below why this is commented out
            //AliceNotebookEntry firstNotebookEntry = _aliceNotebookEntries[0];

            // ### Assert

            for (int i = 0; i < _aliceNotebookEntries.Count; i++)
            {
                if (i == 0) continue;
                else if (
                    _aliceNotebookEntries[i].MyPolarisation != _aliceNotebookEntries[i - 1].MyPolarisation ||
                    _aliceNotebookEntries[i].MyData != _aliceNotebookEntries[i - 1].MyData
                    )
                    notIdenticalEntryDetected = true;

                // Alternative way to test this but i dont know if it is valid to test like this
                //_aliceNotebookEntries[i].MyPolarisation.Should().Be(firstNotebookEntry.MyPolarisation);
                //_aliceNotebookEntries[i].MyData.Should().Be(firstNotebookEntry.MyData);
            }

            notIdenticalEntryDetected.Should().Be(false);
        }

        [Test]
        public void NoteDown_ShouldUseExistingEntries_AliceSendsPolarisation()
        {
            // This UnitTest creates polarisation entries in Eve and Bob before they create any entries themselfs


            // ### Arrange
            var entry = new AliceNotebookEntry
            {
                MyPolarisation = Polarisation.Rectilinear,
            };
            _aliceNotebookEntries.Add(entry);
            _alice.SendPolarisations();

            // ### Act
            _bob.NoteDownPolarisation(Polarisation.Diagonal);

            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert

            _bobNotebookEntries.Count.Should().Be(1);
            _bobNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);
            _bobNotebookEntries[0].PolarisationPartner.Should().Be(Polarisation.Rectilinear);

            _eveNotebookEntries.Count.Should().Be(1);
            _eveNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);
            _eveNotebookEntries[0].PolarisationAlice.Should().Be(Polarisation.Rectilinear);
        }

        [Test]
        public void NoteDown_ShouldUseExistingEntries_BobSendsPolarisation()
        {
            // This UnitTest creates polarisation entries in Eve and Alice before they create any entries themselfs


            // ### Arrange
            var entry = new BobNotebookEntry
            {
                MyPolarisation = Polarisation.Rectilinear,
            };
            _bobNotebookEntries.Add(entry);
            _bob.SendPolarisations();

            // ### Act
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Diagonal);

            _eve.NoteDownPolarisation(Polarisation.Diagonal);

            // ### Assert

            _aliceNotebookEntries.Count.Should().Be(1);
            _aliceNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);
            _aliceNotebookEntries[0].MyData.Should().Be(DataBit.One);
            _aliceNotebookEntries[0].PolarisationPartner.Should().Be(Polarisation.Rectilinear);

            _eveNotebookEntries.Count.Should().Be(1);
            _eveNotebookEntries[0].MyPolarisation.Should().Be(Polarisation.Diagonal);
            _eveNotebookEntries[0].PolarisationBob.Should().Be(Polarisation.Rectilinear);
        }
    }
}