using System.Collections.ObjectModel;
using System.Linq;

namespace QuantumCryptoCram.Domain.Data
{
    public static class NotebookExtensions
    {
        /// <summary>
        /// Ensures the existence of a <see cref="TEntry"/> at index <paramref name="idx"/> in <paramref name="notebook"/>.
        /// </summary>
        /// <param name="notebook">The notebook that should should get indexed.</param>
        /// <param name="idx">The required index.</param>
        /// <returns>The Entry at index <paramref name="idx"/>.</returns>
        /// <typeparam name="TEntry">The concrete type of <see cref="ProtocolRoleNotebookEntry"/>.</typeparam>
        public static TEntry SafelyGetEntry<TEntry>(this ObservableCollection<TEntry> notebook, int idx)
            where TEntry : ProtocolRoleNotebookEntry, new()
        {
            // I.e. if required index is 0 and count is 0 we require 1 new entry
            int missingEntries = (idx - notebook.Count) + 1;
            if (missingEntries > 0)
            {
                for (int i = 0; i < missingEntries; i++)
                {
                    notebook.Add(new TEntry()
                    {
                        Id = notebook.Count,
                    });
                }
            }

            return notebook[idx];
        }

        /// <summary>
        /// Gets the first invalid notebook entry and if none is found a new entry is added
        /// to the end of the list.
        /// </summary>
        /// <param name="notebook">The notebook this action is performed on.</param>
        /// <returns>The first entry found or a new entry.</returns>
        /// <typeparam name="TEntry">The concrete type of <see cref="ProtocolRoleNotebookEntry"/>.</typeparam>
        public static TEntry GetFirstInvalidEntryOrAddNew<TEntry>(this ObservableCollection<TEntry> notebook)
            where TEntry : ProtocolRoleNotebookEntry, new()
        {
            TEntry entry = notebook.FirstOrDefault(e => !e.IsValidEntry());
            if (entry == null)
            {
                entry = new TEntry
                {
                    Id = notebook.Count,
                };
                notebook.Add(entry);
            }

            return entry;
        }
    }
}