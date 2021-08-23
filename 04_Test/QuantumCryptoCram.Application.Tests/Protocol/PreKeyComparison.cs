using FluentAssertions;
using NUnit.Framework;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    /// <summary>
    /// Test cases for the PreKeyComparison Message according to UC-18.
    /// </summary>
    [TestFixture]
    public class PreKeyComparison : ProtocolTestBase
    {
        [Test]
        public void PreKeyComparison_PartnerReceivesBitsForComparison()
        {
            // ### Arrange
            DataBit prekeyBit = DataBit.One;

            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = prekeyBit,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPreKeySelectedForComparison = true,
                IsPolarisationMatching = true,
                PreKey = prekeyBit,
                FinalKey = prekeyBit,
            });
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1)
            {
                MyData = DataBit.Zero,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPolarisationMatching = true,
                FinalKey = DataBit.Zero,
            });

            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = prekeyBit,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                FinalKey = prekeyBit,
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(1)
            {
                MyData = DataBit.Zero,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                FinalKey = DataBit.Zero,
            });

            // ### Act
            _alice.SendPrekeySelectionForComparison();

            // ### Assert
            _aliceNotebookEntries[0].WasPreKeySelectionSentOrReceived.Should().Be(true);
            _aliceNotebookEntries[1].WasPreKeySelectionSentOrReceived.Should().Be(false);
            _aliceNotebookEntries[0].FinalKey.Should().BeNull();
            _aliceNotebookEntries[1].FinalKey.Should().NotBeNull();

            _bobNotebookEntries[0].WasPreKeySelectionSentOrReceived.Should().Be(true);
            _bobNotebookEntries[1].WasPreKeySelectionSentOrReceived.Should().Be(false);
            _bobNotebookEntries[0].PreKeyPartner.Should().Be(prekeyBit);
            _bobNotebookEntries[1].PreKeyPartner.Should().BeNull();
            _bobNotebookEntries[0].FinalKey.Should().BeNull();
            _bobNotebookEntries[1].FinalKey.Should().NotBeNull();
        }

        [Test]
        public void PreKeyComparison_DoNotSendPrekeyTwice()
        {
            // ### Arrange
            int secondTimeNumSent = 0;
            DataBit prekeyBit = DataBit.One;

            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = prekeyBit,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPreKeySelectedForComparison = true,
                IsPolarisationMatching = true,
                PreKey = prekeyBit,
                FinalKey = prekeyBit,
            });
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1)
            {
                MyData = DataBit.Zero,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPolarisationMatching = true,
                FinalKey = DataBit.Zero,
            });

            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = prekeyBit,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                FinalKey = prekeyBit,
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(1)
            {
                MyData = DataBit.Zero,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                FinalKey = DataBit.Zero,
            });

            // ### Act
            _alice.SendPrekeySelectionForComparison();
            _aliceNotebookEntries[1].IsPreKeySelectedForComparison = true;
            _aliceNotebookEntries[1].PreKey = _aliceNotebookEntries[1].MyData;

            _publicNetwork.Subscribe<PreKeySelectionForComparisonMessage>(this, msg =>
                secondTimeNumSent = msg.SelectedPreKeyBits.Count
            );
            _alice.SendPrekeySelectionForComparison();


            // ### Assert
            secondTimeNumSent.Should().Be(1);
        }
    }
}
