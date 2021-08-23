using QuantumCryptoCram.Presentation.Utility.Navigation;
using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the ShellViewModel.
    /// </summary>
    /// <seealso cref="Stylet.Conductor{Stylet.IScreen}" />
    /// <seealso cref="INavigationControllerDelegate" />
    public class ShellViewModel : Conductor<IScreen>, INavigationControllerDelegate
    {
        private string _textFromView;

        /// <summary>
        /// Gets or sets the text from view.
        /// </summary>
        /// <value>
        /// The text from view.
        /// </value>
        public string TextFromView
        {
            get => _textFromView;
            set
            {
                _textFromView = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Navigates to given View.
        /// </summary>
        /// <param name="screen">The screen.</param>
        public void NavigateTo(IScreen screen) => ActivateItem(screen);
    }
}
