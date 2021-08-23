using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using QuantumCryptoCram.Common.Random;

namespace QuantumCryptoCram.Common.Tests
{
    /// <summary>
    /// Dummy test so pipeline does not fail because of an empty test project
    /// </summary>
    [TestFixture]
    public class RandomGeneratorTests
    {
        [Test]
        public void PickRandomItems_ShouldPickMaxPossibleItems()
        {
            // Arrange
            IEnumerable<int> entriesToSelect = Enumerable.Range(0, 10);
            RandomGenerator randomGenerator = new RandomGenerator();

            // Act
            IList<int> randomPickedItems = randomGenerator.PickRandomItems(entriesToSelect, 10);

            // Assert
            randomPickedItems.Count.Should().Be(10);
        }

        [Test]
        public void PickRandomItems_ShouldPickAsManyAsRequested()
        {
            // Arrange
            IEnumerable<int> entriesToSelect = Enumerable.Range(0, 10);
            RandomGenerator randomGenerator = new RandomGenerator();
            int amountWanted = 5;

            // Act
            IList<int> randomPickedItems = randomGenerator.PickRandomItems(entriesToSelect, amountWanted);

            // Assert
            randomPickedItems.Count.Should().Be(amountWanted);
        }

        [Test]
        public void PickRandomItems_ShouldPickNoneIfAmountWantedIsZero()
        {
            // Arrange
            IEnumerable<int> entriesToSelect = Enumerable.Range(0, 10);
            RandomGenerator randomGenerator = new RandomGenerator();
            int amountWanted = 0;

            // Act
            Action act = () => randomGenerator.PickRandomItems(entriesToSelect, amountWanted);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void PickRandomItems_ShouldPickNoneIfAmountIsInvalid()
        {
            // Arrange
            IEnumerable<int> entriesToSelect = Enumerable.Range(0, 10);
            RandomGenerator randomGenerator = new RandomGenerator();
            int amountWanted = 15;

            // Act
            Action act = () => randomGenerator.PickRandomItems(entriesToSelect, amountWanted);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}