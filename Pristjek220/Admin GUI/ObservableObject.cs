using System.ComponentModel;
using System.Runtime.CompilerServices;
using Admin_GUI.Annotations;

namespace Admin_GUI
{
  internal class ObservableObject : INotifyPropertyChanged
    {
      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
}
