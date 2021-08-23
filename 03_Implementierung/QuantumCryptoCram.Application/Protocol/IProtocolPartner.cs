using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Interface for view-invoked methods used by every non-illicit partner involved in the quantum key exchange.
    /// </summary>
    /// <typeparam name="TEntry"> Defines which concrete type of a notebook entry the child class works with. </typeparam>
    public interface IProtocolPartner<TEntry> : IProtocolRole<TEntry>
        where TEntry : ProtocolPartnerNotebookEntry, new()
    {
        /// <summary>
        ///  Gets or sets a value indicating whether the protocol partner claims to have detected Eve.
        /// </summary>
        bool DetectedEve { get; set; }

        /// <summary>
        /// Send all the polarisations used so far over the network, which were not published yet.
        /// </summary>
        void SendPolarisations();

        /// <summary>
        /// Send all polarisations matching with the partners polarisations.
        /// </summary>
        void SendPolarisationMatches();

        /// <summary>
        /// Send indices of all bits used for the prekey comparison.
        /// </summary>
        void SendPrekeySelectionForComparison();

        /// <summary>
        /// Send indices of all prekey bits that match with the received prekey bits.
        /// </summary>
        void SendPrekeyMatches();

        /// <summary>
        /// This function selects random Databits and marks them as PreKey.
        /// </summary>
        /// <param name="count">The number of Databits that should be randomly selected.</param>
        void SelectRandomBitsForPreKey(int count);

        /// <summary>
        /// Gets the current concatenated final Key of the "Final Key" notebook column.
        /// </summary>
        /// <returns> the current concatenated final <see cref="Key"/>.</returns>
        Key GetFinalKey();
    }
}