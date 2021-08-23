using System;
using System.Collections.Generic;
using System.Linq;

using QuantumCryptoCram.Domain.Communication;

namespace QuantumCryptoCram.Application.Communication
{
    /// <summary>
    /// Class that aggregates <see cref="PublicMessage"/> messages and informs all subscribes of a concrete message type.
    /// </summary>
    public class PublicNetwork : IPublicNetwork
    {
        /// <summary>
        /// Storage for all registered handlers.
        /// </summary>
        private readonly Dictionary<object, List<Delegate>> _subscribers = new Dictionary<object, List<Delegate>>();

        /// <inheritdoc/>
        public void PublishMessage<TMessage>(object sender, TMessage message)
            where TMessage : PublicMessage
        {
            if (message == null)
            {
                return;
            }

            // Do not send the event to the object publishing the event.
            IEnumerable<List<Delegate>> subscriberHandlers = _subscribers
                .Where(s => s.Key != sender)
                .Select(pair => pair.Value);

            foreach (List<Delegate> handlers in subscriberHandlers)
            {
                foreach (Delegate handler in handlers)
                {
                    if (handler is Action<TMessage>)
                    {
                        var h = handler as Action<TMessage>;
                        h(message);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public Action<TMessage> Subscribe<TMessage>(object receiver, Action<TMessage> messageHandler)
            where TMessage : PublicMessage
        {
            if (messageHandler == null)
            {
                throw new ArgumentNullException(nameof(messageHandler));
            }

            if (_subscribers.ContainsKey(receiver))
            {
                _subscribers[receiver].Add(messageHandler);
            }
            else
            {
                _subscribers.Add(receiver, new List<Delegate> { messageHandler });
            }

            return messageHandler;
        }

        /// <inheritdoc/>
        public void Unsubscribe<TMessage>(object receiver, Action<TMessage> messageHandler)
            where TMessage : PublicMessage
        {
            _subscribers[receiver].RemoveAll(handler => handler == (Delegate)messageHandler);
            if (_subscribers[receiver].Count == 0)
            {
                _subscribers.Remove(receiver);
            }
        }
    }
}