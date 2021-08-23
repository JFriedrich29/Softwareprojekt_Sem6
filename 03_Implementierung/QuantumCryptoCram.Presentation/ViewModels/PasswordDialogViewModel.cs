using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels
{
    /// <summary>
    /// This is the ViewModel for the PasswordDialogView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class PasswordDialogViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private string _password;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordDialogViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">The WindowManager responsible for creating the dialog.</param>
        public PasswordDialogViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        /// <summary>
        /// Called when the user presses OK.
        /// </summary>
        public void CloseWithSuccess()
        {
            RequestClose(true);
        }

        /// <summary>
        /// Called when the user cancels the dialog.
        /// </summary>
        public void Cancel()
        {
            RequestClose(false);
        }

        /// <summary>
        /// Creates a dialog requesting the user to input a password.
        /// </summary>
        /// <returns>The password when the user presses OK, null otherwise.</returns>
        public string RequestPasswordInput()
        {
            bool? result = _windowManager.ShowDialog(this);

            if (result.GetValueOrDefault(true))
            {
                return Password ?? string.Empty;
            }
            else
            {
                return null;
            }
        }
    }
}
