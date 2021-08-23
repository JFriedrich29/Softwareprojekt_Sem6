using Stylet;

namespace QuantumCryptoCram.Presentation.Utility.Navigation
{
    public interface INavigationControllerDelegate
    {
        void NavigateTo(IScreen screen);
    }
}
