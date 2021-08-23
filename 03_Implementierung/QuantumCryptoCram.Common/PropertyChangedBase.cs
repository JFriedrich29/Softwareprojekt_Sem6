using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuantumCryptoCram.Common
{
    /// <summary> Class that holds the base logic to implement <see cref="INotifyPropertyChanged"./>. </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that gets fired when the corresponding property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Triggers an event informing that the value of the property with given name has been changed.
        /// </summary>
        /// <param name="propertyName">The property name that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}