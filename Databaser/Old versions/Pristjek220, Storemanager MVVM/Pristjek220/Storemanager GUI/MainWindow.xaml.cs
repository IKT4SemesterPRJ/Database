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
using Storemanager_GUI.User_Controls;

namespace Storemanager_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserControl NewProduct;
        private readonly UserControl DeleteProduct;
        private readonly UserControl ChangePrice;

        public MainWindow()
        {
            NewProduct = new NewProduct();
            DeleteProduct = new DeleteProduct();
            ChangePrice = new ChangePrice();
            InitializeComponent();
        }

        private void BtbUCNewProduct_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = NewProduct;
        }

        private void BtbUCDeletePoduct_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = DeleteProduct;
        }

        private void BtbUCChangePrice_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = ChangePrice;
        }
    }
}
