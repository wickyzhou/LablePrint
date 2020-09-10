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
    /// WarehouseMaterialTransferAndSafeInventory.xaml 的交互逻辑
    /// </summary>
    public partial class WarehouseMaterialTransferAndSafeInventoryPage : Page
    {
        public WarehouseMaterialTransferAndSafeInventoryPage()
        {
            InitializeComponent();
            this.DataContext = new WarehouseMaterialTransferAndSafeInventoryViewModel();
  
        }
    }
}
