using System.ComponentModel;
using System.Runtime.CompilerServices;
using Administration_GUI.Annotations;

namespace Administration_GUI
{
    /// <summary>
    ///     Class is used to include INotifyPropertyChanged in other classes, by inheriting from this class,
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        ///     Is an event that is used when the Property is Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The method that is called when you want to notify the gui when there has been and update on the attribute 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}