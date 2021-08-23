using System.Collections.Generic;

namespace QuantumCryptoCram.Domain.Communication
{
    /// <summary>
    /// Message announcing the results of the prekey-comparison.
    /// </summary>
    public class PreKeyComparisonResultMessage : PublicMessage
    {
        /// <summary>
        /// Gets a list of all bit-ids where the partners values matched our own.
        /// </summary>
        public List<int> Matches { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreKeyComparisonResultMessage"/> class.
        /// </summary>
        /// <param name="matches">List of ids for matching bits to be sent.</param>
        public PreKeyComparisonResultMessage(List<int> matches)
        : base()
        {
            Matches = matches;
        }
    }
}