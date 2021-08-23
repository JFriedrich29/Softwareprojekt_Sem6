using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Common.Encryption;
using QuantumCryptoCram.Common.Extensions;
using QuantumCryptoCram.Common.Random;
using QuantumCryptoCram.Domain.Data;
using QuantumCryptoCram.Domain.Protocol;
using QuantumCryptoCram.Domain.Quantum;
using QuantumCryptoCram.Presentation.Utility.Navigation;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels.EncryptionTest
{
    /// <summary>
    /// This is the ViewModel for the EveEncryptionTestView.
    /// </summary>
    /// <seealso cref="Stylet.Screen" />
    public class EveEncryptionTestViewModel : Screen
    {
        private readonly IEve _eve;
        private readonly INavigationController _navigationController;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IEncryptionService _encryptionService;
        private readonly IDialogViewModelFactory _dialogFactory;

        private string _plainText;
        private BitArray _cipherText;
        private ObservableCollection<FinalKeyBitWithRelevanceViewModel> _finalKeyBitsWithRelevance;

        /// <summary>
        /// Gets or sets the content for the final key table that even can edit.
        /// </summary>
        public ObservableCollection<FinalKeyBitWithRelevanceViewModel> FinalKeyBitsWithRelevance
        {
            get => _finalKeyBitsWithRelevance;
            set => SetAndNotify(ref _finalKeyBitsWithRelevance, value);
        }

        /// <summary>
        /// Gets list of<see cref="EncryptionTestEntry"/> representing the notebook where Alice notes down data for testing the encryption.
        /// </summary>
        public ObservableCollection<EncryptionTestEntry> MyEncryptionTestNotebook { get; }

        /// <summary>
        /// Gets or sets the cipher text message.
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
                DecryptCipher();
            }
        }

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
        /// Initializes a new instance of the <see cref="EveEncryptionTestViewModel"/> class.
        /// </summary>
        /// <param name="eve">The eve model class reference.</param>
        /// <param name="encryptionService">A <see cref="IEncryptionService"/> which is used by every protocol role to de-/encrypt messages.</param>
        /// <param name="navigationController">Interface that Stylet provides to navigate between views.</param>
        /// <param name="randomGenerator">Interface for randomness.</param>
        /// <param name="dialogFactory">Factory for creating dialogs.</param>
        /// <exception cref="ArgumentNullException">.</exception>
        public EveEncryptionTestViewModel(
            IEve eve,
            IEncryptionService encryptionService,
            Func<IRandomGenerator> randomGenerator,
            INavigationController navigationController,
            IDialogViewModelFactory dialogFactory)
        {
            _eve = eve;
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
            _dialogFactory = dialogFactory;
            _randomGenerator = randomGenerator();
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));

            MyEncryptionTestNotebook = new ObservableCollection<EncryptionTestEntry>();

            UpdateKeyBitsWithRelevance();
            _cipherText = _eve.Cipher;
            DecryptCipher();
        }

        private void UpdateKeyBitsWithRelevance()
        {
            var finalKeyBitsWithRelevance = new List<FinalKeyBitWithRelevanceViewModel>();
            foreach (EveNotebookEntry entry in _eve.MyNotebook)
            {
                var vm = new FinalKeyBitWithRelevanceViewModel()
                {
                    DataBit = entry.MyData == DataBit.One,
                    RelevanceType = entry.RelevanceType,
                };
                if (_eve.EditedKey.Count > 0)
                {
                    vm.FinalKeyBit = _eve.EditedKey[entry.Id];
                }
                else if (entry.RelevanceType == MeasuredDataKeyRelevanceType.AliceBobEveMatch)
                {
                    vm.FinalKeyBit = entry.MyData == DataBit.One;
                }

                finalKeyBitsWithRelevance.Add(vm);
            }

            FinalKeyBitsWithRelevance = new ObservableCollection<FinalKeyBitWithRelevanceViewModel>(finalKeyBitsWithRelevance);
        }

        /// <summary>
        /// When eve edited a final key bit update UI.
        /// </summary>
        public void CellEditedCommand()
        {
            _eve.EditedKey = FinalKeyBitsWithRelevance.Select(e => e.FinalKeyBit).ToList();
            DecryptCipher();
        }

        /// <summary>
        /// Fills the edited final key with random bits.
        /// </summary>
        public void FillWithRandomBitsCommand()
        {
            foreach (FinalKeyBitWithRelevanceViewModel vms in FinalKeyBitsWithRelevance
                .Where(vm => !vm.FinalKeyBit.HasValue &&
                       vm.RelevanceType != MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded &&
                       vm.RelevanceType != MeasuredDataKeyRelevanceType.NoMatch))
            {
                vms.FinalKeyBit = _randomGenerator.GetRandomBool();
            }

            _eve.EditedKey = FinalKeyBitsWithRelevance.Select(e => e.FinalKeyBit).ToList();
            DecryptCipher();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void ResetCommand()
        {
            _eve.EditedKey.Clear();
            UpdateKeyBitsWithRelevance();
            DecryptCipher();
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        /// <value>
        /// The back command.
        /// </value>
        public Action BackCommand => Back;

        /// <summary>
        /// Gets the help command.
        /// </summary>
        /// <value>
        /// The help command.
        /// </value>
        public Action HelpCommand => Help;

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
            docuVM.DisplayLernhilfe(@"Lernhilfe_Eve_Nachricht.md", @"EveNaEn.png");
        }

        /// <summary>
        /// Decrypts the cipher text with the current edited final key.
        /// </summary>
        private void DecryptCipher()
        {
            if (_cipherText == null || _cipherText.Length == 0)
                return;

            // converts the bit string into a bit array for further usage
            BitArray cipherMessageBitArray = _cipherText;

            Key currentFinalKey = GetCurrentEditedFinalKey();
            if (currentFinalKey.KeySize == 0)
                return;

            Key paddedKey = currentFinalKey.GetPaddedKey(cipherMessageBitArray.Length);

            var plainTextBitArray = (BitArray)cipherMessageBitArray.Clone();
            _encryptionService.Decrypt(ref plainTextBitArray, paddedKey.ToBitArray);

            UpdateUi(plainTextBitArray, cipherMessageBitArray, paddedKey);
        }

        /// <summary>
        /// Updates the UI after a final key bit has been edited.
        /// </summary>
        private void UpdateUi(BitArray plainTextBitArray, BitArray cipherMessageBitArray, Key paddedKey)
        {
            MyEncryptionTestNotebook.Clear();
            for (int i = 0; i < plainTextBitArray.Count; i++)
            {
                var entry = new EncryptionTestEntry
                {
                    MessageBit = plainTextBitArray[i],
                    CipherBit = cipherMessageBitArray[i],
                    KeyBit = paddedKey[i],
                };
                MyEncryptionTestNotebook.Add(entry);
            }

            PlainText = plainTextBitArray.ToASCIIString();
        }

        private Key GetCurrentEditedFinalKey()
        {
            return new Key(FinalKeyBitsWithRelevance.Where(e => e.FinalKeyBit.HasValue).Select(e => e.FinalKeyBit.Value));
        }
    }
}