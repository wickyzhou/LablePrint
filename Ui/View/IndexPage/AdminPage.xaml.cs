using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ui.MVVM.View;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// AdminPage.xaml 的交互逻辑
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void BtnDemo_Click(object sender, RoutedEventArgs e)
        {
            DemoMainWindow window = new DemoMainWindow()
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow window = new UserManagementWindow()
            {
                Owner = Window.GetWindow(this)
            };
            window.ShowDialog();

        }

        private void BtnPage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
