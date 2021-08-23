using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.ViewModels.EncryptionTest
{
    public class FinalKeyBitWithRelevanceViewModel : PropertyChangedBase
    {
        private bool? _finalKeyBit;
        private MeasuredDataKeyRelevanceType _relevanceType;
        private bool? _dataBit;

        public bool? FinalKeyBit
        {
            get => _finalKeyBit;

            set
            {
                _finalKeyBit = value;
                OnPropertyChanged();
            }
        }

        public MeasuredDataKeyRelevanceType RelevanceType
        {
            get => _relevanceType;

            set
            {
                _relevanceType = value;
                OnPropertyChanged();
            }
        }

        public bool? DataBit
        {
            get => _dataBit;

            set
            {
                _dataBit = value;
                OnPropertyChanged();
            }
        }
    }
}