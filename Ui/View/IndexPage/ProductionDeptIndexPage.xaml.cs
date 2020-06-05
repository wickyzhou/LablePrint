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
using Ui.View.InfoWindow;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptIndexPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptIndexPage : Page
    {
        public ProductionDeptIndexPage()
        {
            InitializeComponent();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
        }

        private void BtnModifyDate_Click(object sender, RoutedEventArgs e)
        {
            //ProductionDeptModifyDate window = new ProductionDeptModifyDate()
            //{
            //    Owner = Window.GetWindow(this)
            //};
            //window.ShowDialog();
        }
    }
}
