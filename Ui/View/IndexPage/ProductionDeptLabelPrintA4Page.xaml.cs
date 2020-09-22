using Model;
using System.Runtime.Caching;
using System.Windows.Controls;
using Ui.Service;
using Ui.ViewModel.IndexPage;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptLabelPrintA4Page.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptLabelPrintA4Page : Page
    {
        public ProductionDeptLabelPrintA4Page()
        {
            InitializeComponent();
            this.DataContext = new ProductionDeptLabelPrintA4ViewModel();
            new CommonService().GetUserCustomDataGridColumn((MemoryCache.Default["UserCache"] as UserCacheModel).User.ID, this.DGPrintA4,0);
        }
    }
}
