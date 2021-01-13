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
using Ui.Command;

namespace Ui.UserControls
{
    /// <summary>
    /// ExportUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ExportUserControl : UserControl
    {
        public ExportUserControl()
        {
            InitializeComponent();
        }



        public string ExportPath
        {
            get { return (string)GetValue(ExportPathProperty); }
            set { SetValue(ExportPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExportPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExportPathProperty =
            DependencyProperty.Register("ExportPath", typeof(string), typeof(ExportUserControl), new PropertyMetadata( new PropertyChangedCallback(OnExportPathChanged)));

        private static void OnExportPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExportUserControl euc = d as ExportUserControl;
            if (euc != null)
                euc.tbPath.Text = e.NewValue.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 导出目录选择
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExportPath = fbd.SelectedPath;
            }
        }



        public ICommand ExportCommand
        {
            get { return (ICommand)GetValue(ExportCommandProperty); }
            set { SetValue(ExportCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ExportCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExportCommandProperty =
            DependencyProperty.Register("ExportCommand", typeof(ICommand), typeof(ExportUserControl), new PropertyMetadata(null));

    }
}
