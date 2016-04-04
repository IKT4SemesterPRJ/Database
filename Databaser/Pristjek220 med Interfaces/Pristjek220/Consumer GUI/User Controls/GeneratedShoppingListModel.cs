using System.Collections.ObjectModel;
using System.Linq;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    internal class GeneratedShoppingListModel : ObservableObject, IPageViewModel
    {
        private UnitOfWork _unit = new UnitOfWork(new DataContext());
        private readonly IConsumer _user;

        public GeneratedShoppingListModel(Consumer.Consumer user)
        {
            _user = user;
        }

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData
        {
            get
            {
                return
                    new ObservableCollection<StoreProductAndPrice>(
                        _user.GeneratedShoppingListData.OrderBy(listData => listData.StoreName)
                            .ThenBy(listData => listData.ProductName));
            }
        }
    }
}