using System.Windows;
using AutoComplete;
using Pristjek220Data;
using Consumer;


namespace Consumer_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly IConsumer _user;
        private readonly IAutocomplete _autocomplete;

        public MainWindow()
        {
            InitializeComponent();
            var unit = new UnitOfWork(new DataContext());
            _user = new Consumer.Consumer(unit);
            _autocomplete = new Autocomplete(unit);
        }

        private void btnFindProduct_Click(object sender, RoutedEventArgs e)
        {
            string product = atbxFindProduct.Text;
            var store = _user.FindCheapestStore(product);
            if (store != null)
                lblFindProduct.Content = store.StoreName;
            else
                lblFindProduct.Content = "Vare findes ikke";
        }

        private void AutoBox_OnTextChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var autoComplete = _autocomplete.AutoCompleteProduct(atbxFindProduct.Text);
            atbxFindProduct.ItemsSource = autoComplete;
        }
    }
}
