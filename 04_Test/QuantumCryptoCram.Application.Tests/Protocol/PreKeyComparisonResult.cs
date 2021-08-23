using FluentAssertions;

using NUnit.Framework;

using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    /// <summary>
    /// Test cases for the PreKeyComparisonResult message according to UC-20.
    /// </summary>
    [TestFixture]
    public class PreKeyComparisonResult : ProtocolTestBase
    {
        [Test]
        public void PreKeyComparisonResult_PartnerReceivesResult()
        {
            DataBit firstBitMatching = DataBit.One;
            DataBit secondBitAlice = DataBit.One;
            DataBit secondBitBob = DataBit.Zero;

            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = firstBitMatching,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPreKeySelectedForComparison = true,
                IsPolarisationMatching = true,
                WasPreKeySelectionSentOrReceived = true,
                PreKey = firstBitMatching,
            });
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1)
            {
                MyData = secondBitAlice,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                IsPreKeySelectedForComparison = true,
                IsPolarisationMatching = true,
                WasPreKeySelectionSentOrReceived = true,
                PreKey = secondBitAlice,
            });

            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = firstBitMatching,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                PreKey = firstBitMatching,
                IsPreKeyMatching = true,
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(1)
            {
                MyData = secondBitBob,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                PreKey = secondBitBob,
                IsPreKeyMatching = false,
            });

            // ### Act
            _bob.SendPrekeyMatches();

            // ### Assert
            _aliceNotebookEntries[0].IsPreKeyMatching.Should().Be(true);
            _aliceNotebookEntries[1].IsPreKeyMatching.Should().Be(false);
        }

        // Initial messages can only be sent once, but matches messages can be sent multiple times.
        [Test]
        public void PreKeyComparisonResult_ShouldSendTwice()
        {
            int secondTimeNumSent = 0;
            DataBit firstBitMatching = DataBit.One;

            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = firstBitMatching,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                IsPreKeySelectedForComparison = true,
                PreKey = firstBitMatching,
                WasPreKeySelectionSentOrReceived = true,
                IsPreKeyMatching = true,
            });

            // ### Act
            _bob.SendPrekeyMatches();
            _publicNetwork.Subscribe<PreKeyComparisonResultMessage>(this, msg => secondTimeNumSent = msg.Matches.Count);
            _bob.SendPrekeyMatches();

            // ### Assert
            secondTimeNumSent.Should().Be(1);
        }

        [Test]
        public void PreKeyComparisonResult_AliceAddedPrekeyBitsAfterPrekeySentForComparison()
        {
            DataBit firstBitMatching = DataBit.One;
            DataBit secondBit = DataBit.One;

            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                MyData = firstBitMatching,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                IsPolarisationMatching = true,
                WasPolarisationSent = true,
                PolarisationPartner = Polarisation.Diagonal,
                PreKey = firstBitMatching,
                IsPreKeySelectedForComparison = true,
            });
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1)
            {
                MyData = secondBit,
                MyPolarisation = Polarisation.Diagonal,
                WasPhotonSent = true,
                IsPolarisationMatching = true,
                PolarisationPartner = Polarisation.Diagonal,
                WasPolarisationSent = true,
                PreKey = secondBit,
                IsPreKeyMatching = true,
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = firstBitMatching,
                MyPolarisation = Polarisation.Diagonal,
                PolarisationPartner = Polarisation.Diagonal,
                IsPolarisationMatching = true,
                IsPreKeySelectedForComparison = true,
                PreKey = firstBitMatching,
            });

            // ### Act
            _alice.SendPrekeySelectionForComparison();
            _aliceNotebookEntries[1].IsPreKeySelectedForComparison = true;

            _bobNotebookEntries[0].IsPreKeyMatching = true;
            _bob.SendPrekeyMatches();

            // ### Assert
            // Check that the partner prekey bit is still null as per MR !69 comment
            // 8360.
            _aliceNotebookEntries[1].PreKeyPartner.Should().BeNull();

            // IsPrekeyMatching was initialized to true, which makes no sense in the
            // actual protocol. This value should be left unchanged by the
            // comparison result message.
            _aliceNotebookEntries[1].IsPreKeyMatching.Should().BeTrue();
        }
    }
}