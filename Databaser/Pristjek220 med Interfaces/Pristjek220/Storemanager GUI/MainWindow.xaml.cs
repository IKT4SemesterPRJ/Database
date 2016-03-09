using System.Windows;
using Pristjek220Data;

namespace Storemanager_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Storemanager.IStoremanager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Storemanager.Storemanager(new UnitOfWork(new DataContext()), new Store() {StoreName = "Aldi"});
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = tbxAddProductName.Text;
            double productPrice = double.Parse(tbxAddProductPrice.Text);

            if (_manager.AddProductToMyStore(productName, productPrice) != 0)
            {
                lblConfirm.Content = ($"produktet {productName} findes allerede");
                return;
            }

            lblConfirm.Content = ($"{productName} er indsat, med prisen {productPrice} i butikken {_manager.Store.StoreName}");
        }
    }
}
