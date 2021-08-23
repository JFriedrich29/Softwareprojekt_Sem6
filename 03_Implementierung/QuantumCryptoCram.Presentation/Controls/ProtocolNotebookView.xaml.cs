using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using QuantumCryptoCram.Presentation.ViewModels.Protocol;

namespace QuantumCryptoCram.Presentation.Controls
{
    /// <summary>
    /// Interaktionslogik für ProtocolNotebookView.xaml.
    /// </summary>
    public partial class ProtocolNotebookView : UserControl
    {
        public ObservableCollection<AliceNotebookEntryViewModel> ItemSource
        {
            get { return (ObservableCollection<AliceNotebookEntryViewModel>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AliceNotebookEntryViewModels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register(
                nameof(ItemSource),
                typeof(ObservableCollection<AliceNotebookEntryViewModel>),
                typeof(ProtocolNotebookView),
                new PropertyMetadata(new ObservableCollection<AliceNotebookEntryViewModel>()));

        public bool IsProtocolInProgress
        {
            get { return (bool)GetValue(IsProtocolInProgressProperty); }
            set { SetValue(IsProtocolInProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProtocolDone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsProtocolInProgressProperty =
            DependencyProperty.Register("IsProtocolInProgress", typeof(bool), typeof(ProtocolNotebookView), new PropertyMetadata(false));

        public bool HasPhotonSent
        {
            get { return (bool)GetValue(HasPhotonSentProperty); }
            set { SetValue(HasPhotonSentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasPhotonSent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasPhotonSentProperty =
            DependencyProperty.Register(nameof(HasPhotonSent), typeof(bool), typeof(ProtocolNotebookView), new PropertyMetadata(false, HasPhotonSent_Changed));

        public ProtocolNotebookView()
        {
            InitializeComponent();
        }

        private static void HasPhotonSent_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Apparently you cannot directly bind the Visibility property in this case for whatever reason:
            // https://stackoverflow.com/questions/15310467/how-to-bind-datagridtemplatecolumn-visibility-to-a-property-outside-of-datagrid
            //
            // Setting the visibilty from code behind is used as a workaround.
            if (d is ProtocolNotebookView view)
            {
                view.PhotonSentColumn.Visibility = view.HasPhotonSent ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg =
                    new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                    {
                        RoutedEvent = MouseWheelEvent,
                        Source = sender,
                    };
                var parent = ((Control)sender).Parent as UIElement;
                parent?.RaiseEvent(eventArg);
            }
        }
    }
}