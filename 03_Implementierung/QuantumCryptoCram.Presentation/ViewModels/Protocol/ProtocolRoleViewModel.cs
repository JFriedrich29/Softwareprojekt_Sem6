using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using QuantumCryptoCram.Application.Protocol;
using QuantumCryptoCram.Domain.Data;

using Stylet;

namespace QuantumCryptoCram.Presentation.ViewModels.Protocol
{
    public abstract class ProtocolRoleViewModel<TNotebookEntry, TNotebookEntryViewModel> : Screen
        where TNotebookEntry : ProtocolRoleNotebookEntry, new()
        where TNotebookEntryViewModel : ProtocolRoleNotebookEntryViewModel<TNotebookEntry>, new()
    {
        private readonly IProtocolRole<TNotebookEntry> _protocolRoleModel;
        private bool _isProtocolInProgress;
        private bool _quantumChannelUpdateTrigger;
        private bool _publicChannelUpdateTrigger;

        /// <summary>
        /// Gets the notebook where protocol data is noted down by the protocol roles.
        /// </summary>
        /// <value>
        /// The protocol notebook.
        /// </value>
        public ObservableCollection<TNotebookEntryViewModel> ProtocolNotebook { get; }

        /// <summary>
        /// Gets the internal protocol notebook.
        /// </summary>
        /// <value>
        /// The internal protocol notebook.
        /// </value>
        public ObservableCollection<TNotebookEntry> InternalProtocolNotebook { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the protocol is finished and the UI buttons are locked.
        /// </summary>
        public bool IsProtocolInProgress
        {
            get => _isProtocolInProgress;
            set => SetAndNotify(ref _isProtocolInProgress, value);
        }

        public bool QuantumChannelUpdateTrigger
        {
            get => _quantumChannelUpdateTrigger;
            set => SetAndNotify(ref _quantumChannelUpdateTrigger, value);
        }

        public bool PublicChannelUpdateTrigger
        {
            get => _publicChannelUpdateTrigger;
            set => SetAndNotify(ref _publicChannelUpdateTrigger, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolRoleViewModel{TNotebookEntry, TNotebookEntryViewModel}"/> class.
        /// </summary>
        /// <param name="protocolRoleModel">The protocol role model.</param>
        /// <exception cref="ArgumentNullException">protocolRoleModel.</exception>
        protected ProtocolRoleViewModel(IProtocolRole<TNotebookEntry> protocolRoleModel)
        {
            _protocolRoleModel = protocolRoleModel ?? throw new ArgumentNullException(nameof(protocolRoleModel));
            InternalProtocolNotebook = _protocolRoleModel.MyNotebook;
            ProtocolNotebook = new ObservableCollection<TNotebookEntryViewModel>();

            WrapInternalProtocolNotebook();
        }

        /// <summary>
        /// Wraps the notebook entry protocolRoleModel classes with the matching view protocolRoleModel class,
        /// that holds UI specific properties and logic.
        /// </summary>
        private void WrapInternalProtocolNotebook()
        {
            if (InternalProtocolNotebook == null)
                throw new ArgumentException("No internal protocol notebook that can be wrapped was provided.");

            foreach (TNotebookEntry newEntry in InternalProtocolNotebook)
            {
                WrapNotebookEntry(newEntry);
            }

            // Subscribe for new entries that will be added in the future
            InternalProtocolNotebook.CollectionChanged += InternalProtocolNotebookCollectionChanged;
        }

        /// <summary>
        /// Wraps a notebook entry where protocol data is noted down by the protocol roles.
        /// </summary>
        /// <param name="entryToWrap">The entry to wrap.</param>
        protected virtual void WrapNotebookEntry(TNotebookEntry entryToWrap)
        {
            ProtocolNotebook.Add(new TNotebookEntryViewModel() { InternalNotebookEntry = entryToWrap });
        }

        /// <summary>
        /// Called when the collection of notebook entries change in the bob protocolRoleModel class.
        /// This method wraps all added entries in corresponding view models, so UI specific logic can be added.
        /// </summary>
        /// <param name="sender">Model class reference.</param>
        /// <param name="args"> Args contain information how the list changed. </param>
        /// <exception cref="ArgumentOutOfRangeException">NotifyCollectionChangedAction unknown.</exception>
        protected void InternalProtocolNotebookCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    WrapNotebookEntry(args.NewItems.Cast<TNotebookEntry>().First());

                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void TriggerPublicChannelAnimation()
        {
            PublicChannelUpdateTrigger = false;
            PublicChannelUpdateTrigger = true;
        }

        protected void TriggerQuantumChannelAnimation()
        {
            QuantumChannelUpdateTrigger = false;
            QuantumChannelUpdateTrigger = true;
        }

        /// <summary>
        /// Is called when the view closes.
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            InternalProtocolNotebook.CollectionChanged -= InternalProtocolNotebookCollectionChanged;
        }
    }
}