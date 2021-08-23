using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// The base class for a notebook entry in which protocol data about a corresponding photon gets stored.
    /// </summary>
    public abstract class ProtocolRoleNotebookEntry : BaseEntry
    {
        private DataBit? _myData;
        private Polarisation? _polarisation;

        /// <summary>
        /// Gets the unique ID for the identification of a notebook entry about one photon.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data bit that the notebook owner knows about the corresponding photon.
        /// </summary>
        public DataBit? MyData
        {
            get
            {
                return _myData;
            }

            set
            {
                _myData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the polarisation that the notebook owner knows about the corresponding photon.
        /// </summary>
        public Polarisation? MyPolarisation
        {
            get
            {
                return _polarisation;
            }

            set
            {
                _polarisation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolRoleNotebookEntry"/> class.
        /// </summary>
        /// <param name="id">The id of the corresponding photon.</param>
        protected ProtocolRoleNotebookEntry(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolRoleNotebookEntry"/> class.
        /// </summary>
        protected ProtocolRoleNotebookEntry()
        {
        }

        /// <summary>
        /// Describes whether this entry has been created properly or only exists as a placeholder because of a message.
        /// </summary>
        /// <returns>True if either MyData or MyPolarisation are not null.</returns>
        public bool IsValidEntry()
        {
            return MyData.HasValue || MyPolarisation.HasValue;
        }
    }
}