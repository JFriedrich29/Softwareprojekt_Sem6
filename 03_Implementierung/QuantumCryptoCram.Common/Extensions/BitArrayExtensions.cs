using System;
using System.Collections;
using System.Text;

namespace QuantumCryptoCram.Common.Extensions
{
    /// <summary>
    /// Class containing <see cref="BitArray"/> helper methods.
    /// </summary>
    public static class BitArrayExtensions
    {
        /// <summary>
        /// Convert the <see cref="BitArray"/> into an array of <see cref="byte"/>s.
        /// </summary>
        /// <param name="bitArray">The BitArray to be converted.</param>
        /// <returns>The <see cref="byte"/>-representation of the <see cref="BitArray"/>.</returns>
        public static byte[] ToBytes(this BitArray bitArray)
        {
            if (bitArray.Length == 0)
            {
                return Array.Empty<byte>();
            }

            int num_bytes = ((bitArray.Length - 1) / 8) + 1;
            byte[] bytes = new byte[num_bytes];
            bitArray.CopyTo(bytes, 0);
            return bytes;
        }

        /// <summary>
        /// Try to interpret raw bits as an ASCII string.
        /// </summary>
        /// <param name="bitArray">The bits.</param>
        /// <returns>The resulting string.</returns>
        public static string ToASCIIString(this BitArray bitArray)
        {
            return Encoding.ASCII.GetString(bitArray.ToBytes());
        }
    }
}