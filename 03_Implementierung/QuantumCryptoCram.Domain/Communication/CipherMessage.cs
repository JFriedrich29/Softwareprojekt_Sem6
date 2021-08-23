using System.Collections;
using System.Collections.Generic;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Communication
{
    /// <summary>
    /// Message containing text encrypted using the finalkey.
    /// </summary>
    public class CipherMessage : PublicMessage
    {
        /// <summary>
        /// Gets the list of booleans representing bits of the ciphertext.
        /// </summary>
        public BitArray Cipher { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherMessage"/> class.
        /// </summary>
        /// <param name="cipher">The bit-representation of the ciphertext to be sent.</param>
        public CipherMessage(BitArray cipher)
        : base()
        {
            Cipher = cipher;
        }
    }
}