using System;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the ResultView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class ResultViewModel : Screen
    {
        private readonly INavigationController _navigationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">The navigation controller.</param>
        /// <exception cref="ArgumentNullException">navigationController.</exception>
        public ResultViewModel(INavigationController navigationController)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
        }

        /// <summary>
        /// Navigates to Alice.
        /// </summary>
        public void NavigateToAliceCommand() => _navigationController.NavigateToAlice();

        /// <summary>
        /// Navigates to Bob.
        /// </summary>
        public void NavigateToBobCommand() => _navigationController.NavigateToBob();

        /// <summary>
        /// Navigates to Eve.
        /// </summary>
        public void NavigateToEveCommand() => _navigationController.NavigateToEve();

        /// <summary>
        /// Navigates to main.
        /// </summary>
        public void NavigateToMainCommand() => _navigationController.NavigateToMain();
    }
}
