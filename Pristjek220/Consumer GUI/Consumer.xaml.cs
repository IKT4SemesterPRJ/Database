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
using Pristjek220Data;
using SharedFunctionalities;

namespace Consumer_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            var unit = new UnitOfWork(new DataContext());
            var user = new Consumer.Consumer(unit);
            var autocomplete = new Autocomplete(unit);
            var databaseFunctions = new DatabaseFunctions(unit);
            InitializeComponent();
            DataContext = new ConsumerViewModel(user, autocomplete, databaseFunctions);
        }
    }
}
