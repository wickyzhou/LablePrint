using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
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
using Ui.Service;
using Ui.ViewModel.IndexPage;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptLabelPrintLSPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptLabelPrintLSPage : Page
    {
        public ProductionDeptLabelPrintLSPage()
        {
            InitializeComponent();

            new CommonService().GetUserDataGridColumn((MemoryCache.Default["UserCache"] as UserCacheModel).User.ID, this.DGPrintLS, 0);
            this.DataContext = new ProductionDeptLabelPrintLSViewModel();
        }
    }
}
