using Bll.Services;
using Model;
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
using static Ui.Command.DelegateCommand;

namespace Ui.View
{

    //public delegate void UpdateHistoryDelegate(int count);
    /// <summary>
    /// LabelPrintPrintCountModifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintPrintCountModifyWindow : System.Windows.Window
    {
        //public event RefreshParentDelegate RefreshEvent;
        private LabelPrintHistoryModel _window;
        public LabelPrintPrintCountModifyWindow(LabelPrintHistoryModel window)
        {
            _window = window;
            this.DataContext = _window;
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            this.Loaded += Window_Loaded;
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(this.LastPrintCount.Text, out int count))
                {
                    int id = int.Parse(this.ID.Text);
                    string seq = this.Seq.Text;
                    if (!string.IsNullOrEmpty(seq))
                    {
                        count = 1;
                        _window.PrintCount = 1;
                    }
                    MessageBoxResult result = MessageBox.Show($" 确认修改 序号为【 {seq} 】的标签  下次打印张数【 {count} 】张，", "温馨提示", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        int rows = new LabelPrintService().ModifyHistoryDataByID(id, count,seq);

                        if (rows == 1)
                        {
                            this.Close();
                            //RefreshEvent();
                            MessageBox.Show("保存成功");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请输入正确的数字");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LastPrintCount.Focus();
            this.LastPrintCount.SelectAll();
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            _window.Seq = string.Empty;
        }
    }
}
