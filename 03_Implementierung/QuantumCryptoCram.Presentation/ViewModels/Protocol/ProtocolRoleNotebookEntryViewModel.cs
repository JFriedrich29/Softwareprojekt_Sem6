using System.ComponentModel;

using QuantumCryptoCram.Common;
using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    /// <summary>
    /// Class that wraps a <see cref="TNotebookEntry"/> so UI specific logic and properties can be added.
    /// </summary>
    /// <typeparam name="TNotebookEntry">Type of the wrapped notebook entry model.</typeparam>
    public abstract class ProtocolRoleNotebookEntryViewModel<TNotebookEntry> : PropertyChangedBase
        where TNotebookEntry : ProtocolRoleNotebookEntry
    {
        private TNotebookEntry _internalNotebookEntry;

        /// <summary>
        /// Gets or sets the notebook entry.
        /// </summary>
        public TNotebookEntry InternalNotebookEntry
        {
            get => _internalNotebookEntry;

            set
            {
                _internalNotebookEntry = value;
                InternalNotebookEntry.PropertyChanged += (sender, eventArgs) => InternalNotebookEntry_PropertyChanged(eventArgs);
                OnPropertyChanged(nameof(InternalNotebookEntry));
            }
        }

        protected virtual void InternalNotebookEntry_PropertyChanged(PropertyChangedEventArgs e)
        {
        }
    }
}