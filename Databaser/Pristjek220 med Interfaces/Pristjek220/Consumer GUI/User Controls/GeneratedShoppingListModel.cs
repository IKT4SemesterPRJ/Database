using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;


        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingList { get; } = new ObservableCollection<StoreProductAndPrice>();
    }
}
