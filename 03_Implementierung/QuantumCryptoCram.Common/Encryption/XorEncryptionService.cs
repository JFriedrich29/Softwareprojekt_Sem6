using System;
using System.Collections;

namespace QuantumCryptoCram.Common.Encryption
{
    /// <summary>
    /// An impelemntation of <see cref="IEncryptionService"/>.
    /// </summary>
    public class XorEncryptionService : IEncryptionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XorEncryptionService"/> class.
        /// </summary>
        public XorEncryptionService()
        {
        }

        /// <inheritdoc/>
        public void Decrypt(ref BitArray listToDecrypt, BitArray key)
        {
            if (listToDecrypt.Count == key.Count)
            {
                listToDecrypt = listToDecrypt.Xor(key);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <inheritdoc/>
        public void Encrypt(ref BitArray listToEncrypt, BitArray key)
        {
            if (listToEncrypt.Count == key.Count)
            {
                listToEncrypt = listToEncrypt.Xor(key);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}