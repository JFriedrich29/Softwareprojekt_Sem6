using System.Collections.Generic;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Communication
{
    /// <summary>
    /// Message announcing which bits to use for prekey-comparison.
    /// </summary>
    public class PreKeySelectionForComparisonMessage : PublicMessage
    {
        /// <summary>
        /// Gets a list containing the id of each matching bit.
        /// </summary>
        public Dictionary<int, DataBit> SelectedPreKeyBits { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreKeySelectionForComparisonMessage"/> class.
        /// </summary>
        /// <param name="selectedPreKeyBits">The dict of the PreKey bit-ids and values.</param>
        public PreKeySelectionForComparisonMessage(Dictionary<int, DataBit> selectedPreKeyBits)
        : base()
        {
            SelectedPreKeyBits = selectedPreKeyBits;
        }
    }
}