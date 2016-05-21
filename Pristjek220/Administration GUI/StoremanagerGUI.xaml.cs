using System.Windows;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StoremanagerGUI : Window
    {
        /// <summary>
        ///     StoremanagerGUI constructor takes a UnitOfWork to create an StoremanagerViewModel
        /// </summary>
        /// <param name="autocomplete"></param>
        /// <param name="storemanager"></param>
        public StoremanagerGUI(IAutocomplete autocomplete, IStoremanager storemanager)
        {
            InitializeComponent();
            base.DataContext = new StoremanagerViewModel(storemanager, autocomplete);
        }
    }
}
