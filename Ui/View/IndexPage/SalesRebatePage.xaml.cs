﻿using Model;
using System.Runtime.Caching;
using System.Windows.Controls;
using Ui.Service;
using Ui.ViewModel.IndexPage;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// SalesRebatePage.xaml 的交互逻辑
    /// </summary>
    public partial class SalesRebatePage : Page
    {
        public SalesRebatePage()
        {
            InitializeComponent();
            this.DataContext = new SalesRebateViewModel();
            new CommonService().GetUserDataGridColumn((MemoryCache.Default["UserCache"] as UserCacheModel).User.ID, this.DGSalesRebate, 1);
        }
    }
}
