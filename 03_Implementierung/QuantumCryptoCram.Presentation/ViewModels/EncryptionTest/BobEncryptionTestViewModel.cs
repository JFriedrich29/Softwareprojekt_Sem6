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
    /// This is the ViewModel for the BobEncryptionTestView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class BobEncryptionTestViewModel : Screen
    {
        private readonly INavigationController _navigationController;
        private readonly Key _myFinalKey;
        private readonly IEncryptionService _encryptionService;
        private readonly IDialogViewModelFactory _dialogFactory;
        private readonly IBob _bob;
        private string _plainText;
        private BitArray _cipherText;

        /// <summary>
        /// Gets or sets the text that was entered in the Message to encrypt textbox. On set it updates the message and the cipher in the encryption notebook.
        /// </summary>
        public BitArray CipherText
        {
            get
            {
                return _cipherText;
            }

            set
            {
                _cipherText = value;
                UpdateMessage();
            }
        }

        /// <summary>
        /// Gets list of<see cref="EncryptionTestEntry"/> representing the notebook where Alice notes down data for testing the encryption.
        /// </summary>
        public ObservableCollection<EncryptionTestEntry> MyEncryptionTestNotebook { get; }

        /// <summary>
        /// Gets or sets the ASCII message.
        /// </summary>
        /// <value>
        /// The ASCII message.
        /// </value>
        public string PlainText
        {
            get => _plainText;
            set => SetAndNotify(ref _plainText, value);
        }

        /// <summary>
        /// Gets or set a value indicating whether eve was detected.
        /// </summary>
        public bool DetectedEve
        {
            get => _bob.DetectedEve;

            set => _bob.DetectedEve = value;
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
        /// Initializes a new instance of the <see cref="BobEncryptionTestViewModel"/> class.
        /// </summary>
        /// <param name="navigationController">The navigation controller.</param>
        /// <param name="bob">The alice model class reference.</param>
        /// <param name="encryptionService">A <see cref="IEncryptionService"/> which is used by every protocol role to de-/encrypt messages.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">navigationController.</exception>
        public BobEncryptionTestViewModel(
            INavigationController navigationController,
            IBob bob,
            IEncryptionService encryptionService,
            IDialogViewModelFactory dialogFactory)
        {
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _bob = bob;
            _encryptionService = encryptionService;
            _dialogFactory = dialogFactory;
            MyEncryptionTestNotebook = new ObservableCollection<EncryptionTestEntry>();

            _myFinalKey = bob.GetFinalKey();
            _cipherText = bob.Cipher;
            UpdateMessage();
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
            docuVM.DisplayLernhilfe(@"Lernhilfe_Bob_Nachricht.md", @"BobNaEm.png");
        }

        /// <summary>
        /// This function updates the message- and cipher bit entries in Bob's encryption test notebook.
        /// </summary>
        private void UpdateMessage()
        {
            MyEncryptionTestNotebook.Clear();

            if (_myFinalKey.KeySize > 0)
            {
                Key paddedKey = _myFinalKey.GetPaddedKey(_cipherText.Length);

                var plainTextBitArray = (BitArray)_cipherText.Clone();
                _encryptionService.Decrypt(ref plainTextBitArray, paddedKey.ToBitArray);

                for (int i = 0; i < plainTextBitArray.Count; i++)
                {
                    var entry = new EncryptionTestEntry
                    {
                        MessageBit = plainTextBitArray[i],
                        CipherBit = _cipherText[i],
                        KeyBit = paddedKey[i],
                    };
                    MyEncryptionTestNotebook.Add(entry);
                }

                PlainText = plainTextBitArray.ToASCIIString();
            }
        }
    }
}