namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Notebook entry in which Bob stores protocol data about a corresponding photon.
    /// </summary>
    public class BobNotebookEntry : ProtocolPartnerNotebookEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BobNotebookEntry"/> class.
        /// </summary>
        /// <param name="id">The id of the corresponding photon.</param>
        public BobNotebookEntry(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BobNotebookEntry"/> class.
        /// </summary>
        public BobNotebookEntry()
            : base()
        {
        }
    }
}