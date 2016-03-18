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
        private string oldtext = String.Empty;
        public FindProduct()
        {
            InitializeComponent();
        }

        //private void AutoCompleteBox_OnTextChanged(object sender, RoutedEventArgs e)
        //{
        //    if (acbSeachForProduct.Text.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
        //    {
        //        oldtext = acbSeachForProduct.Text;
        //    }
        //    else
        //    {
        //        acbSeachForProduct.Text = oldtext;
        //        System.Windows.MessageBox.Show("Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9", "ERROR", MessageBoxButton.OK);
        //    }
        //}
    }
}
