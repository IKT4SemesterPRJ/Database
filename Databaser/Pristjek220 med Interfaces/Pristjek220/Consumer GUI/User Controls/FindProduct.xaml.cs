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
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    /// <summary>
    /// Interaction logic for FindProduct.xaml
    /// </summary>
    public partial class FindProduct : UserControl
    {
        private readonly IConsumer _user;
        private readonly IAutocomplete _autocomplete;
        private string oldtext = String.Empty;
        private readonly List<string> _autoCompleteStringList;

        public FindProduct()
        {
            InitializeComponent();
            var unit = new UnitOfWork(new DataContext());
            _user = new Consumer.Consumer(unit);
            _autocomplete = new Autocomplete(unit);
            _autoCompleteStringList = new List<string>();

        }

        private void BtnFindProduct_OnClick(object sender, RoutedEventArgs e)
        {
            string product = acbSeachForProduct.Text;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                System.Windows.MessageBox.Show($"Det er billigst i {store.StoreName}", "Billigste forretning",MessageBoxButton.OK);
            else
                System.Windows.MessageBox.Show($"{acbSeachForProduct.Text}", "Billigste forretning", MessageBoxButton.OK);
        }

        private void AcbSeachForProduct_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (acbSeachForProduct.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                oldtext = acbSeachForProduct.Text;

                foreach (var item in _autocomplete.AutoCompleteProduct(acbSeachForProduct.Text))
                {
                    _autoCompleteStringList.Add(item);
                }
                acbSeachForProduct.ItemsSource = _autoCompleteStringList;
                //acbSeachForProduct.PopulateComplete();
            }
            else
            {
                acbSeachForProduct.Text = oldtext;
                System.Windows.MessageBox.Show("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
            }

        }
    }
}
