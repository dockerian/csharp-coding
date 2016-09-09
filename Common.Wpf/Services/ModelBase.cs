using System.Collections.Generic;
using System.ComponentModel;

namespace Common.Wpf.Services
{
    /// <summary>
    /// A common, lightweight implementation of <see cref="INotifyPropertyChanged"/> and an explicit
    /// implementation of <see cref="IRaisePropertyChanged"/>.
    /// </summary>
    /// <remarks>
    /// <para>This object explicitly implements the <see cref="IRaisePropertyChanged"/> interface for sole use with the
    /// extension methods defined in the <see cref="RaisePropertyChangedExtensions"/>
    /// class.</para>
    /// <para>Even though this class is named <see cref="ModelBase"/>, it can just as well be used as a lightweight base
    /// class for View Models.
    /// </para>
    /// </remarks>
    public abstract class ModelBase : INotifyPropertyChanged, IRaisePropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChangedHandler += value; }
            remove { propertyChangedHandler -= value; }
        }
        private PropertyChangedEventHandler propertyChangedHandler;

        #endregion INotifyPropertyChanged Members

        #region IRaisePropertyChanged Members

        public void OnPropertyChanged(string propertyName)
        {
            var hasHandler = propertyChangedHandler != null;
            if (hasHandler)
            {
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Raises the property changed event for a single property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="INotifyPropertyChanged.PropertyChanged"/> event has
        /// any subscribers; <c>false</c> if there the event has no subscribers. <c>true</c> is also
        /// returned in the case that the base implementation does not provide such data. This is the
        /// case with <c>"Common.Wpf.Services.VMElement"</c>.
        /// </returns>
        bool IRaisePropertyChanged.RaisePropertyChanged(string propertyName)
        {
            var hasHandler = propertyChangedHandler != null;
            if (hasHandler)
            {
                propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
            return hasHandler;
        }

        /// <summary>
        /// Raises the property changed event for a multiple properties.
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <returns>
        ///   <c>true</c> if the <see cref="INotifyPropertyChanged.PropertyChanged"/> event has
        /// any subscribers; <c>false</c> if there the event has no subscribers. <c>true</c> is also
        /// returned in the case that the base implementation does not provide such data. This is the
        /// case with <c>Zetron.CallTaking.ViewModel.ViewElementBase"</c>, since it derives from
        /// the <c>"Common.Wpf.Services.VMElement"</c> class.
        /// </returns>
        bool IRaisePropertyChanged.RaisePropertyChanged(IEnumerable<string> propertyNames)
        {
            if (propertyNames != null)
            {
                var hasHandler = propertyChangedHandler != null;
                if (hasHandler)
                {
                    foreach (var propertyName in propertyNames)
                    {
                        propertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
                    }
                }

                return hasHandler;
            }

            return false;
        }

        #endregion IRaisePropertyChanged Members
    }
}
