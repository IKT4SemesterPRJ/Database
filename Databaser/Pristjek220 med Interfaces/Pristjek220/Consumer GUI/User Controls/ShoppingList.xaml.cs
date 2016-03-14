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
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : UserControl
    {
        private readonly IConsumer _user;
        private readonly IAutocomplete _autocomplete;
        private string oldtext = String.Empty;

        public ShoppingList()
        {
            InitializeComponent();
            var unit = new UnitOfWork(new DataContext());
            _user = new Consumer.Consumer(unit);
            _autocomplete = new Autocomplete(unit);
        }

        private void AcbProductToList_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (acbProductToList.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                oldtext = acbProductToList.Text;
                var autoComplete = _autocomplete.AutoCompleteProduct(acbProductToList.Text);
                acbProductToList.ItemsSource = autoComplete;

            }
            else
            {
                acbProductToList.Text = oldtext;
                System.Windows.MessageBox.Show("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
            }
        }

        private void BtnItemToList_OnClick(object sender, RoutedEventArgs e)
        {
            string product = acbProductToList.Text;
            //check om produktet findes i en if, og hvis den ikke gør så lav et vindue
            //if(protuctExist == true)
            lbxShoppingList.Items.Add(product);
            //else
            System.Windows.MessageBox.Show("produktet findes ikke", "Error", MessageBoxButton.OK);

        }

        private void BtnDeleteListItem_OnClick(object sender, RoutedEventArgs e)
        {
            lbxShoppingList.Items.RemoveAt(lbxShoppingList.SelectedIndex);
        }
    }
}
