using System.Windows;
using System.Windows.Input;

namespace QuantumCryptoCram.Presentation.Views
{
    /// <summary>
    /// Interaction logic for RootView.xaml.
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private bool _isFullscreen = false;
        private WindowState _lastWindowState;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                ToggleFullScreenMode();
            }
        }

        /// <summary>
        /// Toggles the application full screen mode.
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/questions/4939219/wpf-full-screen-on-maximize"/>
        private void ToggleFullScreenMode()
        {
            if (_isFullscreen)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = _lastWindowState;
                _isFullscreen = false;
            }
            else
            {
                _lastWindowState = WindowState;

                WindowStyle = WindowStyle.None;
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Minimized;
                WindowState = WindowState.Maximized;
                _isFullscreen = true;
            }
        }
    }
}