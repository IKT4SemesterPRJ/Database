using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Prism.Events;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private IConsumer _user;

        public GeneratedShoppingListModel(Consumer.Consumer user)
        {
            _user = user;
        }

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData => _user.GeneratedShoppingListData;
    }
}

