using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    /// <summary>
    /// Test cases for the use case UC-14.
    /// </summary>
    [TestFixture]
    public class SendPreKeySelectionForComparison : ProtocolTestBase
    {
        [Test]
        public void SendPreKeySelectionForComparison_ShouldUpdatePartnerPreKeyCorrectly()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0) { MyData = DataBit.One, PreKey = DataBit.One, IsPreKeySelectedForComparison = true });
            _aliceNotebookEntries.Add(new AliceNotebookEntry(1) { MyData = DataBit.One, PreKey = DataBit.Zero, IsPreKeySelectedForComparison = true });

            _bobNotebookEntries.Add(new BobNotebookEntry(0));
            _bobNotebookEntries.Add(new BobNotebookEntry(1));

            // ### Act
            _alice.SendPrekeySelectionForComparison();

            // ### Assert
            _bobNotebookEntries.Count.Should().Be(2);
            _bobNotebookEntries[0].PreKeyPartner.Should().Be(DataBit.One);
            _bobNotebookEntries[1].PreKeyPartner.Should().Be(DataBit.Zero);
        }

        [Test]
        public void SendPreKeySelectionForComparison_ShouldTrowExceptionIfPreKeyBitIsSelectedButHasNoValue()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0) { MyData = DataBit.One, IsPreKeySelectedForComparison = true });

            // ### Act
            Action sendPreKeySelections = () => _alice.SendPrekeySelectionForComparison();

            // ### Assert
            sendPreKeySelections.Should().ThrowExactly<ProtocolException>();
        }

        [Test]
        public void SendPreKeySelectionForComparison_ShouldAssignEveRelevanceTypeAsAliceBobMatchButDiscarded()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPreKeySelectedForComparison = true,
                MyData = DataBit.One,
                PreKey = DataBit.One
            });
            _bobNotebookEntries.Add(new BobNotebookEntry(0));
            _eveNotebookEntries.Add(new EveNotebookEntry(0));

            // ### Act
            _alice.SendPrekeySelectionForComparison();

            // ### Assert
            _eveNotebookEntries[0].RelevanceType.Should().Be(MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded);
        }

        [Test]
        public void SendPreKeySelectionForComparison_ShouldSendEachPreKeyOnlyOnce()
        {
            // ### Arrange
            _aliceNotebookEntries.Add(new AliceNotebookEntry(0)
            {
                IsPreKeySelectedForComparison = true,
                MyData = DataBit.One,
                PreKey = DataBit.One
            });
            int secondTimeNumSent = 0;

            // ### Act
            _alice.SendPrekeySelectionForComparison();
            _publicNetwork.Subscribe<PreKeySelectionForComparisonMessage>(this, msg => secondTimeNumSent = msg.SelectedPreKeyBits.Count);
            _alice.SendPrekeySelectionForComparison();

            // ### Assert
            secondTimeNumSent.Should().Be(0);
        }
    }
}