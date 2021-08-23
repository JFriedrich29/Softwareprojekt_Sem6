using System.Windows;
using System.Windows.Controls;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Presentation.Controls
{
    /// <summary>
    /// Interaktionslogik für PhotonView.xaml.
    /// </summary>
    public partial class PhotonView : UserControl
    {
        /// <summary>
        /// Gets or sets the polarisation.
        /// </summary>
        /// <value>
        /// The polarisation.
        /// </value>
        public Polarisation Polarisation
        {
            get { return (Polarisation)GetValue(PolarisationProperty); }
            set { SetValue(PolarisationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Polarisation. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PolarisationProperty =
            DependencyProperty.Register(nameof(Polarisation), typeof(Polarisation), typeof(PhotonView));

        /// <summary>
        /// Gets or sets the bit.
        /// </summary>
        /// <value>
        /// The bit.
        /// </value>
        public DataBit Bit
        {
            get { return (DataBit)GetValue(BitProperty); }
            set { SetValue(BitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BitProperty =
            DependencyProperty.Register(nameof(Bit), typeof(DataBit), typeof(PhotonView));

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotonView"/> class.
        /// </summary>
        public PhotonView()
        {
            InitializeComponent();
        }
    }
}
