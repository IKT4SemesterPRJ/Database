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


namespace Consumer_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Consumer.Consumer User;
        
        public MainWindow()
        {
            InitializeComponent();
            User = new Consumer.Consumer();
        }

        private void btnFindProduct_Click(object sender, RoutedEventArgs e)
        {
            string product = tbxFindProduct.Text;
            var store = User.FindCheapestStore(product);
            if (store != null)
               lblFindProduct.Content = store.StoreName;
            else
               lblFindProduct.Content = "Vare findes ikke";
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var autoComplete = User.AutoComplete(Test.Text);
            Test.ItemsSource = autoComplete;
            Test.IsDropDownOpen = true;
        }
    }
}
