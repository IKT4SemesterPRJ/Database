using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoComplete;
using Storemanager;
using Pristjek220Data;
using System;

namespace Storemanager_GUI.User_Controls
{ 
    internal class NewProductModel : ObservableObject, IPageViewModel
    {
        private ICommand _addToStoreDatabaseCommand;
        private ICommand _populatingNewProductCommand;
        private ICommand _illegalSignNewProductCommand;
        private readonly IAutocomplete _autocomplete;
        private readonly Storemanager.IStoremanager _manager;
        private string oldtext = string.Empty;
        private string oldnum = string.Empty;
        private object atbxAddProductName;
        private object lblConfirm;
        private object tbxAddProductPrice;


        public ICommand AddToStoreDatabaseCommand
        {
            get
            {
                return _addToStoreDatabaseCommand ?? (_addToStoreDatabaseCommand = new RelayCommand(AddToStoreDatabaseCommand));
            }
        }



        

        /*
        public NewProduct()
        {
            InitializeComponent();
            _manager = new Storemanager.Storemanager(new UnitOfWork(new DataContext()), new Store() { StoreName = "Aldi" });
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

            productName = char.ToUpper(productName[0]) + productName.Substring(1).ToLower();

            var product = _manager.FindProduct(productName);
            if (product == null)
            {
                product = new Product() { ProductName = productName };
                _manager.AddProductToDb(product);
                product = _manager.FindProduct(productName);
            }

            if (_manager.AddProductToMyStore(product, productPrice) != 0)
            {
                lblConfirm.Content = ($"Produktet {productName} findes allerede");
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
        */
    }
}
