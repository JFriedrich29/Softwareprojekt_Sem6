using System.Windows;
using System.Windows.Controls;

namespace QuantumCryptoCram.Presentation.Views
{
    /// <summary>
    /// Interaktionslogik für PasswordDialogView.xaml.
    /// </summary>
    public partial class PasswordDialogView : Window
    {
        public PasswordDialogView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Exposes the password to the ViewModel.
        /// Does not violate MVVM as no direct link exists between this and the ViewModel.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}