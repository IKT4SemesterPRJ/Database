using System.Windows;
using Pristjek220Data;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StoremanagerGUI : Window
    {

        /// <summary>
        ///     StoremanagerGUI constructor takes a UnitOfWork to create an ApplicationViewModel
        /// </summary>
        /// <param name="store"></param>
        /// <param name="unit"></param>
        public StoremanagerGUI(Store store, IUnitOfWork unit)
        {
            InitializeComponent();
            base.DataContext = new ApplicationViewModel(store, unit);
        }
    }
}
