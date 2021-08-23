using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Presentation.Controls
{
    /// <summary>
    /// Interaktionslogik für PolarisatioView.xaml.
    /// </summary>
    public partial class PolarisationView : UserControl
    {
        /// <summary>
        /// Gets or sets the polarisation.
        /// </summary>
        /// <value>
        /// The polarisation.
        /// </value>
        public Polarisation? Polarisation
        {
            get { return (Polarisation)GetValue(PolarisationProperty); }
            set { SetValue(PolarisationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Polarisation. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PolarisationProperty =
            DependencyProperty.Register(nameof(Polarisation), typeof(Polarisation?), typeof(PolarisationView), new PropertyMetadata(null, Polarisation_Changed));

        private static void Polarisation_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PolarisationView view)
            {
                if (e.NewValue is Polarisation polarisation)
                {
                    if (polarisation == Domain.Quantum.Polarisation.Rectilinear)
                    {
                        view.PolarisationIcon.Kind = PackIconKind.ArrowAll;
                        view.PolarisationIcon.Foreground = Brushes.Blue;
                    }
                    else
                    {
                        view.PolarisationIcon.Kind = PackIconKind.ArrowExpandAll;
                        view.PolarisationIcon.Foreground = Brushes.Red;
                    }

                    view.PolarisationIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    view.PolarisationIcon.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarisationView"/> class.
        /// </summary>
        public PolarisationView()
        {
            InitializeComponent();
        }
    }
}