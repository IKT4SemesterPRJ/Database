using System.Windows;
using Pristjek220Data;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        /// <summary>
        ///     Admin constructor takes a UnitOfWork to create an AdminViewModel
        /// </summary>
        /// <param name="unit"></param>
        public Admin(IUnitOfWork unit)
        {
            InitializeComponent();
            DataContext = new AdminViewModel(unit);
        }
    }
}
