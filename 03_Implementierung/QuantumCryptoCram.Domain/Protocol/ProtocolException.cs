using System;

namespace QuantumCryptoCram.Domain.Protocol
{
    public class ProtocolException : Exception
    {
        public ProtocolException()
        {
        }

        public ProtocolException(string message)
            : base(message)
        {
        }

        public ProtocolException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}