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
        private IEventAggregator Event;

        public GeneratedShoppingListModel(IEventAggregator eventAggregator)
        {
            Event = eventAggregator;
            _user = new Consumer.Consumer(_unit);
            Event.GetEvent<PubSubEvent<List<ProductInfo>>>().Subscribe(Update);
        }

        private void Update(List<ProductInfo> List)
        {
            GeneratedShoppingListData.Clear();
            var tempGeneretedShopList = _user.CreateShoppingList(List);
            foreach (var item in tempGeneretedShopList)
            {
                GeneratedShoppingListData.Add(item);
            }
        }


        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; } = new ObservableCollection<StoreProductAndPrice>();
    }
}
