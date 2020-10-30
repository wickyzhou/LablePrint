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
    /// SalesRebateAmountRangeAddView.xaml 的交互逻辑
    /// </summary>
    public partial class SalesRebateAmountRangeCreateView : Window
    {
        double _lastMaxValue;
        public SalesRebateAmountRangeCreateView(double lastMaxValue)
        {
            InitializeComponent();
            _lastMaxValue = lastMaxValue;
            this.DataContext = new SalesRebateAmountRangeCreateViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
            this.MouseLeftButtonDown += (sender, e) => { if(e.LeftButton== MouseButtonState.Pressed) this.DragMove(); };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SalesRebateAmountRangeCreateViewModel).Entity.AmountLower = _lastMaxValue;
        }
    }
}
