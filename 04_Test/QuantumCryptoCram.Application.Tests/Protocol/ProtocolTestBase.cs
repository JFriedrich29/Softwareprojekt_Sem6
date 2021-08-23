using System.Collections.ObjectModel;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application.Tests.Protocol
{
    [TestFixture]
    public class ProtocolTestBase
    {
        protected Alice _alice;
        protected Bob _bob;
        protected Eve _eve;
        protected IRandomGenerator _randomGenerator;
        protected IPhotonMeasurement _photonMeasurement;
        protected IPublicNetwork _publicNetwork;
        protected IEncryptionService _encryptionService;
        protected PhotonGenerator _photonGenerator;
        protected LocalQuantumPipe QuantumPipeAe;
        protected LocalQuantumPipe QuantumPipeEb;
        protected ObservableCollection<AliceNotebookEntry> _aliceNotebookEntries;
        protected ObservableCollection<BobNotebookEntry> _bobNotebookEntries;
        protected ObservableCollection<EveNotebookEntry> _eveNotebookEntries;

        [SetUp]
        public void Setup()
        {
            // ### Common Arrange
            _randomGenerator = Substitute.For<IRandomGenerator>();
            _randomGenerator.GetRandomBool().Returns(true);
            _photonMeasurement = new RealPhotonMeasurement(_randomGenerator);
            _photonGenerator = new PhotonGenerator(_randomGenerator, _photonMeasurement);
            _publicNetwork = new PublicNetwork();
            _encryptionService = new XorEncryptionService();

            QuantumPipeAe = new LocalQuantumPipe();
            QuantumPipeEb = new LocalQuantumPipe();

            _aliceNotebookEntries = new ObservableCollection<AliceNotebookEntry>();
            _bobNotebookEntries = new ObservableCollection<BobNotebookEntry>();
            _eveNotebookEntries = new ObservableCollection<EveNotebookEntry>();

            _alice = new Alice(_aliceNotebookEntries, _publicNetwork, QuantumPipeAe, _randomGenerator, _photonGenerator);
            _eve = new Eve(_eveNotebookEntries, _publicNetwork, QuantumPipeAe, QuantumPipeEb, _randomGenerator);
            _bob = new Bob(_bobNotebookEntries, _publicNetwork, QuantumPipeEb, _randomGenerator);
        }
    }
}