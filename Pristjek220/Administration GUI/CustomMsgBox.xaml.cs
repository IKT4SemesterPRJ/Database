using System.Windows;
using System.Windows.Forms;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for CustomMsgBox.xaml
    /// </summary>
    public partial class CustomMsgBox
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomMsgBox()
        {
            InitializeComponent();
        }

        static CustomMsgBox _customMsgBox;
        static DialogResult _result = System.Windows.Forms.DialogResult.No;

        /// <summary>
        ///     Shows the custom messagebox
        /// </summary>
        /// <param name="labelText"></param>
        /// <param name="caption"></param>
        /// <param name="btn1"></param>
        /// <param name="btn2"></param>
        /// <returns></returns>
        public static DialogResult Show(string labelText, string caption, string btn1, string btn2)
        {
            _customMsgBox = new CustomMsgBox
            {
                BtnLeft = {Content = btn1},
                BtnRight = {Content = btn2},
                TextBlock = {Text = labelText},
                Title = caption
            };
            _customMsgBox.ShowDialog();
            return _result;
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            _result = System.Windows.Forms.DialogResult.Yes;
            _customMsgBox.Close();
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            _result = System.Windows.Forms.DialogResult.No;
            _customMsgBox.Close();
        }
    }
}
