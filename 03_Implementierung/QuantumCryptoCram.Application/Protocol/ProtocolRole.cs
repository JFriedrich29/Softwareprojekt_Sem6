using System;
using System.Collections.ObjectModel;

using QuantumCryptoCram.Application.Communication;
using QuantumCryptoCram.Common;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Communication;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Application.Protocol
{
        /// <inheritdoc/
    public abstract class ProtocolRole<TEntry> : IDisposable, IProtocolRole<TEntry>
        where TEntry : ProtocolRoleNotebookEntry, new()
    {
        /// <inheritdoc/
        public ProtocolRoleType Role { get; protected set; }

        /// <inheritdoc/>
        public ObservableCollection<TEntry> MyNotebook { get; }

        /// <inheritdoc/>
        public Key FinalKey { get;  set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Gets the public unencrypted network that protocol participants can use to communicate.
        /// </summary>
        protected IPublicNetwork PublicNetwork { get; }

        /// <summary>
        /// Gets a reference to the <see cref="IRandomGenerator"/> so inheriting classes can use it.
        /// </summary>
        protected IRandomGenerator RandomGenerator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolRole{TNotebookEntry}"/> class.
        /// </summary>
        /// <param name="notebook">A list a protocol participant uses to keep track of protocol information.</param>
        /// <param name="publicNetwork">A public unencrypted network that protocol participants can use to communicate.</param>
        /// <param name="randomGenerator">A <see cref="IRandomGenerator"/> which is used by every protocol role to perform random operations.</param>
        protected ProtocolRole(ObservableCollection<TEntry> notebook, IPublicNetwork publicNetwork, IRandomGenerator randomGenerator)
        {
            MyNotebook = notebook;
            PublicNetwork = publicNetwork;
            RandomGenerator = randomGenerator;
        }

        /// <inheritdoc/>
        public void ProtocolDone()
        {
            PublicNetwork.PublishMessage(this, new RoleProtocolDoneMessage() { Role = Role });
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method (true) or from a finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}