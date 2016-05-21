using System.Windows;
using Administration;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        /// <summary>
        ///     Admin constructor takes a admin and a autocomplete to create an AdminViewModel
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="autocomplete"></param>
        public Admin(IAdmin admin, IAutocomplete autocomplete)
        {
            InitializeComponent();

            DataContext = new AdminViewModel(admin, autocomplete);
        }
    }
}
