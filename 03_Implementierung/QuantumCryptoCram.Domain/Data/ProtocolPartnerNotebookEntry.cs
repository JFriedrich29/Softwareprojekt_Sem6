using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Notebook entry for storing protocol data about a corresponding photon, which both
    /// protocol partners, Alice and Bob, have in common.
    /// </summary>
    public class ProtocolPartnerNotebookEntry : ProtocolRoleNotebookEntry
    {
        // Own Polarisation
        private bool _wasPolarisationSent = false;

        // Partner Polarisation
        private Polarisation? _polarisationPartner = null;

        private bool _isPolarisationMatching = false;                       // Selected by user in the UI

        // Own Prekey
        private DataBit? _preKey = null;

        private bool _isPreKeySelectedForComparison = false;                // Selected by user in the UI
        private bool _wasPreKeySelectionSentOrReceived = false;

        // Partner Prekey
        private DataBit? _preKeyPartner = null;

        private bool _isPreKeyMatching = false;                             // Selected by user in the UI
        private bool _wasPreKeyMatchSent = false;

        // Final key
        private DataBit? _finalKey = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolPartnerNotebookEntry"/> class.
        /// </summary>
        /// <param name="id">The id of the corresponding photon.</param>
        public ProtocolPartnerNotebookEntry(int id)
            : base(id)
        {
        }

        #region Own Polarisation

        /// <summary>
        /// Gets or sets a value indicating whether the own polarisation of the corresponding photon already has been sent
        /// to the partner for the polarisation comparison.
        /// We also need this to evaluate the non matching polarisations after a polarisation comparison result message,
        /// because it only contains the matching ids.
        /// </summary>
        public bool WasPolarisationSent
        {
            get
            {
                return _wasPolarisationSent;
            }

            set
            {
                _wasPolarisationSent = value;
                OnPropertyChanged();
            }
        }

        #endregion Own Polarisation

        #region Partner Polarisation

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolPartnerNotebookEntry"/> class.
        /// </summary>
        public ProtocolPartnerNotebookEntry()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the polarisation that the communication partner used to measure the corresponding photon.
        /// Can be null if unknown.
        /// </summary>
        public Polarisation? PolarisationPartner
        {
            get
            {
                return _polarisationPartner;
            }

            set
            {
                _polarisationPartner = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the own polarisation is matching with the polarisation of the
        /// partner.
        /// </summary>
        public bool IsPolarisationMatching
        {
            get
            {
                return _isPolarisationMatching;
            }

            set
            {
                _isPolarisationMatching = value;
                if (value == false) IsPreKeySelectedForComparison = false;
                UpdatePreAndFinalKey();
                OnPropertyChanged();
            }
        }

        #endregion Partner Polarisation

        #region Own Prekey

        /// <summary>
        /// Gets or sets the value of the prekey bit if the corresponding photon is used as prekey bit, else it is null.
        /// </summary>
        public DataBit? PreKey
        {
            get
            {
                return _preKey;
            }

            set
            {
                _preKey = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPreKeyMatching));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data bit of the corresponding photon already has been sent for
        /// the prekey comparison to the partner.
        /// </summary>
        public bool IsPreKeySelectedForComparison
        {
            get
            {
                return _isPreKeySelectedForComparison;
            }

            set
            {
                _isPreKeySelectedForComparison = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the own prekey bit was sent to the partner (for comparison).
        /// If this value is true the entry cannot be used for the final key.
        /// </summary>
        public bool WasPreKeySelectionSentOrReceived
        {
            get
            {
                return _wasPreKeySelectionSentOrReceived;
            }

            set
            {
                _wasPreKeySelectionSentOrReceived = value;
                OnPropertyChanged();
            }
        }

        #endregion Own Prekey

        #region Partner Prekey

        /// <summary>
        /// Gets or sets the corresponding prekey bit of the partner.
        /// Can be null if unknown.
        /// </summary>
        public DataBit? PreKeyPartner
        {
            get
            {
                return _preKeyPartner;
            }

            set
            {
                _preKeyPartner = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPreKeyMatching));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the own prekey bit is matching with the corresponding prekey bit of the partner.
        /// If not verified, it is null.
        /// </summary>
        public bool IsPreKeyMatching
        {
            get
            {
                return _isPreKeyMatching;
            }

            set
            {
                _isPreKeyMatching = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the corresponding prekey bit of the partner.
        /// Can be null if unknown.
        /// </summary>
        public bool WasPreKeyMatchSent
        {
            get
            {
                return _wasPreKeyMatchSent;
            }

            set
            {
                _wasPreKeyMatchSent = value;
                OnPropertyChanged();
            }
        }

        #endregion Partner Prekey

        #region Final key

        /// <summary>
        /// Gets or sets the value of the finalkey bit if the corresponding photon is used as prekey bit, else it is null.
        /// </summary>
        public DataBit? FinalKey
        {
            get
            {
                return _finalKey;
            }

            set
            {
                _finalKey = value;
                OnPropertyChanged();
            }
        }

        #endregion Final key

        /// <summary>
        /// Sets PreKey and FinalKey for this entry.
        /// The FinalKey will not be set, if this entry was used for PreKey comparison.
        /// </summary>
        public void UpdatePreAndFinalKey()
        {
            if (IsPolarisationMatching)
            {
                PreKey = MyData;
                if (!WasPreKeySelectionSentOrReceived) FinalKey = MyData;
            }
            else
            {
                PreKey = null;
                FinalKey = null;
            }
        }
    }
}