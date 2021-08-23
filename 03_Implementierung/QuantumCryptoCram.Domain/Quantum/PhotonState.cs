namespace QuantumCryptoCram.Domain.Quantum
{
    /// <summary>
    /// Model class describing the state of a photon.
    /// </summary>
    public class PhotonState
    {
        /// <summary>
        /// Gets or sets a value indicating the polarisation of the photon.
        /// </summary>
        public Polarisation Polarisation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data bit stored in the photon.
        /// </summary>
        public DataBit Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotonState"/> class.
        /// </summary>
        /// <param name="data">The data bit of the photon.</param>
        /// <param name="polarisation">The polarisation of the photon.</param>
        public PhotonState(DataBit data, Polarisation polarisation)
        {
            Data = data;
            Polarisation = polarisation;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }

            if (!(obj is PhotonState))
            {
                return false;
            }

            return (Data == ((PhotonState)obj).Data)
                && (Polarisation == ((PhotonState)obj).Polarisation);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Polarisation.GetHashCode() ^ Data.GetHashCode();
        }
    }
}