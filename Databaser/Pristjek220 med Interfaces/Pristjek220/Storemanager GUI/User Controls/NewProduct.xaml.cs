using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoComplete;
using Pristjek220Data;

namespace Storemanager_GUI.User_Controls
{
    /// <summary>
    /// Interaction logic for NewProduct.xaml
    /// </summary>
    public partial class NewProduct : UserControl
    {
        private readonly IAutocomplete _autocomplete;
        private readonly Storemanager.IStoremanager _manager;
        private string oldtext = String.Empty;
        private string oldnum = String.Empty;

        public NewProduct()
        {
            InitializeComponent();
            _manager = new Storemanager.Storemanager(new UnitOfWork(new DataContext()), new Store() {StoreName = "Aldi"});
            _autocomplete = new Autocomplete(new UnitOfWork(new DataContext()));
        }

        private void AtbxAddProductName_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (atbxAddProductName.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                oldtext = atbxAddProductName.Text;
                var autoComplete = _autocomplete.AutoCompleteProduct(atbxAddProductName.Text);
                atbxAddProductName.ItemsSource = autoComplete;

            }
            else
            {
                atbxAddProductName.Text = oldtext;
                System.Windows.MessageBox.Show("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9",
                    "ERROR", MessageBoxButton.OK);
            }
        }

        private void BtnAddProduct_OnClick(object sender, RoutedEventArgs e)
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

            lblConfirm.Content =
                ($"{productName} er indsat, med prisen {productPrice} i butikken {_manager.Store.StoreName}");

        }

        private void TbxAddProductPrice_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (atbxAddProductName.Text.All(chr => char.IsNumber(chr) || isDot(chr)))
            {
                oldnum = atbxAddProductName.Text;

            }
            else
            {
                atbxAddProductName.Text = oldtext;
                System.Windows.MessageBox.Show("Der kan kun skrives tallene fra 0 til 9 og punktum", "ERROR",
                    MessageBoxButton.OK);
            }
        }

        private bool isDot(char text)
        {
            if (text.ToString() == ".")
                return true;
            return false;
        }
    }
}