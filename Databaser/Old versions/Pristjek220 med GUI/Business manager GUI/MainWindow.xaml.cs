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
using Database;

namespace Business_manager_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Storemanager Manager;
        public MainWindow()
        {
            InitializeComponent();
            Manager = new Storemanager("Aldi");
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = tbxAddProductName.Text;
            double productPrice = double.Parse(tbxAddProductPrice.Text);

            if (Manager.AddProduct(productName, productPrice) != 0)
            {
                lblConfirm.Content = ($"produktet {productName} findes allerede");
                return;
            }

            lblConfirm.Content = ($"{productName} er indsat, med prisen {productPrice} i butikken {Manager.Store.StoreName}");
        }
    }
}
