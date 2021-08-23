using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Extensions;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;

namespace QuantumCryptoCram.Presentation.Tests
{
    [TestFixture]
    public class EveEncryptionTestViewModelTestsEveEncryptionTestViewModelTests
    {
        private IEncryptionService _encryptionService;
        private EveEncryptionTestViewModel _eveVM;
        private IEve _eve;
        private INavigationController _nav;
        private IRandomGenerator _ran;

        [SetUp]
        public void Setup()
        {
            // ### Common Arrange
            _encryptionService = new XorEncryptionService();

            _eve = Substitute.For<IEve>();

            _ran = Substitute.For<IRandomGenerator>();
            _nav = Substitute.For<INavigationController>();

            // Default _eveVM Setup with empty notebook
            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>());
            _eve.EditedKey.Returns(new List<bool?>());
            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());
        }

        [Test]
        public void Reset_ShouldResetTheEditableFinalKeys()
        {
            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>()
            {
                new EveNotebookEntry(0)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(1)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.NoMatch,
                },
            });
            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());

            var textVM = new FinalKeyBitWithRelevanceViewModel() { FinalKeyBit = true };
            _eveVM.FinalKeyBitsWithRelevance = new ObservableCollection<FinalKeyBitWithRelevanceViewModel>(
                new List<FinalKeyBitWithRelevanceViewModel>()
                {
                    new FinalKeyBitWithRelevanceViewModel() {FinalKeyBit = true,},
                    textVM
                }
            );

            // Act
            _eveVM.ResetCommand();

            // Assert
            _eveVM.FinalKeyBitsWithRelevance.Should().BeEquivalentTo(new List<FinalKeyBitWithRelevanceViewModel>()
            {
                new FinalKeyBitWithRelevanceViewModel() {
                    DataBit = true,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                    FinalKeyBit = true, },
                new FinalKeyBitWithRelevanceViewModel() {
                    DataBit = true,
                    RelevanceType = MeasuredDataKeyRelevanceType.NoMatch,
                    FinalKeyBit = null,},
            });
        }

        [Test]
        public void ShouldDecryptCorrectly_WhenCipherTextIsSet_AndKeyIsCorrect()
        {
            // Arrange

            // Create an encrypted message
            string expectedMessage = "Hallo";
            var plainTextBits = expectedMessage.ToBitArray();

            var cipherMessageBitArray = (BitArray)plainTextBits.Clone();
            var testKey = new Key(
                Enumerable.Repeat(DataBit.One, 1)
            );
            Key paddedKey = testKey.GetPaddedKey(plainTextBits.Length);

            _encryptionService.Encrypt(ref cipherMessageBitArray, paddedKey.ToBitArray);

            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>()
            {
                new EveNotebookEntry(0)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                }
            });

            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _eveVM.MyEncryptionTestNotebook;

            // ### Act

            _eveVM.CipherText = cipherMessageBitArray;

            // Assert
            _eveVM.PlainText.Should().Be(expectedMessage);
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_LongerOrEqualKeyLength()
        {
            // ### Arrange

            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>()
            {
                new EveNotebookEntry(0)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(1)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(2)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(3)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(4)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(5)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(6)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(7)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(8)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(9)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                }
            });

            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _eveVM.MyEncryptionTestNotebook;

            // ### Act
            _eveVM.CipherText = "T".ToBitArray();

            // ### Assert

            //00101010 CIPHER
            //11111111 KEY
            //11010101 MESSAGE

            encryptionTestNotebook[0].MessageBit.Should().Be(true);
            encryptionTestNotebook[0].CipherBit.Should().Be(false);

            encryptionTestNotebook[1].MessageBit.Should().Be(true);
            encryptionTestNotebook[1].CipherBit.Should().Be(false);

            encryptionTestNotebook[2].MessageBit.Should().Be(false);
            encryptionTestNotebook[2].CipherBit.Should().Be(true);

            encryptionTestNotebook[3].MessageBit.Should().Be(true);
            encryptionTestNotebook[3].CipherBit.Should().Be(false);

            encryptionTestNotebook[4].MessageBit.Should().Be(false);
            encryptionTestNotebook[4].CipherBit.Should().Be(true);

            encryptionTestNotebook[5].MessageBit.Should().Be(true);
            encryptionTestNotebook[5].CipherBit.Should().Be(false);

            encryptionTestNotebook[6].MessageBit.Should().Be(false);
            encryptionTestNotebook[6].CipherBit.Should().Be(true);

            encryptionTestNotebook[7].MessageBit.Should().Be(true);
            encryptionTestNotebook[7].CipherBit.Should().Be(false);
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_ShorterKey()
        {
            // ### Arrange
            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>()
            {
                new EveNotebookEntry(0)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(1)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(2)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(3)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(4)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                }
            });

            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _eveVM.MyEncryptionTestNotebook;

            // ### Act

            _eveVM.CipherText = "TT".ToBitArray();

            // ### Assert

            //00101010 00101010 CIPHER
            //11111111 11111111 KEY (second byte is repeat)
            //11010101 11010101 MESSAGE

            encryptionTestNotebook[0].MessageBit.Should().Be(true);
            encryptionTestNotebook[0].CipherBit.Should().Be(false);

            encryptionTestNotebook[1].MessageBit.Should().Be(true);
            encryptionTestNotebook[1].CipherBit.Should().Be(false);

            encryptionTestNotebook[2].MessageBit.Should().Be(false);
            encryptionTestNotebook[2].CipherBit.Should().Be(true);

            encryptionTestNotebook[3].MessageBit.Should().Be(true);
            encryptionTestNotebook[3].CipherBit.Should().Be(false);

            encryptionTestNotebook[4].MessageBit.Should().Be(false);
            encryptionTestNotebook[4].CipherBit.Should().Be(true);

            encryptionTestNotebook[5].MessageBit.Should().Be(true);
            encryptionTestNotebook[5].CipherBit.Should().Be(false);

            encryptionTestNotebook[6].MessageBit.Should().Be(false);
            encryptionTestNotebook[6].CipherBit.Should().Be(true);

            encryptionTestNotebook[7].MessageBit.Should().Be(true);
            encryptionTestNotebook[7].CipherBit.Should().Be(false);

            encryptionTestNotebook[8].MessageBit.Should().Be(true);
            encryptionTestNotebook[8].CipherBit.Should().Be(false);

            encryptionTestNotebook[9].MessageBit.Should().Be(true);
            encryptionTestNotebook[9].CipherBit.Should().Be(false);

            encryptionTestNotebook[10].MessageBit.Should().Be(false);
            encryptionTestNotebook[10].CipherBit.Should().Be(true);

            encryptionTestNotebook[11].MessageBit.Should().Be(true);
            encryptionTestNotebook[11].CipherBit.Should().Be(false);

            encryptionTestNotebook[12].MessageBit.Should().Be(false);
            encryptionTestNotebook[12].CipherBit.Should().Be(true);

            encryptionTestNotebook[13].MessageBit.Should().Be(true);
            encryptionTestNotebook[13].CipherBit.Should().Be(false);

            encryptionTestNotebook[14].MessageBit.Should().Be(false);
            encryptionTestNotebook[14].CipherBit.Should().Be(true);

            encryptionTestNotebook[15].MessageBit.Should().Be(true);
            encryptionTestNotebook[15].CipherBit.Should().Be(false);
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_FarShorterKey()
        {
            // ### Arrange
            _eve.MyNotebook.Returns(new ObservableCollection<EveNotebookEntry>()
            {
                new EveNotebookEntry(0)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(1)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                },
                new EveNotebookEntry(2)
                {
                    MyData = DataBit.One,
                    RelevanceType = MeasuredDataKeyRelevanceType.AliceBobEveMatch,
                }
            });

            _eveVM = new EveEncryptionTestViewModel(_eve, _encryptionService, () => _ran, _nav, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _eveVM.MyEncryptionTestNotebook;

            // ### Act

            _eveVM.CipherText = "TT".ToBitArray();

            // ### Assert

            //00101010 00101010 CIPHER
            //11111111 11111111 KEY (everything after 3rd bit is repeat)
            //11010101 11010101 MESSAGE

            encryptionTestNotebook[0].MessageBit.Should().Be(true);
            encryptionTestNotebook[0].CipherBit.Should().Be(false);

            encryptionTestNotebook[1].MessageBit.Should().Be(true);
            encryptionTestNotebook[1].CipherBit.Should().Be(false);

            encryptionTestNotebook[2].MessageBit.Should().Be(false);
            encryptionTestNotebook[2].CipherBit.Should().Be(true);

            encryptionTestNotebook[3].MessageBit.Should().Be(true);
            encryptionTestNotebook[3].CipherBit.Should().Be(false);

            encryptionTestNotebook[4].MessageBit.Should().Be(false);
            encryptionTestNotebook[4].CipherBit.Should().Be(true);

            encryptionTestNotebook[5].MessageBit.Should().Be(true);
            encryptionTestNotebook[5].CipherBit.Should().Be(false);

            encryptionTestNotebook[6].MessageBit.Should().Be(false);
            encryptionTestNotebook[6].CipherBit.Should().Be(true);

            encryptionTestNotebook[7].MessageBit.Should().Be(true);
            encryptionTestNotebook[7].CipherBit.Should().Be(false);

            encryptionTestNotebook[8].MessageBit.Should().Be(true);
            encryptionTestNotebook[8].CipherBit.Should().Be(false);

            encryptionTestNotebook[9].MessageBit.Should().Be(true);
            encryptionTestNotebook[9].CipherBit.Should().Be(false);

            encryptionTestNotebook[10].MessageBit.Should().Be(false);
            encryptionTestNotebook[10].CipherBit.Should().Be(true);

            encryptionTestNotebook[11].MessageBit.Should().Be(true);
            encryptionTestNotebook[11].CipherBit.Should().Be(false);

            encryptionTestNotebook[12].MessageBit.Should().Be(false);
            encryptionTestNotebook[12].CipherBit.Should().Be(true);

            encryptionTestNotebook[13].MessageBit.Should().Be(true);
            encryptionTestNotebook[13].CipherBit.Should().Be(false);

            encryptionTestNotebook[14].MessageBit.Should().Be(false);
            encryptionTestNotebook[14].CipherBit.Should().Be(true);

            encryptionTestNotebook[15].MessageBit.Should().Be(true);
            encryptionTestNotebook[15].CipherBit.Should().Be(false);
        }

        [Test]
        public void FillWithRandom_ShouldFillMissingEditableFinalKeys()
        {
            _ran.GetRandomBool().Returns(true);

            var textVM = new FinalKeyBitWithRelevanceViewModel() { FinalKeyBit = null };
            _eveVM.FinalKeyBitsWithRelevance = new ObservableCollection<FinalKeyBitWithRelevanceViewModel>(
                new List<FinalKeyBitWithRelevanceViewModel>()
                {
                        new FinalKeyBitWithRelevanceViewModel() {FinalKeyBit = true,},
                        textVM
                }
            );
            // Act
            _eveVM.FillWithRandomBitsCommand();

            // Assert
            textVM.FinalKeyBit.Should().Be(true);
        }
    }
}