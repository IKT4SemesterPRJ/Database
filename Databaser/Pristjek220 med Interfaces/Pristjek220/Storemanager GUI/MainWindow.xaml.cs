using System.Windows;
using System.Windows.Controls;
using AutoComplete;
using Pristjek220Data;

namespace Storemanager_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAutocomplete _autocomplete;
        private readonly Storemanager.IStoremanager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new Storemanager.Storemanager(new UnitOfWork(new DataContext()), new Store() {StoreName = "Aldi"});
            _autocomplete = new Autocomplete(new UnitOfWork(new DataContext()));
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = atbxAddProductName.Text;
            double productPrice = double.Parse(tbxAddProductPrice.Text);

            var product = _manager.FindProduct(productName);
            if (product == null)
            {
                product = new Product() {ProductName = productName};
                _manager.AddProductToDb(product);
                product = _manager.FindProduct(productName);
            }

            if (_manager.AddProductToMyStore(product, productPrice) != 0)
            {
                lblConfirm.Content = ($"produktet {productName} findes allerede");
                return;
            }

            lblConfirm.Content = ($"{productName} er indsat, med prisen {productPrice} i butikken {_manager.Store.StoreName}");
        }

        private void atbxAddProductName_TextChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var autoComplete = _autocomplete.AutoCompleteProduct(atbxAddProductName.Text);
            atbxAddProductName.ItemsSource = autoComplete;
        }
    }
}
