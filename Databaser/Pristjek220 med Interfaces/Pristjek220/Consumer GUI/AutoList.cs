using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Consumer_GUI
{
    public class AutoList : ObservableCollection<AutoComplete123>
    {
        public AutoList()
        {
        }
    }

    public class AutoComplete123 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    
        public AutoComplete123(string item)
        {
            autoCompleteString = item;
        }

        public override string ToString()
        {
            return string.Format(autoCompleteString);
        }

        public string autoCompleteString { get; set;}
    }
}