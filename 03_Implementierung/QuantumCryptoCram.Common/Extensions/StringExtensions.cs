using System.Collections;
using System.Text;

namespace QuantumCryptoCram.Common.Extensions
{
    /// <summary>
    /// Class containing <see cref="string"/> helper methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a <see cref="string"/> into its ASCII representation stored in a <see cref="BitArray"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to convert.</param>
        /// <returns>The <see cref="BitArray"/> representing the <see cref="string"/> according to ASCII.</returns>
        public static BitArray ToBitArray(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new BitArray(0);
            }
            else
            {
                return new BitArray(Encoding.ASCII.GetBytes(str));
            }
        }
    }
}