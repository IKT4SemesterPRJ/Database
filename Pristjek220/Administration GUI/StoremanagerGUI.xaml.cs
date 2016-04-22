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
using Administration_GUI.User_Controls;
using Pristjek220Data;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StoremanagerGUI : Window
    {

        public StoremanagerGUI(Store store)
        {
            InitializeComponent();
            base.DataContext = new ApplicationViewModel(store);
        }
    }
}
