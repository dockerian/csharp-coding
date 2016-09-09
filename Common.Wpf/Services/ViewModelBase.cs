using System.Collections.Generic;

namespace Common.Wpf.Services
{
    /// <summary>
    /// A derivation of the <see cref="Common.Wpf.Services.VMElement"/> class with an explicit
    /// implementation of <see cref="IRaisePropertyChanged"/>.
    /// </summary>
    /// <remarks>
    /// This object explicitly implements the <see cref="IRaisePropertyChanged"/> interface for sole use with the
    /// extension methods defined in the <see cref="RaisePropertyChangedExtensions"/> class.
    /// </remarks>
    public abstract class ViewModelBase : VMElement, IRaisePropertyChanged
    {
        #region IRaisePropertyChanged Members

        /// <summary>
        /// Raises the property changed event for a single property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> always, since the base class does not provide this information.
        /// </returns>
        bool IRaisePropertyChanged.RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(propertyName);

            // Since the base class doesn't provides us with this info, just indicate true
            var hasHandler = true;
            return hasHandler;
        }

        /// <summary>
        /// Raises the property changed event for a multiple properties.
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <returns>
        ///   <c>true</c> always, since the base class does not provide this information.
        /// </returns>
        bool IRaisePropertyChanged.RaisePropertyChanged(IEnumerable<string> propertyNames)
        {
            this.OnPropertyChanged(propertyNames);

            // Since the base class doesn't provides us with this info, just indicate true
            var hasHandler = true;
            return hasHandler;
        }

        #endregion IRaisePropertyChanged Members
    }
}
