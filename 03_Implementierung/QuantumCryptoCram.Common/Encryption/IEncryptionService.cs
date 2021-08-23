using System.Collections;

namespace QuantumCryptoCram.Common.Encryption
{
    /// <summary>
    /// This interface provides methodes to de-/encrypt a list of bools.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// This function encrypts listToEncrypt with key through an XOR cipher.
        /// </summary>
        /// <param name="listToEncrypt">A BitArray that should that will be encrypted.</param>
        /// <param name="key">A BitArray that should be used to encrypt listToEncrypt.</param>
        void Encrypt(ref BitArray listToEncrypt, BitArray key);

        /// <summary>
        /// This function decrypts listToDecrypt with key through an XOR cipher.
        /// </summary>
        /// <param name="listToDecrypt">A BitArray that should be decrypted.</param>
        /// <param name="key">A BitArray that should be used to decrypt listToDecrypt.</param>
        void Decrypt(ref BitArray listToDecrypt, BitArray key);
    }
}