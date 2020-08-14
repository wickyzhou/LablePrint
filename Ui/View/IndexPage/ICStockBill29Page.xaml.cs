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
using Ui.ViewModel.IndexPage;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ICStockBill29Page.xaml 的交互逻辑
    /// </summary>
    public partial class ICStockBill29Page : Page
    {
        public ICStockBill29Page()
        {
            InitializeComponent();
            this.DataContext = new ICStockBill29ViewModel();

        }
    }
}
