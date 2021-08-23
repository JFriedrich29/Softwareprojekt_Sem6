using System.Collections.Generic;

using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.DesignTime
{
    public class ProtocolAnalysisDesignTimeVm
    {
        /// <summary>
        /// Gets the number of polarisations sent by Alice before actually sending the photons.
        /// </summary>
        public List<Statistic> ProtocolStatistics { get; set; }

        /// <summary>
        /// Gets the number of polarisations sent by Alice before sending the photons that
        /// were used by Eve for measuring the corresponding photons.
        /// </summary>
        public int ExploitedLeakedPolarisationsCount { get; private set; } = 5;

        /// <summary>
        /// Gets the length of the FinalKey.
        /// </summary>
        public int KeyLength { get; private set; } = 12;

        /// <summary>
        /// Gets the factor between the length of the one time pad used for encryption
        /// and the key. If this factor is greater than 1, the key was too short
        /// for the encryption.
        /// </summary>
        public float PaddingFactor { get; private set; } = 21;

        /// <summary>
        /// Gets the amount of Photons that were sent.
        /// </summary>
        public int PhotonsSent { get; private set; } = 10;

        /// <summary>
        /// Gets the number of photons matching between Alice and Bob.
        /// </summary>
        public int PartnerMatchingPolarisationCount { get; private set; } = 42;

        /// <summary>
        /// Gets the number of photons matching between Alice, Eve and Bob.
        /// </summary>
        public int AllMatchingPolarisationCount { get; private set; } = 13;

        /// <summary>
        /// Gets the Length of the PreKey divided by the FinalKey Length.
        /// </summary>
        public float PreKeyToKeyLengthRatio { get; private set; } = 1.5f;

        /// <summary>
        /// Gets a value indicating whether Alice detected eve.
        /// </summary>
        public bool AliceDetectedEve { get; private set; } = false;

        /// <summary>
        /// Gets a value indicating whether Alice detected eve.
        /// </summary>
        public bool BobDetectedEve { get; private set; } = true;

        public ProtocolAnalysisDesignTimeVm()
        {
            ProtocolStatistics = new List<Statistic>()
            {
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
                new Statistic()
                {
                    Name = "LeakedPolarisationsCount",
                    Description = "LeakedPol",
                    Value = 10.ToString(),
                },
            };
        }
    }
}