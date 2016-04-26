using System.Windows;
using Pristjek220Data;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StoremanagerGUI : Window
    {

        public StoremanagerGUI(Store store, IUnitOfWork unit)
        {
            InitializeComponent();
            base.DataContext = new ApplicationViewModel(store, unit);
        }
    }
}
