using Bll.Services;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Ui.View
{
    /// <summary>
    /// LabelPrintTemplateRefWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintTemplateRefWindow : System.Windows.Window
    {
        private static ObservableCollection<PrintTemplateRefModel> RefModels;
        private static ObservableCollection<PrintTemplateRefModel> RefModelss;
        public LabelPrintTemplateRefWindow()
        {
            InitializeComponent();

            RefModelss = new ObservableCollection<PrintTemplateRefModel>(new PrintTemplateService().GetPrintTemplateRef());
            this.CbTableName.ItemsSource = from s in RefModelss
                                           group s by s.ModuleName
                                            into g
                                           select g.Key;
            this.CbTableName.SelectedIndex = 0;
            RefModels = new ObservableCollection<PrintTemplateRefModel>( RefModelss.Where(m => m.ModuleName == this.CbTableName.Text));
            this.DG1.ItemsSource = RefModels;

            //新建window后台代码
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
        }


        private void CbTableName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RefModels = new ObservableCollection<PrintTemplateRefModel>(RefModelss.Where(m => m.ModuleName == this.CbTableName.Text));
            this.DG1.ItemsSource = RefModels;
        }

        private void DG1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.DG1.SelectedItems.Count == 1)
            {
                var refModel = this.DG1.SelectedItem as PrintTemplateRefModel;
                Clipboard.SetText(refModel.TemplateFieldName);
            }
        }
    }
}
