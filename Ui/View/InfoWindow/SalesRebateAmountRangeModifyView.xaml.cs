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
using System.Windows.Shapes;
using Ui.ViewModel;

namespace Ui.View.InfoWindow
{
    /// <summary>
    /// SalesRebateAmountRangeModifyView.xaml 的交互逻辑
    /// </summary>
    public partial class SalesRebateAmountRangeModifyView : Window
    {
        public SalesRebateAmountRangeModifyView()
        {
            InitializeComponent();
            this.DataContext = new SalesRebateAmountRangeModifyViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
        }
    }
}
