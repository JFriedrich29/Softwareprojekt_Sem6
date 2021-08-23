using System.Collections.Generic;

using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;

namespace QuantumCryptoCram.Presentation.DesignTime
{
    public class EveEncryptionDesignTimeVM
    {
        public EveEncryptionDesignTimeVM()
        {
            FinalKeyBitsWithRelevance =
               new List<FinalKeyBitWithRelevanceViewModel>()
               {
                    new FinalKeyBitWithRelevanceViewModel()
                    {
                        DataBit = true,
                        RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                    },
                    new FinalKeyBitWithRelevanceViewModel()
                    {
                        DataBit = false,
                        RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                    },
                    new FinalKeyBitWithRelevanceViewModel()
                    {
                        DataBit = true,
                        RelevanceType = MeasuredDataKeyRelevanceType.AliceBobMatch,
                    }, new FinalKeyBitWithRelevanceViewModel()
                    {
                        DataBit = true,
                        RelevanceType = MeasuredDataKeyRelevanceType.NoMatch,
                    },
               };
        }

        public List<FinalKeyBitWithRelevanceViewModel> FinalKeyBitsWithRelevance { get; set; }
    }
}