using Model;
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
using Ui.ViewModel;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// WarehouseDeptShippingPage.xaml 的交互逻辑
    /// </summary>
    public partial class WarehouseDeptShippingPage : Page
    {

        public static DateTime Date = System.DateTime.Now.AddDays(-1);

        public WarehouseDeptShippingPage()
        {
            InitializeComponent();
            this.DataContext = new ConsignmentShippingViewModel();
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            var model = (sender as DataGridRow).Item as ConsignmentBillModel;
            model.IsChecked = !model.IsChecked;
        }
    }
}
