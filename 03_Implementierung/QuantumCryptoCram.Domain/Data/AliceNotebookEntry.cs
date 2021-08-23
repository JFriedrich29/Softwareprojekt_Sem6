namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Notebook entry in which Alice stores protocol data about a corresponding photon.
    /// </summary>
    public class AliceNotebookEntry : ProtocolPartnerNotebookEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AliceNotebookEntry"/> class.
        /// </summary>
        /// <param name="id">The id of the corresponding photon.</param>
        public AliceNotebookEntry(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliceNotebookEntry"/> class.
        /// </summary>
        public AliceNotebookEntry()
            : base()
        {
        }

        private bool _wasPhotonSent = false;

        /// <summary>
        /// Gets or sets a value indicating whether the corresponding photon already has been sent to Bob.
        /// </summary>
        public bool WasPhotonSent
        {
            get
            {
                return _wasPhotonSent;
            }

            set
            {
                _wasPhotonSent = value;
                OnPropertyChanged();
            }
        }
    }
}