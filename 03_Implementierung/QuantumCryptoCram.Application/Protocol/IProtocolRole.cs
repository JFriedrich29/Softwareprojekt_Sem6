using System.Collections.ObjectModel;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Application.Protocol
{
    /// <summary>
    /// Representation of every role potentially involved in the BB84 protocol.
    /// </summary>
    /// <typeparam name="TEntry"> Defines which concrete type of a notebook entry the child class works with. </typeparam>
    public interface IProtocolRole<TEntry>
        where TEntry : ProtocolRoleNotebookEntry, new()
    {
        /// <summary>
        /// Gets list of <see cref="ProtocolRoleNotebookEntry"/> representing the notebook.
        /// The list needs to be observable, since the roles update it on incoming messages.
        /// </summary>
        ObservableCollection<TEntry> MyNotebook { get; }

        /// <summary>
        /// Gets or sets an enum describing the role.
        /// </summary>
        ProtocolRoleType Role { get; }

        /// <summary>
        /// Gets or sets the final key that the role will use to en-/decrypt the ciphermessage.
        /// </summary>
        Key FinalKey { get; set; }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Called when the role is finished with the protocol.
        /// </summary>
        void ProtocolDone();
    }
}