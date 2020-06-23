using Bll.Services;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Ui.Command.DelegateCommand;

namespace Ui.View
{

    //public delegate void DisplayUpdate();


    /// <summary>
    /// LabelPrintCurrentPrintDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LabelPrintCurrentPrintDataWindow : System.Windows.Window
    {
        public ObservableCollection<LabelPrintCurrencyModel> CurrentRecords = new ObservableCollection<LabelPrintCurrencyModel>();
        private readonly UserModel _user;
        private DateTime _date;
        private readonly PrintSchemaParameterModel _printConfig;
        private bool isRefreshParentDataGrid = false;//是否需要刷新标签打印界面表格
        public event RefreshParentDelegate RefreshEvent;

        public LabelPrintCurrentPrintDataWindow(UserModel user, DateTime date, PrintSchemaParameterModel printConfig)
        {
            InitializeComponent();
            this._user = user;
            this._date = date;
            this._printConfig = printConfig;

            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };

            this.Loaded += Window_Loaded;

            //this.MainDataGrid.LoadingRow+= (sender, e) => { if (f==0) { var row = e.Row.Item as LabelPrintCurrencyModel; row.PrintOrder = e.Row.GetIndex() * 10;  } };
            //this.MainDataGrid.PreviewDragEnter += (sender, e) => { e.Effects = DragDropEffects.Move; };
            //this.MainDataGrid.PreviewDragOver += (sender, e) => {
            //    DataGridRow r = e.Data as DataGridRow;
            //    r.Height = 50;
            //    r.FontSize = 20;
            //    MessageBox.Show("你拖动我");
            //};
            //this.MainDataGrid.PreviewDrop += (sender, e) => { MessageBox.Show("你放下我"); };





            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) =>
            {
                if (isRefreshParentDataGrid)
                {
                    RefreshEvent();
                }
                this.Close();
            }));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (send, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else if (WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
            }));
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var users = new UserService().GetAllUsers();
            this.CbUser.ItemsSource = users;
            this.CbUser.Text = _user.UserName;
            this.DP1.SelectedDate = _date;
            this.CbPrintStatus.ItemsSource = new List<string>() { "未打印", "已打印" };
            this.CbPrintStatus.SelectedIndex = 0;

            this.MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintCurrencyModel>(new LabelPrintService().GetUserData(_date, _user.ID, this.CbPrintStatus.Text));

        }
        private void CbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && e.AddedItems.Count > 0)
            {
                int uerId = (e.AddedItems[0] as UserModel).ID;
                string status = this.CbPrintStatus.Text;
                DateTime date = this.DP1.SelectedDate.Value;
                this.MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintCurrencyModel>(new LabelPrintService().GetUserData(date, uerId, status));
            }
        }
        //.OrderByDescending(m=>m.CreateTime).ThenBy(m=>m.BatchNo)
        private void DP1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DP1.SelectedDate != null && this.IsLoaded)
            {
                string status = this.CbPrintStatus.Text;
                int userId = Convert.ToInt32(this.CbUser.SelectedValue);
                DateTime date = this.DP1.SelectedDate.Value;
                this.MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintCurrencyModel>(new LabelPrintService().GetUserData(date, userId, status));
            }
        }

        private void CbPrintStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && e.AddedItems.Count > 0)
            {
                string status = e.AddedItems[0].ToString();
                DateTime date = this.DP1.SelectedDate.Value;
                int userId = Convert.ToInt32(this.CbUser.SelectedValue);
                this.MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintCurrencyModel>(new LabelPrintService().GetUserData(date, userId, status));
            }
        }

        #region 未打印删除
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems.Count == 1)
            {
                LabelPrintCurrencyModel model = MainDataGrid.SelectedItem as LabelPrintCurrencyModel;
                if (model.PrintStatus == "已打印")
                {
                    MessageBox.Show("已打印的数据不能删除");
                    return;
                }

                string r = new LabelPrintService().DeleteCurrentPrintData(model.ID);
                if (r != null)
                {
                    CurrentRecords.Remove(model);
                    (this.MainDataGrid.ItemsSource as ObservableCollection<LabelPrintCurrencyModel>).Remove(model);
                    this.MainDataGrid.SelectedItem = null;
                    isRefreshParentDataGrid = true;
                    MessageBox.Show(r);
                    return;
                }
                MessageBox.Show("删除失败，请联系管理员");

            }
            else
            {
                MessageBox.Show("删除失败：请先选中数据，有且仅有1条数据");
            }
        }

        private void BtnBatchDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要删除当前生产日期下所有未打印数据", "温馨提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (this.CbPrintStatus.Text == "已打印")
                {
                    MessageBox.Show(" 请先查询未打印数据 ");
                    return;
                }
                string r = new LabelPrintService().BatchDeleteCurrentPrintData(this.DP1.SelectedDate.Value, _user.ID);
                if (r != null)
                {
                    this.MainDataGrid.ItemsSource = null;
                    isRefreshParentDataGrid = true;
                    MessageBox.Show(r);
                    return;
                }
                MessageBox.Show("删除失败，请联系管理员");
            }
        }
        #endregion


        #region 已打印删除
        private void BtnSingleRowDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems.Count == 1)
            {
                LabelPrintCurrencyModel model = MainDataGrid.SelectedItem as LabelPrintCurrencyModel;
                if (model.PrintStatus == "未打印")
                {
                    MessageBox.Show("只能删除已打印数据");
                    return;
                }

                string r = new LabelPrintService().DeleteCurrentPrintData(model.ID);
                if (r != null)
                {
                    (this.MainDataGrid.ItemsSource as ObservableCollection<LabelPrintCurrencyModel>).Remove(model);
                    //CurrentRecords.Remove(model);
                    this.MainDataGrid.SelectedItem = null;
                    // 删除日志
                    new LabelPrintService().DeletePrintLog(new List<int> { model.ID });
                    isRefreshParentDataGrid = true;
                    MessageBox.Show(r);
                    return;
                }
                MessageBox.Show("删除失败，请联系管理员");

            }
            else
            {
                MessageBox.Show("删除失败：请先选中数据，有且仅有1条数据");
            }
        }

        private void BtnBatchNoDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems.Count == 1)
            {
                LabelPrintCurrencyModel model = MainDataGrid.SelectedItem as LabelPrintCurrencyModel;
                if (model.PrintStatus == "未打印")
                {
                    MessageBox.Show("只能删除已打印数据");
                    return;
                }
                var itemSource = MainDataGrid.ItemsSource as ObservableCollection<LabelPrintCurrencyModel>;

                List<int> ids = itemSource.Where(m => m.CreateTime == model.CreateTime && m.BatchNo == model.BatchNo).Select(m => m.ID).ToList<int>();

                string r = new LabelPrintService().BatchDeleteCurrentPrintData(ids);
                if (r != null)
                {
                    for (int i = 0; i < itemSource.Count; i++)
                    {
                        DateTime time = itemSource[i].CreateTime;
                        string batchno = itemSource[i].BatchNo;
                        // 默认提取数据是按添加时间降序
                        if (time > model.CreateTime)
                        {
                            continue;
                        }
                        else if (time == model.CreateTime)
                        {
                            if (batchno == model.BatchNo)
                            {
                                itemSource.RemoveAt(i);
                                i--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    this.MainDataGrid.SelectedItem = null;
                    // 删除日志
                    new LabelPrintService().DeletePrintLog(ids);
                    isRefreshParentDataGrid = true;
                    MessageBox.Show(r);
                    return;
                }
                MessageBox.Show("删除失败，请联系管理员");

            }
            else
            {
                MessageBox.Show("删除失败：有且仅有1条数据");
            }
        }

        private void BtnCreateTimeDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItems.Count == 1)
            {
                LabelPrintCurrencyModel model = MainDataGrid.SelectedItem as LabelPrintCurrencyModel;
                if (model.PrintStatus == "未打印")
                {
                    MessageBox.Show("只能删除已打印数据");
                    return;
                }
                var itemSource = MainDataGrid.ItemsSource as ObservableCollection<LabelPrintCurrencyModel>;

                List<int> ids = itemSource.Where(m => m.CreateTime == model.CreateTime).Select(m => m.ID).ToList<int>();

                string r = new LabelPrintService().BatchDeleteCurrentPrintData(ids);
                if (r != null)
                {
                    for (int i = 0; i < itemSource.Count; i++)
                    {
                        DateTime time = itemSource[i].CreateTime;
                        // 默认提取数据是按添加时间降序
                        if (time > model.CreateTime)
                        {
                            continue;
                        }
                        else if (time == model.CreateTime)
                        {
                            itemSource.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.MainDataGrid.SelectedItem = null;
                    // 删除日志
                    new LabelPrintService().DeletePrintLog(ids);
                    isRefreshParentDataGrid = true;
                    MessageBox.Show(r);
                    return;
                }
                MessageBox.Show("删除失败，请联系管理员");

            }
            else
            {
                MessageBox.Show("删除失败：有且仅有1条数据");
            }
        }
        #endregion

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_printConfig.TemplateFullName) || string.IsNullOrWhiteSpace(_printConfig.PrinterName) || string.IsNullOrWhiteSpace(_printConfig.Orientation))
            {
                MessageBox.Show("请选择Tender打印机、模板、方向");
                return;
            }

            var data = (this.MainDataGrid.ItemsSource as IEnumerable<LabelPrintCurrencyModel>).OrderBy(m=>m.PrintOrder).ToList();
            string r = new PrintHelper().PrintLabel(_printConfig, data);
            if (string.IsNullOrEmpty(r))
            {
                isRefreshParentDataGrid = true;
                MessageBox.Show("打印成功");
            }
            else
            {
                MessageBox.Show(r);
            }

        }

        private void MainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var newValue = (e.EditingElement as TextBox).Text;
           
            if (!string.IsNullOrEmpty(newValue)|| int.TryParse(newValue,out _))
            {
                // 值改变则同步到数据库
                var row = e.Row.Item as LabelPrintCurrencyModel;
                if (row.PrintOrder.ToString() != newValue)
                {
                    new LabelPrintService().ModifyCurrency(row.ID, int.Parse(newValue));
                   //MessageBox.Show($"新值{newValue} \t 旧值{row.PrintOrder} \t ");
                }
            }
            else
            {
                MessageBox.Show(" 必须输入整数 ");
            }

        }
    }
}
