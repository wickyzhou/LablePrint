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
    /// ProductionDeptSemiMaterialsInventoryPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptSemiMaterialsInventoryPage : Page
    {
        public ProductionDeptSemiMaterialsInventoryPage()
        {
            InitializeComponent();
            this.DataContext = new ProductionDeptSemiMaterialsInventoryViewModel();
        }

        private void McmbOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ss = 1;
        }
    }
}
