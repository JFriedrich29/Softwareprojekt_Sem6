using System;
using System.Collections;
using System.Collections.Generic;

using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Protocol
{
    /// <summary>
    /// Model class for holding key data and information about the key.
    /// </summary>
    public class Key
    {
        private readonly List<bool> _keyBits;

        /// <summary>
        /// Initializes a new instance of the <see cref="Key"/> class.
        /// Default constructor.
        /// </summary>
        public Key()
        {
            _keyBits = new List<bool>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Key"/> class.
        /// </summary>
        /// <param name="keyBits"> The key bit sequence. </param>
        public Key(IEnumerable<bool> keyBits)
            : this()
        {
            _keyBits.AddRange(keyBits);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Key"/> class.
        /// </summary>
        /// <param name="keyBits"> The key bit sequence. </param>
        public Key(IEnumerable<DataBit> keyBits)
            : this()
        {
            foreach (DataBit bit in keyBits)
            {
                AppendBit(bit);
            }
        }

        /// <summary>
        /// Overloads the index operator for the Key datatype.
        /// </summary>
        /// <param name="index">The index of the keybit that one wants to access.</param>
        /// <returns>The vale of the keybit at index.</returns>
        public bool this[int index]
        {
            get
            {
                return KeyBits[index];
            }
        }

        /// <summary>
        /// Gets wrapped bool list representing the key bits.
        /// </summary>
        public List<bool> KeyBits => _keyBits;

        /// <summary>
        /// Gets the current size of the key bit sequence.
        /// </summary>
        public int KeySize
        {
            get { return _keyBits.Count; }
        }

        /// <summary>
        /// Adds a new bit to the key bit sequence.
        /// </summary>
        /// <param name="bit"> The bit to add. </param>
        public void AppendBit(bool bit)
        {
            _keyBits.Add(bit);
        }

        /// <summary>
        /// Adds a new bit to the key bit sequence.
        /// </summary>
        /// <param name="bit"> The bit to add. </param>
        public void AppendBit(DataBit bit)
        {
            _keyBits.Add(bit == DataBit.One);
        }

        /// <summary>
        /// Adds multiple bits in order to the key bit sequence.
        /// </summary>
        /// <param name="bits"> The key bit sequence to add. </param>
        public void AppendBits(IEnumerable<DataBit> bits)
        {
            foreach (DataBit bit in bits)
            {
                AppendBit(bit);
            }
        }

        /// <summary>
        /// Gets the key bit sequence as bit array.
        /// </summary>
        public BitArray ToBitArray
        {
            get
            {
                return new BitArray(_keyBits.ToArray());
            }
        }

        /// <summary>
        /// Uses a subsection of the current key if the required length is less than the key length.
        /// Uses the full key if the size matches the required size.
        /// Uses a key, that was possibly multiple times padded with the current key to fit the required size.
        /// </summary>
        /// <param name="neededLength">Length that specifies if the key needs padding or a subsection can be used. </param>
        /// <returns>A new <see cref="Key"/> instance, that holds the specified size key bits.</returns>
        public Key GetPaddedKey(int neededLength)
        {
            if (KeySize == 0)
                throw new Exception("Cant create padded key with a key size of 0");

            // Check if only a subsection of the key is necessary else key needs padding
            if (neededLength < KeySize)
                return new Key(_keyBits.GetRange(0, neededLength));

            var paddedKey = new Key();
            // pad with as much integer multiples of the original key as possible
            while (paddedKey.KeySize < neededLength - KeySize)
            {
                paddedKey._keyBits.AddRange(_keyBits);
            }

            // For the last 'n' missing bits add the first 'n' bits of the original key
            int missingBitsCount = neededLength - paddedKey.KeySize;
            paddedKey._keyBits.AddRange(_keyBits.GetRange(0, missingBitsCount));

            return paddedKey;
        }
    }
}