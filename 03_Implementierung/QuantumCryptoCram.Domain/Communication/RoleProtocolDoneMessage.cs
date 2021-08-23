using QuantumCryptoCram.Domain.Protocol;

namespace QuantumCryptoCram.Domain.Communication
{
    public class RoleProtocolDoneMessage : PublicMessage
    {
        /// <summary>
        /// Gets or sets the role that finished its protocol.
        /// </summary>
        public ProtocolRoleType Role
        {
            get;
            set;
        }
    }
}
