using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    [TestFixture]
    internal class ProtocolPartnerTest : ProtocolTestBase
    {
        [Test]
        public void Test_PublicNetwork_AliceSendsPolarisationUsedMessage_BobHasNoPhotons()
        {
            // Arrange
            const int testPolarisationsCount = 3;
            const int emptyEntries = 3;
            const int totalEntries = emptyEntries + testPolarisationsCount;

            // create test values
            for (int i = emptyEntries; i < totalEntries; i++)
            {
                var testEntry =
                    new AliceNotebookEntry(i) { MyData = DataBit.One, MyPolarisation = Polarisation.Diagonal };
                _aliceNotebookEntries.Add(testEntry);
            }

            // Act
            _alice.SendPolarisations();

            _bobNotebookEntries.Count().Should().Be(totalEntries);
            Assert.IsInstanceOf(typeof(BobNotebookEntry), _bobNotebookEntries[0]);
            // Verify that bobs NotebookEntry is a uninitialized entry
            var emptyEntry = new BobNotebookEntry();
            _bobNotebookEntries[0].MyPolarisation.Should().Be(emptyEntry.MyPolarisation);
            _bobNotebookEntries[0].MyData.Should().Be(emptyEntry.MyData);
            _bobNotebookEntries[0].Id.Should().Be(emptyEntry.Id);
        }

        [Test]
        public void Test_GetFinalKey_ReturnsCorrectKey()
        {
            // Arrange
            DataBit[] testKeyBits =
            {
                DataBit.One,
                DataBit.Zero,
                DataBit.One,
                DataBit.One
            };

            // create test values
            int count = 0;
            foreach (DataBit bit in testKeyBits)
            {
                var testEntry = new AliceNotebookEntry(count)
                {
                    MyData = bit,
                    MyPolarisation = Polarisation.Diagonal,
                    FinalKey = bit
                };
                _aliceNotebookEntries.Add(testEntry);

                count++;
            }

            // Act
            Key generatedKey = _alice.GetFinalKey();

            Key expectedKey = new Key(testKeyBits.AsEnumerable());

            // Verify Key contents are equal
            expectedKey.ToBitArray.Should().Equal(generatedKey.ToBitArray);
        }
    }
}