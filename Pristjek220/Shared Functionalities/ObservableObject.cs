using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharedFunctionalities.Annotations;

namespace SharedFunctionalities
{
    /// <summary>
    ///     This class is used to make other classes inherit INotifyPropertyChanged
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        ///     The even that is used when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     This method is used when you want to notify everone that uses it that it has updated
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}