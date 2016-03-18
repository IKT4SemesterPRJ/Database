using System;
using System.Collections.Generic;
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
        Model model = new Model();
        private string oldtext = String.Empty;

        #region Commands
        ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(searchAfterProduct)); }
        }

        public string ProductName
        {
            get { return model.ProductName; }
            set
            {
                if (value.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
                {
                    model.ProductName = value;
                    oldtext = model.ProductName;
                    OnPropertyChanged();
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
                    ProductName = model.ProductName;
                }
            }
        }



        private void searchAfterProduct()
        {
            var unit = new UnitOfWork(new DataContext());
            IConsumer _user = new Consumer.Consumer(unit);
       
            string product = model.ProductName;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                System.Windows.MessageBox.Show($"Det er billigst i {model.ProductName} {store.StoreName}", "Billigste forretning", MessageBoxButton.OK);
            else
                System.Windows.MessageBox.Show($" hej {model.ProductName}", "Billigste forretning", MessageBoxButton.OK);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
