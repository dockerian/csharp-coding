using System.Collections.Generic;
using System.ComponentModel;

namespace Common.Wpf.Services
{
    /// <summary>
    /// Provides a way for extension methods in (see <see cref="RaisePropertyChangedExtensions"/>) to raise
    /// property change notifications from an object.
    /// </summary>
    public interface IRaisePropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises the property changed event for a single property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the <see cref="INotifyPropertyChanged.PropertyChanged"/> event has
        /// any subscribers; <c>false</c> if there the event has no subscribers. <c>true</c> is also
        /// returned in the case that the base implementation does not provide such data. This is the
        /// case with <c>"Common.Wpf.Services.VMElement"</c>.
        /// </returns>
        bool RaisePropertyChanged(string propertyName);

        /// <summary>
        /// Raises the property changed event for a multiple properties.
        /// </summary>
        /// <param name="propertyNames">The names of the properties that have changed.</param>
        /// <returns><c>true</c> if the <see cref="INotifyPropertyChanged.PropertyChanged"/> event has
        /// any subscribers; <c>false</c> if there the event has no subscribers. <c>true</c> is also
        /// returned in the case that the base implementation does not provide such data. This is the
        /// case with <c>"Common.Wpf.Services.VMElement"</c>.
        /// </returns>
        bool RaisePropertyChanged(IEnumerable<string> propertyNames);
    }
}
