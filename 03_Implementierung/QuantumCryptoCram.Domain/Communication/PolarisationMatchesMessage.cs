using System.Collections.Generic;
using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Domain.Communication
{
    /// <summary>
    /// Message revealing where the user receiving the <see cref="PolarisationUsedMessage"/> used the same polarisation.
    /// </summary>
    public class PolarisationMatchesMessage : PublicMessage
    {
        /// <summary>
        /// Gets a List containing the ids of all matching bits.
        /// </summary>
        public List<int> Matches { get; private set; }

        /// <summary>
        /// Gets the role that publishes its polarisation matches via this message.
        /// </summary>
        public ProtocolRoleType PolarisationPublisher { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarisationMatchesMessage"/> class.
        /// </summary>
        /// <param name="publisher">The role publishing its polarisation.</param>
        /// <param name="matches">The ids to be sent.</param>
        public PolarisationMatchesMessage(ProtocolRoleType publisher, List<int> matches)
        : base()
        {
            PolarisationPublisher = publisher;
            Matches = matches;
        }
    }
}