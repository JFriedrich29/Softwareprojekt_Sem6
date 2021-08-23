using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    [TestFixture]
    class PartialEntries: ProtocolTestBase
    {
        [Test]
        public void ParitalEntries_Issue100Alice()
        {
            // ### Arrange
            _bob.NoteDownPolarisation(Polarisation.Rectilinear);

            // ### Act
            _bob.SendPolarisations();
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Assert
            _aliceNotebookEntries.Count.Should().Be(1);
        }

        [Test]
        public void ParitalEntries_Issue100Bob()
        {
            // ### Arrange
            _alice.NoteDownPhoton(DataBit.One, Polarisation.Rectilinear);

            // ### Act
            _alice.SendPolarisations();
            _bob.NoteDownPolarisation(Polarisation.Rectilinear);

            // ### Assert
            _bobNotebookEntries.Count.Should().Be(1);
        }

        [Test]
        public void ParitalEntries_Issue100Eve()
        {
            // TODO eve logic not fully implemented and thus
            // this case cannot be tested here yet.
        }
    }
}
