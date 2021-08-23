using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Class that stores data for the encryption and decryption of a message.
    /// </summary>
    public class EncryptionTestEntry : BaseEntry
    {
        private bool _keyBit;
        private bool _cipherBit;
        private bool _messageBit;

        /// <summary>
        /// Gets or sets the key bit.
        /// </summary>
        public bool KeyBit
        {
            get
            {
                return _keyBit;
            }

            set
            {
                _keyBit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the message bit.
        /// </summary>
        public bool MessageBit
        {
            get
            {
                return _messageBit;
            }

            set
            {
                _messageBit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the encrypted message bit.
        /// </summary>
        public bool CipherBit
        {
            get
            {
                return _cipherBit;
            }

            set
            {
                _cipherBit = value;
                OnPropertyChanged();
            }
        }
    }
}