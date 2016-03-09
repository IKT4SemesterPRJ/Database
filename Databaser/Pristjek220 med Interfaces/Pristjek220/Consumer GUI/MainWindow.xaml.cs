using System.Windows;
using System.Windows.Controls;
using Pristjek220Data;
using Consumer;


namespace Consumer_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IConsumer User;

        public MainWindow()
        {
            InitializeComponent();
            User = new Consumer.Consumer(new UnitOfWork(new DataContext()));
        }

        private void btnFindProduct_Click(object sender, RoutedEventArgs e)
        {
            string product = tbxFindProduct.Text;
            var store = User.FindCheapestStore(product);
            if (store != null)
                lblFindProduct.Content = store.StoreName;
            else
                lblFindProduct.Content = "Vare findes ikke";
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Test.IsDropDownOpen = true;
            var autoComplete = User.AutoComplete(Test.Text);
            Test.ItemsSource = autoComplete;
        }
    }
}
