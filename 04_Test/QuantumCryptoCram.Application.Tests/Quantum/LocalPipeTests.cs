using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Quantum
{
    [TestFixture]
    public class LocalPipeTests
    {
        [Test]
        public void Test_LocalPipe_Communication()
        {
            // Arrange
            IPhoton photon = Substitute.For<IPhoton>();
            bool fired = false;
            var pipe = new LocalQuantumPipe();
            pipe.PhotonReceived += (s, p) => fired = true;

            // Act
            pipe.SendPhoton(photon);

            // Assert
            fired.Should().BeTrue();
        }
    }
}