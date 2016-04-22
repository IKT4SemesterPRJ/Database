using System.Collections.ObjectModel;
using System.Windows.Input;
using Administration;
using Administration_GUI;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI.User_Controls
{
    class ChangePriceModel : ObservableObject, IPageViewModel
    {
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private Store _store;
        private ICommand _changeProductPriceInStoreDatabaseCommand;
        private ICommand _populatingChangePriceCommand;
        private ICommand _illegalSignChangePriceCommand;
        private ICommand _enterPressedCommand;


        private string _oldtext = string.Empty;

        public ChangePriceModel(Store store)
        {
            _store = store;
            _manager = new Storemanager(new UnitOfWork(new DataContext()), _store);
            _autocomplete = new SharedFunctionalities.Autocomplete(_unit);
        }

        public ICommand ChangeProductPriceInStoreDatabaseCommand => _changeProductPriceInStoreDatabaseCommand?? (_changeProductPriceInStoreDatabaseCommand = new RelayCommand(ChangeProductPriceInStoreDatabase));
        

        public ICommand PopulatingChangePriceCommand => _populatingChangePriceCommand ??
                                                          (_populatingChangePriceCommand = new RelayCommand(PopulatingListChangePrice));

        

        public ICommand IllegalSignChangePriceCommand => _illegalSignChangePriceCommand ??
                                                           (_illegalSignChangePriceCommand = new RelayCommand(IllegalSignChangePrice));

        

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();


        private void PopulatingListChangePrice()
        {
            throw new System.NotImplementedException();
        }


        private void ChangeProductPriceInStoreDatabase()
        {
            throw new System.NotImplementedException();
        }


        private void IllegalSignChangePrice()
        {
            throw new System.NotImplementedException();
        }
    }
}
