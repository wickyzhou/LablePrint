﻿using System;
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
    /// OilSamplePrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class OilSamplePrintPage : Page
    {
        public OilSamplePrintPage()
        {
            InitializeComponent();
            this.DataContext = new OilSamplePrintViewModel();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
        }
    }
}
