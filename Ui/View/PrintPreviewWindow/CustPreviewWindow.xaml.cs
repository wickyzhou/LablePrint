using Common;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Ui.Report;

namespace Ui.View.PrintPreviewWindow
{
    /// <summary>
    /// CustPreviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CustPreviewWindow : System.Windows.Window
    {
        private readonly CustomerModel _customer;
        public CustPreviewWindow(CustomerModel customer)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) =>
            {
                this.Close();
            }));
            _customer = customer;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Customer report = new Customer();
            DataTable dataTable = UniversalFunction.ModelToTable<CustomerModel>(new List<CustomerModel> { _customer });
            report.SetDataSource(dataTable);
            this.CrystalReportsViewer1.ViewerCore.ReportSource = report;
        }
    }
}
