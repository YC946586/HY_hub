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
using System.Windows.Shapes;

namespace Uninstall
{
    /// <summary>
    /// OrxMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class OrxMessageBox : Window
    {
        public OrxMessageBox(string txtBt, string txtnr)
        {
            InitializeComponent();
            TxtContent.Text = txtBt;
            txtTitle.Text = txtnr;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Common.isBtn = true;
            this.Close();
        }

        private void ButtonBase_OnNoClick(object sender, RoutedEventArgs e)
        {
            Common.isBtn = false;
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Common.isBtn = false;
            this.Close();
        }
    }
}
