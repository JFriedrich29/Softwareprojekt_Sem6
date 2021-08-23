namespace QuantumCryptoCram.Domain.Config
{
    /// <summary>
    /// A class that stores the available options for the simulation.
    /// </summary>
    public class SimulationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether Eve is able to
        /// clone photons. This feature is used for demonstrating
        /// how the protocol would break, if Eve had this power. In
        /// the real world cloning photons impossible.
        /// </summary>
        public bool IsPhotonCloningEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Eve role is
        /// activated. When deactivated, the simulation just starts with Alice and Bob.
        /// </summary>
        public bool IsEveActive { get; set; }
    }
}