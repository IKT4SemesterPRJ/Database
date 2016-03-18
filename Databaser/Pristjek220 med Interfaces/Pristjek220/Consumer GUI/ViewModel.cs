using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutoComplete;
using Consumer;
using Consumer_GUI.Annotations;
using Pristjek220Data;

namespace Consumer_GUI
{
    class ViewModel : INotifyPropertyChanged
    {
        readonly Model _model = new Model();
        readonly AutoComplete _autoComplete = new AutoComplete();
        private string oldtext = string.Empty;

        #region Commands
        ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(SearchAfterProduct)); }
        }

        private void SearchAfterProduct()
        {
            var unit = new UnitOfWork(new DataContext());
            IConsumer _user = new Consumer.Consumer(unit);

            string product = _model.ProductName;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                System.Windows.MessageBox.Show($"Det er billigst i {_model.ProductName} {store.StoreName}", "Billigste forretning", MessageBoxButton.OK);
            else
                System.Windows.MessageBox.Show($" hej {_model.ProductName}", "Billigste forretning", MessageBoxButton.OK);
        }

        ICommand _populatingCommand;
        public ICommand PopulatingCommand
        {
            get { return _populatingCommand ?? (_populatingCommand = new RelayCommand(PopulatingList)); }
        }


        private void PopulatingList()
        {
            UnitOfWork unit = new UnitOfWork(new DataContext());
            IAutocomplete autocomplete = new Autocomplete(unit);
            _autoComplete.AutoCompleteList?.Clear(); // not equal null
            foreach (var item in autocomplete.AutoCompleteProduct(_model.ProductName))
            {
                _autoComplete.AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }


        ICommand _illegalSignCommand;
        public ICommand IllegalSignCommand
        {
            get { return _illegalSignCommand ?? (_illegalSignCommand = new RelayCommand(IllegalSign)); }
        }


        private void IllegalSign()
        {
            if (!ProductName.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                System.Windows.MessageBox.Show(
                "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                ProductName = oldtext;
            }
        }

        public string ProductName
        {
            get { return _model.ProductName; }
            set
            {
                oldtext = _model.ProductName;
                _model.ProductName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> AutoCompleteList
        {
            get{ return _autoComplete.AutoCompleteList; }
            set
            {
                AutoCompleteList = value;
                OnPropertyChanged("AutoCompleteList");
            }
        } 
        
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class Model
    {
        public string ProductName { set; get; }
    }

    class AutoComplete
    {
        public ObservableCollection<string> AutoCompleteList = new ObservableCollection<string>();
    }
}
