using System.Collections.Generic;

using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Domain.Communication
{
    /// <summary>
    /// Message containing the polarisations used for sending/receiving.
    /// </summary>
    public class PolarisationUsedMessage : PublicMessage
    {
        /// <summary>
        /// Gets a dictionary containing the id of a bit and the polarisation used to create it.
        /// </summary>
        public Dictionary<int, Polarisation> Polarisations { get; private set; }

        /// <summary>
        /// Gets the role that publishes its polarisations via this message.
        /// </summary>
        public ProtocolRoleType PolarisationPublisher { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarisationUsedMessage"/> class.
        /// </summary>
        /// <param name="publisher">The role publishing its polarisations.</param>
        /// <param name="polarisations">The polarisations to be sent.</param>
        public PolarisationUsedMessage(ProtocolRoleType publisher, Dictionary<int, Polarisation> polarisations)
        : base()
        {
            PolarisationPublisher = publisher;
            Polarisations = polarisations;
        }
    }
}