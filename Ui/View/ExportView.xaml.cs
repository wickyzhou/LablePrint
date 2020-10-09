using Model;
using System;
using System.Collections.Generic;
using System.Data;
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
using Ui.Command;
using Ui.Service;
using Ui.ViewModel;

namespace Ui.View
{
    /// <summary>
    /// ExportView.xaml 的交互逻辑
    /// </summary>
    public partial class ExportView : Window
    {

        private RoutedCommand cmdClear = new RoutedCommand("Clear", typeof(ExportView));
        private readonly int _dataGridTypeId;
        public ExportView(int dataGridTypeId)
        {
            _dataGridTypeId = dataGridTypeId;
            InitializeComponent();
            this.DataContext = new ExportViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
            InitializeExportViewTypedColumnGrid();
        }

        public void InitializeExportViewTypedColumnGrid()
        {
            var xx = new CommonService().GetExportViewTypedColumnWithCheckBox(_dataGridTypeId);

            for (int i = 0; i < xx.Count() + 2; i++)
            {

                GridLength width = (i == 0 || i == xx.Count() + 1) ? new GridLength(0.4, GridUnitType.Star) : new GridLength(1, GridUnitType.Star);
                ColumnDefinition columnDefinition = new ColumnDefinition() { Width = width };
                this.DynamicExportParamterGrid.ColumnDefinitions.Add(columnDefinition);
            }


            for (int i = 0; i < xx.Count(); i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = xx[i].TypedColumnName;
                checkBox.IsChecked = xx[i].IsChecked;
                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                checkBox.Margin = new Thickness() { Bottom = 5, Left = 3, Right = 3, Top = 5 };
                checkBox.SetValue(Grid.ColumnProperty, i + 1);
                checkBox.FontFamily = new FontFamily("微软雅黑");
                checkBox.FontSize = 11.5;
                checkBox.Foreground = new SolidColorBrush((Color)Application.Current.Resources["GenericRedColor"]);
                checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                checkBox.Command = (this.DataContext as ExportViewModel).CheckBoxSelectCommand;
                checkBox.CommandParameter = new List<object> { xx[i].TypedColumnId, xx[i].TypedColumnName};


                /* //这种方法是可行的，但是跟viewmodel就不相关了
              checkBox.Command = cmdClear;
              CommandBinding cb = new CommandBinding();
              cb.Command = cmdClear;
              cb.Executed += cb_Executed;
              this.DynamicExportParamterGrid.CommandBindings.Add(cb);
                */

                this.DynamicExportParamterGrid.Children.Add(checkBox);
                Grid.SetColumn(checkBox, i + 1);
            }
        }

    }
}
