using System.ComponentModel;

using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    /// <summary>
    /// Class that wraps a <see cref="TNotebookEntry"/> so UI specific logic and properties can be added.
    /// </summary>
    /// <typeparam name="TNotebookEntry">Type of the wrapped notebook entry model.</typeparam>
    public abstract class ProtocolPartnerNotebookEntryViewModel<TNotebookEntry>
        : ProtocolRoleNotebookEntryViewModel<TNotebookEntry>
        where TNotebookEntry : ProtocolPartnerNotebookEntry
    {
        /// <summary>
        /// Gets called if a Property within the <see cref="TNotebookEntry"/> gets changed.
        /// </summary>
        /// <param name="e">The Property that gets changed.</param>
        protected override void InternalNotebookEntry_PropertyChanged(PropertyChangedEventArgs e)
        {
            string propName = e.PropertyName;

            if (propName == "MyPolarisation" ||
                propName == "PartnerPolarisation" ||
                propName == "WasPreKeySelectionSentOrReceived")
            {
                OnPropertyChanged(nameof(IsPolarisationMatchingCheckboxEnabled));
                OnPropertyChanged(nameof(IsSelectPrekeyForComparisonCheckboxEnabled));
            }

            if (propName == "PreKey" ||
                propName == "PreKeyPartner")
            {
                OnPropertyChanged(nameof(IsSelectPrekeyForComparisonCheckboxEnabled));
                OnPropertyChanged(nameof(IsPreKeyMatchingCheckboxEnabled));
            }

            base.InternalNotebookEntry_PropertyChanged(e);
        }

        /// <summary>
        /// Gets a value indicating whether polarisation matching checkbox is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if PreKeySelection was sent or received AND
        ///   there is a value for my polarisation and the partner polarisation/>; otherwise, <c>false</c>.
        /// </value>
        public bool IsPolarisationMatchingCheckboxEnabled
        {
            get
            {
                if (InternalNotebookEntry.WasPreKeySelectionSentOrReceived)
                    return false;

                if (!InternalNotebookEntry.MyPolarisation.HasValue || !InternalNotebookEntry.PolarisationPartner.HasValue)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether prekey selection checkbox is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if own PreKey./> is not null AND the partner PreKeyPartner is null
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectPrekeyForComparisonCheckboxEnabled
        {
            get
            {
                if (InternalNotebookEntry.PreKey == null)
                    return false;
                if (InternalNotebookEntry.PreKeyPartner != null)
                    return false;
                if (InternalNotebookEntry.WasPreKeySelectionSentOrReceived)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether prekey matching checkbox is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if own PreKey is not null AND
        ///   the partner prekey is not null
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool IsPreKeyMatchingCheckboxEnabled
        {
            get
            {
                if (InternalNotebookEntry.PreKey == null || InternalNotebookEntry.PreKeyPartner == null)
                    return false;

                return true;
            }
        }
    }
}