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
using Consumer_GUI.User_Controls;

namespace Consumer_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserControl Home;
        private readonly UserControl FindProduct;
        private readonly UserControl ShoppingList;

        public MainWindow()
        {
            FindProduct = new FindProduct();
            ShoppingList = new ShoppingList();
            Home = new Home();
            InitializeComponent();
            InitWindows();
        }

        private void InitWindows()
        {
            cctlShowUC.Content = FindProduct;
            cctlShowUC.Content = ShoppingList;
            cctlShowUC.Content = Home;
        }

        private void BtbUCStartside_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = Home;
        }

        private void BtbUCSøgEfterVare_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = FindProduct;
        }

        private void BtbUCIndkøbsliste_OnClick(object sender, RoutedEventArgs e)
        {
            cctlShowUC.Content = ShoppingList;
        }
    }
}
