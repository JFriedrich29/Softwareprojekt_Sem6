using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Extensions;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;

namespace QuantumCryptoCram.Presentation.Tests
{
    [TestFixture]
    public class BobEncryptionTestViewModelTests
    {
        private IEncryptionService _encryptionService;
        private BobEncryptionTestViewModel _bobVM;
        private IBob _bob;
        private INavigationController _nav;

        [SetUp]
        public void Setup()
        {
            // ### Common Arrange
            _encryptionService = new XorEncryptionService();

            _bob = Substitute.For<IBob>();
            _bob.Cipher.Returns(new BitArray(0));

            _nav = Substitute.For<INavigationController>();
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_LongerOrEqualKeyLength()
        {
            // ### Arrange
            var newCipherText = "T".ToBitArray();
            _bob.GetFinalKey().Returns(
                new Key(
                    Enumerable.Repeat(DataBit.One, 10)
                ));
            _bobVM = new BobEncryptionTestViewModel(_nav, _bob, _encryptionService, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _bobVM.MyEncryptionTestNotebook;

            // ### Act
            _bobVM.CipherText = newCipherText;

            // ### Assert
            _bobVM.CipherText.ToASCIIString().Should().Be("T");

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
            var newCipherText = "TT".ToBitArray();

            _bob.GetFinalKey().Returns(
                new Key(
                    Enumerable.Repeat(DataBit.One, 10)
                ));
            _bobVM = new BobEncryptionTestViewModel(_nav, _bob, _encryptionService, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _bobVM.MyEncryptionTestNotebook;

            // ### Act
            _bobVM.CipherText = newCipherText;

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
            var newCipherText = "TT".ToBitArray();

            _bob.GetFinalKey().Returns(
               new Key(
                   Enumerable.Repeat(DataBit.One, 3)
               ));
            _bobVM = new BobEncryptionTestViewModel(_nav, _bob, _encryptionService, Substitute.For<IDialogViewModelFactory>());
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _bobVM.MyEncryptionTestNotebook;

            // ### Act
            _bobVM.CipherText = newCipherText;

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
    }
}