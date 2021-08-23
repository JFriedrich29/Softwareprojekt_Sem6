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
    /// Test cases for the use case UC-16.
    /// </summary>
    [TestFixture]
    public class SendPolarisationMatches : ProtocolTestBase
    {
        [Test]
        public void SendPolarisationMatches_ByAlice_ShouldAssignEveRelevanceTypeAsNoMatch_WhenPartnerPolarisationAndOwnPolarisationDoesNotMatch()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = false
            });
            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Rectilinear,
                PolarisationAlice = Polarisation.Rectilinear
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.NoMatch);
        }

        [Test]
        public void SendPolarisationMatches_ByBob_ShouldAssignEveRelevanceTypeAsNoMatch_WhenPartnerPolarisationAndOwnPolarisationDoesNotMatch()
        {
            // ### Arrange
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                IsPolarisationMatching = false
            });
            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Rectilinear,
                PolarisationBob = Polarisation.Rectilinear
            });

            // ### Act
            _bob.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.NoMatch);
        }

        [Test]
        public void SendPolarisationMatches_ByAlice_ShouldAssignEveRelevanceTypeAsAliceBobEveMatch_WhenPartnerPolarisationAndOwnPolarisationMatches()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });

            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Rectilinear,
                PolarisationBob = Polarisation.Rectilinear,
                MyData = DataBit.One
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobEveMatch);
        }

        [Test]
        public void SendPolarisationMatches_ByBob_ShouldAssignEveRelevanceTypeAsAliceBobEveMatch_WhenPartnerPolarisationAndOwnPolarisationMatches()
        {
            // ### Arrange
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });

            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Rectilinear,
                PolarisationAlice = Polarisation.Rectilinear,
                MyData = DataBit.One
            });

            // ### Act
            _bob.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobEveMatch);
        }

        [Test]
        public void SendPolarisationMatches_ByAlice_ShouldAssignEveRelevanceTypeAsAliceBobMatch_WhenPartnerPolarisationMatchesButOwnDoesNotMatch()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });

            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Diagonal,
                PolarisationBob = Polarisation.Rectilinear,
                MyData = DataBit.One
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobMatch);
        }

        [Test]
        public void SendPolarisationMatches_ByBob_ShouldAssignEveRelevanceTypeAsAliceBobMatch_WhenPartnerPolarisationMatchesButOwnDoesNotMatch()
        {
            // ### Arrange
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });

            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Diagonal,
                PolarisationAlice = Polarisation.Rectilinear,
                MyData = DataBit.One
            });

            // ### Act
            _bob.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobMatch);
        }

        [Test]
        public void SendPolarisationMatches_ShouldNotDeassignEveRelevanceTypeAliceBobMatchButDiscarded()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });

            _eveNotebookEntries.Add(new EveNotebookEntry(0)
            {
                MyPolarisation = Polarisation.Diagonal,
                PolarisationBob = Polarisation.Rectilinear,
                MyData = DataBit.One,
                RelevanceType = MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded);
        }

        // Initial messages can only be sent once, but matches messages can be sent multiple times.
        [Test]
        public void SendPolarisationMatches_ShouldBeAbleToSendSameMatchEntryTwice()
        {
            // ### Arrange
            int secondTimeNumSent = 0;
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });


            // ### Act
            _alice.SendPolarisationMatches();
            _publicNetwork.Subscribe<PolarisationMatchesMessage>(this, msg => secondTimeNumSent = msg.Matches.Count);
            _alice.SendPolarisationMatches();

            // ### Assert
            secondTimeNumSent.Should().Be(1);
        }

        [Test]
        public void SendPolarisationMatches_ShouldAssignPartnersPreKeyAndFinalKey()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = DataBit.One,
                WasPolarisationSent = true
            }); ;


            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _bobNotebookEntries[0].IsPolarisationMatching.Should().BeTrue();
            _bobNotebookEntries[0].PreKey.Should().Be(DataBit.One);
            _bobNotebookEntries[0].FinalKey.Should().Be(DataBit.One);
        }

        [Test]
        public void SendPolarisationMatches_ShouldNotAssignFinalKey_WhenWasPrekeySelectionSentOrReceivedIsSet()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = true
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                MyData = DataBit.One,
                WasPreKeySelectionSentOrReceived = true,
                WasPolarisationSent = true
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _bobNotebookEntries[0].PreKey.Should().Be(DataBit.One);
            _bobNotebookEntries[0].FinalKey.Should().BeNull();
            _bobNotebookEntries[0].IsPolarisationMatching.Should().BeTrue();
        }

        [Test]
        public void SendPolarisationMatches_ShouldDeassignPartnersPreKeyAndFinalKey()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPolarisationMatching = false
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(0)
            {
                PreKey = DataBit.One,
                FinalKey = DataBit.One,
                WasPolarisationSent = true
            });

            // ### Act
            _alice.SendPolarisationMatches();

            // ### Assert
            _bobNotebookEntries[0].PreKey.Should().BeNull();
            _bobNotebookEntries[0].FinalKey.Should().BeNull();
            _bobNotebookEntries[0].IsPolarisationMatching.Should().BeFalse();
        }
    }
}