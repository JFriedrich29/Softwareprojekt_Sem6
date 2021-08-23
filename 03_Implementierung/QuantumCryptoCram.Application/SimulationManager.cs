using System;
using System.Collections.ObjectModel;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Application.Quantum;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Config;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Application
{
    /// <inheritdoc/>
    public class SimulationManager : ISimulationManager
    {
        private readonly SimulationOptions _options;
        private readonly Func<IRandomGenerator> _randomGenerator;
        private IPublicNetwork _network;
        private bool _isSimulationActive;

        /// <inheritdoc/>
        public IAlice Alice { get; private set; }

        /// <inheritdoc/>
        public IBob Bob { get; private set; }

        /// <inheritdoc/>
        public IEve Eve { get; private set; }

        /// <inheritdoc/>
        public bool IsAliceDone
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool IsBobDone
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public bool IsEveDone
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationManager"/> class.
        /// </summary>
        /// <param name="options">Current simulation options.</param>
        /// <param name="randomGenerator"> Common dependency random generator. It is a delegate function, so a new instance is created for every call.</param>
        public SimulationManager(SimulationOptions options, Func<IRandomGenerator> randomGenerator)
        {
            _options = options;
            _randomGenerator = randomGenerator;
        }

        /// <inheritdoc/>
        public void StartSimulation()
        {
            // With this check we only instantiate the classes once for the whole simulation
            // and therefore we also do not need to implement a singleton pattern for each class.
            if (_isSimulationActive)
                return;
            _isSimulationActive = true;

            // Set up photon measurement depending on the current user selection for photon cloning
            IPhotonMeasurement photonMeasurement;
            if (_options.IsPhotonCloningEnabled)
                photonMeasurement = new PhotonMeasurementWithCloning(_randomGenerator());
            else
                photonMeasurement = new RealPhotonMeasurement(_randomGenerator());

            // Set up public unencrypted network that the protocol roles can communicate with
            _network = new PublicNetwork();
            _network.Subscribe<RoleProtocolDoneMessage>(this, HandleRoleProtocolDoneHandler);

            // Set up main classes in the right communication topology
            if (_options.IsEveActive)
            {
                CreateEveActiveTopology(photonMeasurement);
            }
            else
            {
                CreateEveInactiveTopology(photonMeasurement);
            }
        }

        private void CreateEveInactiveTopology(IPhotonMeasurement photonMeasurement)
        {
            var pipe = new LocalQuantumPipe();
            Alice = new Alice(
                new ObservableCollection<AliceNotebookEntry>(),
                _network,
                pipe,
                _randomGenerator(),
                new PhotonGenerator(_randomGenerator(), photonMeasurement));

            Eve = null;

            Bob = new Bob(
                new ObservableCollection<BobNotebookEntry>(),
                _network,
                pipe,
                _randomGenerator());
        }

        private void CreateEveActiveTopology(IPhotonMeasurement photonMeasurement)
        {
            var pipeAE = new LocalQuantumPipe();
            var pipeEB = new LocalQuantumPipe();
            Alice = new Alice(
                new ObservableCollection<AliceNotebookEntry>(),
                _network,
                pipeAE,
                _randomGenerator(),
                new PhotonGenerator(_randomGenerator(), photonMeasurement));
            Eve = new Eve(
                new ObservableCollection<EveNotebookEntry>(),
                _network,
                pipeAE,
                pipeEB,
                _randomGenerator());
            Bob = new Bob(
                new ObservableCollection<BobNotebookEntry>(),
                _network,
                pipeEB,
                _randomGenerator());
        }

        /// <inheritdoc/>
        public void StopSimulation()
        {
            _isSimulationActive = false;
            IsAliceDone = false;
            IsBobDone = false;
            IsEveDone = false;

            Alice?.Dispose();
            Alice = null;
            Bob?.Dispose();
            Bob = null;
            Eve?.Dispose();
            Eve = null;
        }

        /// <summary>
        /// Handle the event where a role finishes its part of the protocol.
        /// </summary>
        /// <param name="message">The event.</param>
        private void HandleRoleProtocolDoneHandler(RoleProtocolDoneMessage message)
        {
            switch (message.Role)
            {
                case ProtocolRoleType.Alice:
                    IsAliceDone = true;
                    break;

                case ProtocolRoleType.Bob:
                    IsBobDone = true;
                    break;

                case ProtocolRoleType.Eve:
                    IsEveDone = true;
                    break;

                default:
                    throw new ProtocolException("Unknown protocol role finished.");
            }
        }
    }
}