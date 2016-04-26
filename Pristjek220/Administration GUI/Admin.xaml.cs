using System.Windows;
using Pristjek220Data;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin(IUnitOfWork unit)
        {
            InitializeComponent();
            DataContext = new AdminViewModel(unit);
        }
    }
}
