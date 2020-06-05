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
    /// ConsignmentBillEntryCopyView.xaml 的交互逻辑
    /// </summary>
    public partial class ConsignmentBillEntryCopyView : Window
    {
        public ConsignmentBillEntryCopyView()
        {
            InitializeComponent();
            this.TbFocus.Focus();
            this.TbFocus.SelectAll();
            this.DataContext = new ConsignmentBillEntryCopyViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
        }
    }
}
