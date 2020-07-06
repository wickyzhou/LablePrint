using Common;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using wf=System.Windows.Forms;
using Ui.Service;
using System.IO;
using Ui.ViewModel.IndexPage;

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
            this.DataContext = new ProductionDeptIndexPageViewModel();
        }
    }
}
