using System.Collections.ObjectModel;
using System.Linq;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;
using QuantumCryptoCram.Presentation.ViewModels;
using QuantumCryptoCram.Presentation.ViewModels.EncryptionTest;

namespace QuantumCryptoCram.Presentation.Tests
{
    [TestFixture]
    public class AliceEncryptionTestViewModelTests
    {
        private IEncryptionService _encryptionService;
        private AliceEncryptionTestViewModel _aliceVM;
        private IAlice _alice;
        private INavigationController _nav;
        private IDialogViewModelFactory _dialogViewModelFactory;

        [SetUp]
        public void Setup()
        {
            // ### Common Arrange
            _encryptionService = new XorEncryptionService();

            _alice = Substitute.For<IAlice>();

            _nav = Substitute.For<INavigationController>();

            _dialogViewModelFactory = Substitute.For<IDialogViewModelFactory>();
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_LongerOrEqualKeyLength()
        {
            // ### Arrange
            string newPlainText = "T";
            _alice.GetFinalKey().Returns(
                new Key(
                    Enumerable.Repeat(DataBit.One, 10)
                ));
            // Instantiate alice vm after alice notebook is set up, because we read the current final key from it.
            _aliceVM = new AliceEncryptionTestViewModel(_alice, _encryptionService, _nav, _dialogViewModelFactory);
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _aliceVM.MyEncryptionTestNotebook;

            // ### Act
            _aliceVM.ChosenPlainText = newPlainText;

            // ### Assert
            _aliceVM.ChosenPlainText.Should().Be("T");

            encryptionTestNotebook[0].MessageBit.Should().Be(false);
            encryptionTestNotebook[0].CipherBit.Should().Be(true);

            encryptionTestNotebook[1].MessageBit.Should().Be(false);
            encryptionTestNotebook[1].CipherBit.Should().Be(true);

            encryptionTestNotebook[2].MessageBit.Should().Be(true);
            encryptionTestNotebook[2].CipherBit.Should().Be(false);

            encryptionTestNotebook[3].MessageBit.Should().Be(false);
            encryptionTestNotebook[3].CipherBit.Should().Be(true);

            encryptionTestNotebook[4].MessageBit.Should().Be(true);
            encryptionTestNotebook[4].CipherBit.Should().Be(false);

            encryptionTestNotebook[5].MessageBit.Should().Be(false);
            encryptionTestNotebook[5].CipherBit.Should().Be(true);

            encryptionTestNotebook[6].MessageBit.Should().Be(true);
            encryptionTestNotebook[6].CipherBit.Should().Be(false);

            encryptionTestNotebook[7].MessageBit.Should().Be(false);
            encryptionTestNotebook[7].CipherBit.Should().Be(true);
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_ShorterKey()
        {
            // ### Arrange

            string newPlainText = "TT";

            _alice.GetFinalKey().Returns(
                new Key(
                    Enumerable.Repeat(DataBit.One, 10)
                ));
            // Instantiate alice vm after alice notebook is set up, because we read the current final key from it.
            _aliceVM = new AliceEncryptionTestViewModel(_alice, _encryptionService, _nav, _dialogViewModelFactory);
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _aliceVM.MyEncryptionTestNotebook;

            // ### Act

            _aliceVM.ChosenPlainText = newPlainText;

            // ### Assert

            _aliceVM.ChosenPlainText.Should().Be("TT");

            //0010101000101010

            encryptionTestNotebook[0].MessageBit.Should().Be(false);
            encryptionTestNotebook[0].CipherBit.Should().Be(true);

            encryptionTestNotebook[1].MessageBit.Should().Be(false);
            encryptionTestNotebook[1].CipherBit.Should().Be(true);

            encryptionTestNotebook[2].MessageBit.Should().Be(true);
            encryptionTestNotebook[2].CipherBit.Should().Be(false);

            encryptionTestNotebook[3].MessageBit.Should().Be(false);
            encryptionTestNotebook[3].CipherBit.Should().Be(true);

            encryptionTestNotebook[4].MessageBit.Should().Be(true);
            encryptionTestNotebook[4].CipherBit.Should().Be(false);

            encryptionTestNotebook[5].MessageBit.Should().Be(false);
            encryptionTestNotebook[5].CipherBit.Should().Be(true);

            encryptionTestNotebook[6].MessageBit.Should().Be(true);
            encryptionTestNotebook[6].CipherBit.Should().Be(false);

            encryptionTestNotebook[7].MessageBit.Should().Be(false);
            encryptionTestNotebook[7].CipherBit.Should().Be(true);

            encryptionTestNotebook[8].MessageBit.Should().Be(false);
            encryptionTestNotebook[8].CipherBit.Should().Be(true);

            encryptionTestNotebook[9].MessageBit.Should().Be(false);
            encryptionTestNotebook[9].CipherBit.Should().Be(true);

            encryptionTestNotebook[10].MessageBit.Should().Be(true);
            encryptionTestNotebook[10].CipherBit.Should().Be(false);

            encryptionTestNotebook[11].MessageBit.Should().Be(false);
            encryptionTestNotebook[11].CipherBit.Should().Be(true);

            encryptionTestNotebook[12].MessageBit.Should().Be(true);
            encryptionTestNotebook[12].CipherBit.Should().Be(false);

            encryptionTestNotebook[13].MessageBit.Should().Be(false);
            encryptionTestNotebook[13].CipherBit.Should().Be(true);

            encryptionTestNotebook[14].MessageBit.Should().Be(true);
            encryptionTestNotebook[14].CipherBit.Should().Be(false);

            encryptionTestNotebook[15].MessageBit.Should().Be(false);
            encryptionTestNotebook[15].CipherBit.Should().Be(true);
        }

        [Test]
        public void UpdateMessageAndCipher_ShouldUpdateMessageBitsAndCipherBits_FarShorterKey()
        {
            // ### Arrange

            string newPlainText = "TT";

            _alice.GetFinalKey().Returns(
               new Key(
                   Enumerable.Repeat(DataBit.One, 3)
               ));
            // Instantiate alice vm after alice notebook is set up, because we read the current final key from it.
            _aliceVM = new AliceEncryptionTestViewModel(_alice, _encryptionService, _nav, _dialogViewModelFactory);
            ObservableCollection<EncryptionTestEntry> encryptionTestNotebook = _aliceVM.MyEncryptionTestNotebook;

            // ### Act

            _aliceVM.ChosenPlainText = newPlainText;

            // ### Assert

            _aliceVM.ChosenPlainText.Should().Be("TT");

            //0010101000101010

            encryptionTestNotebook[0].MessageBit.Should().Be(false);
            encryptionTestNotebook[0].CipherBit.Should().Be(true);

            encryptionTestNotebook[1].MessageBit.Should().Be(false);
            encryptionTestNotebook[1].CipherBit.Should().Be(true);

            encryptionTestNotebook[2].MessageBit.Should().Be(true);
            encryptionTestNotebook[2].CipherBit.Should().Be(false);

            encryptionTestNotebook[3].MessageBit.Should().Be(false);
            encryptionTestNotebook[3].CipherBit.Should().Be(true);

            encryptionTestNotebook[4].MessageBit.Should().Be(true);
            encryptionTestNotebook[4].CipherBit.Should().Be(false);

            encryptionTestNotebook[5].MessageBit.Should().Be(false);
            encryptionTestNotebook[5].CipherBit.Should().Be(true);

            encryptionTestNotebook[6].MessageBit.Should().Be(true);
            encryptionTestNotebook[6].CipherBit.Should().Be(false);

            encryptionTestNotebook[7].MessageBit.Should().Be(false);
            encryptionTestNotebook[7].CipherBit.Should().Be(true);

            encryptionTestNotebook[8].MessageBit.Should().Be(false);
            encryptionTestNotebook[8].CipherBit.Should().Be(true);

            encryptionTestNotebook[9].MessageBit.Should().Be(false);
            encryptionTestNotebook[9].CipherBit.Should().Be(true);

            encryptionTestNotebook[10].MessageBit.Should().Be(true);
            encryptionTestNotebook[10].CipherBit.Should().Be(false);

            encryptionTestNotebook[11].MessageBit.Should().Be(false);
            encryptionTestNotebook[11].CipherBit.Should().Be(true);

            encryptionTestNotebook[12].MessageBit.Should().Be(true);
            encryptionTestNotebook[12].CipherBit.Should().Be(false);

            encryptionTestNotebook[13].MessageBit.Should().Be(false);
            encryptionTestNotebook[13].CipherBit.Should().Be(true);

            encryptionTestNotebook[14].MessageBit.Should().Be(true);
            encryptionTestNotebook[14].CipherBit.Should().Be(false);

            encryptionTestNotebook[15].MessageBit.Should().Be(false);
            encryptionTestNotebook[15].CipherBit.Should().Be(true);
        }
    }
}