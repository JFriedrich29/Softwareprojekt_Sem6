using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Notebook entry in which Eve stores protocol data about a corresponding photon.
    /// </summary>
    public class EveNotebookEntry : ProtocolRoleNotebookEntry
    {
        private Polarisation? _polarisationAlice = null;
        private Polarisation? _polarisationBob = null;
        private MeasuredDataKeyRelevanceType _relevanceType = MeasuredDataKeyRelevanceType.NoMatch;

        /// <summary>
        /// Gets or sets the polarisation that Bob used to measure the corresponding photon.
        /// </summary>
        public Polarisation? PolarisationAlice
        {
            get
            {
                return _polarisationAlice;
            }

            set
            {
                _polarisationAlice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the polarisation that Bob used to measure the corresponding photon.
        /// </summary>
        public Polarisation? PolarisationBob
        {
            get
            {
                return _polarisationBob;
            }

            set
            {
                _polarisationBob = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets how relevant the measured data is for Eve.
        /// </summary>
        public MeasuredDataKeyRelevanceType RelevanceType
        {
            get
            {
                return _relevanceType;
            }

            set
            {
                _relevanceType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EveNotebookEntry"/> class.
        /// </summary>
        /// <param name="id">The id of the corresponding photon.</param>
        public EveNotebookEntry(int id)
            : base(id)
        {
        }

        public EveNotebookEntry()
            : base()
        {
        }
    }
}