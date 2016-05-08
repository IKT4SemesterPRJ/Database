using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Administration_GUI
{
    /// <summary>
    /// Interaction logic for CustomMsgBox.xaml
    /// </summary>
    public partial class CustomMsgBox : Window
    {
        public CustomMsgBox()
        {
            InitializeComponent();
        }

        static CustomMsgBox customMsgBox;
        static DialogResult result = System.Windows.Forms.DialogResult.No;

        public static DialogResult Show(string LabelText, string caption, string btn1, string btn2)
        {
            customMsgBox = new CustomMsgBox();
            customMsgBox.BtnLeft.Content = btn1;
            customMsgBox.BtnRight.Content = btn2;
            customMsgBox.TextBlock.Text = LabelText;
            customMsgBox.Title = caption;
            customMsgBox.ShowDialog();
            return result;
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Yes;
            customMsgBox.Close();
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            result = System.Windows.Forms.DialogResult.No;
            customMsgBox.Close();
        }
    }
}
