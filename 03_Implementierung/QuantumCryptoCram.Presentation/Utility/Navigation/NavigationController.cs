using System;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;
using QuantumCryptoCram.Presentation.ViewModels.Protocol;

namespace QuantumCryptoCram.Presentation.Utility.Navigation
{
    public class NavigationController : INavigationController
    {
        private readonly Func<AliceViewModel> _aliceViewModelFactory;
        private readonly Func<AliceEncryptionTestViewModel> _aliceEncryptionTestViewModelFactory;
        private readonly Func<BobViewModel> _bobViewModelFactory;
        private readonly Func<BobEncryptionTestViewModel> _bobEncryptionTestViewModelFactory;
        private readonly Func<EveViewModel> _eveViewModelFactory;
        private readonly Func<EveEncryptionTestViewModel> _eveEncryptionTestViewModelFactory;
        private readonly Func<MainViewModel> _mainViewModelFactory;
        private readonly Func<ResultViewModel> _resultViewModelFactory;
        private readonly Func<LocalModeViewModel> _localViewModelFactory;
        private readonly Func<NetworkModeViewModel> _networkViewModelFactory;
        private readonly Func<SimulationOverviewViewModel> _simulationOverviewViewModelFactory;
        private readonly Func<ProtocolAnalysisViewModel> _protocolAnalysisViewModelFactory;

        public NavigationController(
            Func<AliceViewModel> aliceViewModelFactory,
            Func<AliceEncryptionTestViewModel> aliceEncrytionTestViewModelFactory,
            Func<BobViewModel> bobViewModelFactory,
            Func<BobEncryptionTestViewModel> bobEncryptionTestViewModelFactory,
            Func<EveViewModel> eveViewModelFactory,
            Func<EveEncryptionTestViewModel> eveEncryptionTestViewModelFactory,
            Func<MainViewModel> mainViewModelFactory,
            Func<ResultViewModel> resultViewModelFactory,
            Func<LocalModeViewModel> localViewModelFactory,
            Func<NetworkModeViewModel> networkViewModelFactory,
            Func<SimulationOverviewViewModel> simulationOverviewViewModelFactory,
            Func<ProtocolAnalysisViewModel> protocolAnalysisViewModelFactory)
        {
            _aliceViewModelFactory = aliceViewModelFactory ?? throw new ArgumentNullException(nameof(aliceViewModelFactory));
            _aliceEncryptionTestViewModelFactory = aliceEncrytionTestViewModelFactory ?? throw new ArgumentNullException(nameof(aliceEncrytionTestViewModelFactory));
            _bobViewModelFactory = bobViewModelFactory ?? throw new ArgumentNullException(nameof(bobViewModelFactory));
            _bobEncryptionTestViewModelFactory = bobEncryptionTestViewModelFactory ?? throw new ArgumentNullException(nameof(bobEncryptionTestViewModelFactory));
            _eveViewModelFactory = eveViewModelFactory ?? throw new ArgumentNullException(nameof(eveViewModelFactory));
            _eveEncryptionTestViewModelFactory = eveEncryptionTestViewModelFactory ?? throw new ArgumentNullException(nameof(eveEncryptionTestViewModelFactory));
            _mainViewModelFactory = mainViewModelFactory ?? throw new ArgumentNullException(nameof(mainViewModelFactory));
            _resultViewModelFactory = resultViewModelFactory ?? throw new ArgumentNullException(nameof(resultViewModelFactory));
            _localViewModelFactory = localViewModelFactory ?? throw new ArgumentNullException(nameof(localViewModelFactory));
            _networkViewModelFactory = networkViewModelFactory ?? throw new ArgumentNullException(nameof(networkViewModelFactory));
            _simulationOverviewViewModelFactory = simulationOverviewViewModelFactory ?? throw new ArgumentNullException(nameof(simulationOverviewViewModelFactory));
            _protocolAnalysisViewModelFactory = protocolAnalysisViewModelFactory ?? throw new ArgumentNullException(nameof(protocolAnalysisViewModelFactory));
        }

        public INavigationControllerDelegate Delegate { get; set; }

        public void NavigateToAlice()
        {
            Delegate?.NavigateTo(_aliceViewModelFactory());
        }

        public void NavigateToAliceEncryptionTest()
        {
            Delegate?.NavigateTo(_aliceEncryptionTestViewModelFactory());
        }

        public void NavigateToBob()
        {
            Delegate?.NavigateTo(_bobViewModelFactory());
        }

        public void NavigateToBobEncryptionTest()
        {
            Delegate?.NavigateTo(_bobEncryptionTestViewModelFactory());
        }

        public void NavigateToEve()
        {
            Delegate?.NavigateTo(_eveViewModelFactory());
        }

        public void NavigateToEveEncryption()
        {
            Delegate?.NavigateTo(_eveEncryptionTestViewModelFactory());
        }

        public void NavigateToLocalModeView()
        {
            Delegate?.NavigateTo(_localViewModelFactory());
        }

        public void NavigateToMain()
        {
            Delegate?.NavigateTo(_mainViewModelFactory());
        }

        public void NavigateToNetworkModeView()
        {
            Delegate?.NavigateTo(_networkViewModelFactory());
        }

        public void NavigateToResult()
        {
            Delegate?.NavigateTo(_resultViewModelFactory());
        }

        public void NavigateToSimulationOverviewView()
        {
            Delegate?.NavigateTo(_simulationOverviewViewModelFactory());
        }

        public void NavigateToProtocolAnalysisView()
        {
            Delegate?.NavigateTo(_protocolAnalysisViewModelFactory());
        }
    }
}
