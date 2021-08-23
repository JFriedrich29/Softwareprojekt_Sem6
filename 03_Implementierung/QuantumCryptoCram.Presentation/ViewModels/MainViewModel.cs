using System;
using MaterialDesignThemes.Wpf;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the MainView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class MainViewModel : Screen
    {
        private readonly INavigationController _navigationController;

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        private void ToggleBaseColour(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">The navigation controller.</param>
        /// <exception cref="ArgumentNullException">navigationController.</exception>
        public MainViewModel(INavigationController navigationController)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));

            ToggleBaseColour(false);
        }

        /// <summary>
        /// Navigates to network.
        /// </summary>
        public void NavigateToNetworkCommand() => _navigationController.NavigateToNetworkModeView();

        /// <summary>
        /// Navigates to local.
        /// </summary>
        public void NavigateToLocalCommand() => _navigationController.NavigateToLocalModeView();

/// <summary>
/// Closes the application without confirmation.
/// </summary>
        public void CloseWindowCommand()
        {
            System.Windows.Application.Current.Shutdown();       
        }
    }
}
