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
            string product = atbxFindProduct.Text;
            var store = User.FindCheapestStore(product);
            if (store != null)
                lblFindProduct.Content = store.StoreName;
            else
                lblFindProduct.Content = "Vare findes ikke";
        }

        private void AutoBox_OnTextChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var autoComplete = User.AutoComplete(atbxFindProduct.Text);
            atbxFindProduct.ItemsSource = autoComplete;
        }
    }
}
