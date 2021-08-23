using System;
using System.Collections;
using System.Collections.ObjectModel;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Extensions;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels.EncryptionTest
{
    /// <summary>
    /// This is the ViewModel for the AliceEncryptionTestView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class AliceEncryptionTestViewModel : Screen
    {
        private readonly INavigationController _navigationController;
        private readonly IEncryptionService _encryptionService;
        private readonly IAlice _alice;
        private readonly IDialogViewModelFactory _dialogFactory;

        private readonly Key _myFinalKey;
        private string _infoText = "Infotext";
        private string _chosenPlainText = string.Empty;
        private BitArray _cipher = new BitArray(0);
        private bool _canSendCipher;
        private int _charactersWithoutPadding;
        private string _paddingFactor;

        /// <summary>
        /// Gets or sets the text that was entered in the Message to encrypt textbox. On set it updates the message and the cipher in the encryption notebook.
        /// </summary>
        public string ChosenPlainText
        {
            get
            {
                return _chosenPlainText;
            }

            set
            {
                _chosenPlainText = value;
                UpdateMessageAndCipher();
            }
        }

        /// <summary>
        /// Gets or sets the information text.
        /// </summary>
        /// <value>
        /// The information text.
        /// </value>
        public string InfoText
        {
            get => _infoText;

            set => SetAndNotify(ref _infoText, value);
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>
        /// The back command.
        /// </value>
        public Action BackCommand
        {
            get => Back;
        }

        /// <summary>
        /// Gets the help command.
        /// </summary>
        /// <value>
        /// The help command.
        /// </value>
        public Action HelpCommand
        {
            get => Help;
        }

        /// <summary>
        /// Gets a value indicating whether the cipher can be sent.
        /// </summary>
        public bool CanSendCipher
        {
            get => _canSendCipher;

            private set => SetAndNotify(ref _canSendCipher, value);
        }

        /// <summary>
        /// Gets or set a value indicating whether eve was detected.
        /// </summary>
        public bool DetectedEve
        {
            get => _alice.DetectedEve;

            set => _alice.DetectedEve = value;
        }

        public int CharactersWithoutPadding
        {
            get
            {
                return _charactersWithoutPadding;
            }

            private set
            {
                _charactersWithoutPadding = value;
            }
        }

        public string PaddingFactor
        {
            get
            {
                return _paddingFactor;
            }

            private set
            {
                SetAndNotify(ref _paddingFactor, value);
            }
        }

        /// <summary>
        /// Gets list of<see cref="EncryptionTestEntry"/> representing the notebook where Alice notes down data for testing the encryption.
        /// </summary>
        public ObservableCollection<EncryptionTestEntry> MyEncryptionTestNotebook { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliceEncryptionTestViewModel"/> class.
        /// </summary>
        /// <param name="alice">The alice model class reference.</param>
        /// <param name="encryptionService">A <see cref="IEncryptionService"/> which is used by every protocol role to de-/encrypt messages.</param>
        /// <param name="navigationController">Interface that Stylet provides to navigate between views..</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        public AliceEncryptionTestViewModel(
            IAlice alice,
            IEncryptionService encryptionService,
            INavigationController navigationController,
            IDialogViewModelFactory dialogFactory)
        {
            _alice = alice;
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _dialogFactory = dialogFactory;
            _encryptionService = encryptionService;

            MyEncryptionTestNotebook = new ObservableCollection<EncryptionTestEntry>();

            _canSendCipher = _alice.PlainText.Length == 0;
            _chosenPlainText = _alice.PlainText;
            _myFinalKey = _alice.GetFinalKey();

            _charactersWithoutPadding = _myFinalKey.KeySize / 8;

            UpdateMessageAndCipher();
        }

        /// <summary>
        /// Navigate back to the requested site.
        /// </summary>
        public void Back() => _navigationController.NavigateToSimulationOverviewView();

        /// <summary>
        /// Methode that gets called when the BackButton is pressed.
        /// </summary>
        public void Help()
        {
            DocumentationDialogViewModel docuVM = _dialogFactory.CreateDocumentationDialogViewModel();
            docuVM.DisplayLernhilfe(@"Lernhilfe_Alice_Nachricht.md", @"AliceNaSe.png");
        }

        /// <summary>
        /// Command for sending the cipher message.
        /// </summary>
        public void SendCipherCommand()
        {
            // Store the plain text for when the view is closed.
            _alice.PlainText = _chosenPlainText;
            _alice.SendCipherMessage(_cipher);
            CanSendCipher = false;
        }

        /// <summary>
        /// This function updates the message- and cipher bit entries in Alice's encryption test notebook.
        /// </summary>
        private void UpdateMessageAndCipher()
        {
            MyEncryptionTestNotebook.Clear();

            if (_myFinalKey.KeySize > 0)
            {
                // converts the bit string into a bit array for further usage
                var plainTextBitArray = _chosenPlainText.ToBitArray();

                PaddingFactor = plainTextBitArray.Count.ToString() + "/" + _myFinalKey.KeySize;

                Key paddedKey = _myFinalKey.GetPaddedKey(plainTextBitArray.Length);

                var cipherMessageBitArray = (BitArray)plainTextBitArray.Clone();
                _encryptionService.Encrypt(ref cipherMessageBitArray, paddedKey.ToBitArray);
                _cipher = cipherMessageBitArray;

                for (int i = 0; i < cipherMessageBitArray.Count; i++)
                {
                    var entry = new EncryptionTestEntry
                    {
                        MessageBit = plainTextBitArray[i],
                        CipherBit = cipherMessageBitArray[i],
                        KeyBit = paddedKey[i],
                    };
                    MyEncryptionTestNotebook.Add(entry);
                }
            }
        }
    }
}