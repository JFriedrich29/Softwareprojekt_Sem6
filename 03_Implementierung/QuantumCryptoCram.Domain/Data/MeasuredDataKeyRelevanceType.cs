namespace QuantumCryptoCram.Domain.Data
{
    /// <summary>
    /// Enumeration types that define, how relevant the measured data is for Eve.
    /// </summary>
    public enum MeasuredDataKeyRelevanceType
    {
        /// <summary>
        /// The polarisations of Bob and Alice have matched, and Eve measured with the same polarisation.
        /// </summary>
        AliceBobEveMatch,

        /// <summary>
        /// The polarisations of Bob and Alice have matched, but Eve measured with a different polarisation.
        /// </summary>
        AliceBobMatch,

        /// <summary>
        /// The polarisations of Bob and Alice have matched, but because of the prekey comparison
        /// they discarded the data bit.
        /// </summary>
        AliceBobMatchButDiscarded,

        /// <summary>
        /// The polarisations of Bob and Alice have not matched, so they discarded the data bit.
        /// </summary>
        NoMatch,
    }
}