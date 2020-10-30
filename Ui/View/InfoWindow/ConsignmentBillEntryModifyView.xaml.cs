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
    /// ConsignmentBillModifyView.xaml 的交互逻辑
    /// </summary>
    public partial class ConsignmentBillEntryModifyView : Window
    {
        public ConsignmentBillEntryModifyView()
        {
            InitializeComponent();
            this.TbFocus.Focus();
            this.TbFocus.SelectAll();
            this.DataContext = new ConsignmentBillEntryModifyViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            this.MouseLeftButtonDown += (sender, e) => { if(e.LeftButton== MouseButtonState.Pressed) this.DragMove(); };
        }
    }
}
