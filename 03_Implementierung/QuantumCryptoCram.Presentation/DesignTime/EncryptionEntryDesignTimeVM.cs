using System.Collections.Generic;
using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.DesignTime
{
    /// <summary>
    /// Design aid used to display example data while designing a view.
    /// </summary>
    public class EncryptionEntryDesignTimeVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionEntryDesignTimeVM"/> class with example values.
        /// </summary>
        public EncryptionEntryDesignTimeVM()
        {
            MyEncryptionTestNotebook = new List<EncryptionTestEntry>
            {
                new EncryptionTestEntry
                {
                    KeyBit = true,
                    CipherBit = false,
                    MessageBit = true,
                },
                new EncryptionTestEntry
                {
                    KeyBit = false,
                    CipherBit = false,
                    MessageBit = false,
                },
            };
        }

        /// <summary>
        /// Gets or sets the EncryptionTestNotebook.
        /// </summary>
        public List<EncryptionTestEntry> MyEncryptionTestNotebook { get; set; }
    }
}
