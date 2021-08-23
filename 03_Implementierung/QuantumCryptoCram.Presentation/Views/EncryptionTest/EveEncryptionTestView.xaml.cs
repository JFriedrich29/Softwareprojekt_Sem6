using System.Windows.Controls;
using System.Windows.Input;

namespace QuantumCryptoCram.Presentation.Views.EncryptionTest
{
    /// <summary>
    /// Interaktionslogik für BobView.xaml.
    /// </summary>
    public partial class EveEncryptionTestView : UserControl
    {
        public EveEncryptionTestView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Helper method that makes navigating a grid column with arrow keys more comfortable.
        /// </summary>
        private void MyGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int selectedIndex = MyGrid.SelectedIndex;
            if (e.Key == Key.Down)
            {
                MyGrid.CommitEdit();
                selectedIndex++;
                if (selectedIndex > MyGrid.Items.Count)
                    selectedIndex = MyGrid.Items.Count;
            }
            else if (e.Key == Key.Up)
            {
                MyGrid.CommitEdit();
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = 0;
            }

            MyGrid.SelectedIndex = selectedIndex;
        }
    }
}