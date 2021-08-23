using System;
using QuantumCryptoCram.Domain.Communication;

namespace QuantumCryptoCram.Application.Communication
{
    /// <summary>
    /// A public network class should aggregates <see cref="PublicMessage"/> messages and inform all subscribes of a concrete message type.
    /// </summary>
    public interface IPublicNetwork
    {
        /// <summary>
        /// Publishes a <see cref="PublicMessage"/> to the public network. Every subscriber gets notified about this message.
        /// </summary>
        /// <typeparam name="TMessage">Derived type of a public message.</typeparam>
        /// <param name="sender"> Instance of the message sender.</param>
        /// <param name="message"> The published message.</param>
        void PublishMessage<TMessage>(object sender, TMessage message)
            where TMessage : PublicMessage;

        /// <summary>
        /// Subscribes a message handler to the public network.
        /// </summary>
        /// <typeparam name="TMessage">Derived type of a public message.</typeparam>
        /// <param name="receiver">The instance of the message handler.</param>
        /// <param name="messageHandler">The message handler to be added.</param>
        /// <returns>>The message handler that can be declared inline.</returns>
        Action<TMessage> Subscribe<TMessage>(object receiver, Action<TMessage> messageHandler)
            where TMessage : PublicMessage;

        /// <summary>
        /// Unsubscribe a message handler from the public network.
        /// </summary>
        /// <typeparam name="TMessage">Derived type of a public message.</typeparam>
        /// <param name="receiver">The instance of the message handler.</param>
        /// <param name="messageHandler">The message handler to be removed.</param>
        void Unsubscribe<TMessage>(object receiver, Action<TMessage> messageHandler)
            where TMessage : PublicMessage;
    }
}