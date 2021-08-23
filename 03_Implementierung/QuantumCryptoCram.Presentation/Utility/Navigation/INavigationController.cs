namespace QuantumCryptoCram.Presentation.Utility.Navigation
{
    public interface INavigationController
    {
        void NavigateToAlice();

        void NavigateToAliceEncryptionTest();

        void NavigateToBob();

        void NavigateToBobEncryptionTest();

        void NavigateToEve();

        void NavigateToEveEncryption();

        void NavigateToMain();

        void NavigateToResult();

        void NavigateToLocalModeView();

        void NavigateToNetworkModeView();

        void NavigateToSimulationOverviewView();

        void NavigateToProtocolAnalysisView();
    }
}
