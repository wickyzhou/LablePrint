
using Bll.Services;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Service;
using wf = System.Windows.Forms;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptProductiveTaskPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptProductiveTaskPage : Page
    {
        string exportDirectory = ConfigurationManager.AppSettings["ExportDirectory"];

        private ObservableCollection<ProductiveTaskListModel> ob;
        private List<ProductiveTaskListModel> lists;
        readonly static UserModel user = MemoryCache.Default["user"] as UserModel;
        private static readonly ProductiveTaskWorkService _work = new ProductiveTaskWorkService();

        public ProductionDeptProductiveTaskPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            InitializeComponentDefaultValue();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
        }

        private void InitializeComponentDefaultValue()
        {
            GetDataFromDatabase();

            this.DataContext = ob;
            this.MainDataGrid.ItemsSource = ob;
        }

        private void ClearValue()
        {
            this.tbBatchNo.Text = string.Empty;
            this.tbLable.Text = string.Empty;
            this.tbOrgID.Text = string.Empty;
            this.tbProductionType.Text = string.Empty;

        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            ProductiveTaskModifyWindow productiveTaskModifyWindow = new ProductiveTaskModifyWindow();
            productiveTaskModifyWindow.DG1.DataContext = MainDataGrid.SelectedItem;
            productiveTaskModifyWindow.ShowDialog();
        }

        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            object result = new ProductiveTaskListService().SyncProductiveTaskList(this.DP2.SelectedDate.Value);
            if (result == null)
            {
                MessageBox.Show($"生成成功...");
                this.AuditText.Text = "确认审核";
                GetDataFromDatabase();
                this.DataContext = ob;
                this.MainDataGrid.ItemsSource = ob;
            }
            else
            {
                MessageBox.Show($"{result}");
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            int r = 0;
            //文件选择窗口
            wf.OpenFileDialog opd = new wf.OpenFileDialog();
            opd.Title = "选择文件";
            //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
            opd.Filter = "EXCEL文件|*.xls*";

            if (opd.ShowDialog() == wf.DialogResult.OK)
            {
                string result = new FileHelper().ImportExcelToDatabase(opd.FileName);
                if (result == null)
                {
                    r = (int)new ProductiveTaskListService().AuditProductiveTaskList(this.DP2.SelectedDate.Value);
                }

                this.AuditText.Text = "已审核";
                MessageBox.Show($"成功 {r} 条");
            }
            opd.Dispose();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

            //string exportDirectory1 = Configuration.  .ConfigurationSettings.AppSettings["Debug"];
            if (string.IsNullOrWhiteSpace(exportDirectory))
            {
                wf.FolderBrowserDialog fbd = new wf.FolderBrowserDialog();

                if (fbd.ShowDialog() == wf.DialogResult.OK)
                {
                    exportDirectory = fbd.SelectedPath;

                    //将数据写入配置文件
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["ExportDirectory"].Value = exportDirectory;
                    cfa.Save();
                }
                else
                {
                    return;
                }
            }
            if (!Directory.Exists(exportDirectory))
            {
                MessageBox.Show("配置目录不存在");
                return;
            }

            //将文件itemsource导出到excel
            new FileHelper().ExportItemSourceToExcel(lists, exportDirectory);
            MessageBox.Show("导出成功");
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.MainDataGrid.ItemsSource = GetFilteredItemSource(ob);
        }

        public void GetDataFromDatabase()
        {
            List<CBTypeModel> cBTypes = new List<CBTypeModel>();
            lists = new ProductiveTaskListService().GetAllProductiveTaskListByDate(this.DP2.SelectedDate.Value);
            if (this.BtnQuery == null)
            {
                return;
            }
            if (lists.Count() == 0)
            {
                ob = new ObservableCollection<ProductiveTaskListModel>(new List<ProductiveTaskListModel> { new ProductiveTaskListModel { FBucketName = "暂无数据，请先点击【生成数据】", FAuditTip = "确认审核" } });
                return;
            }
            ob = new ObservableCollection<ProductiveTaskListModel>(lists);

            var countGroup = lists.GroupBy(m => m.FType).Select(t => new { FType = t.Key, Count = t.Count() });
            foreach (var item in countGroup)
            {
                if (!string.IsNullOrWhiteSpace(item.FType))
                {
                    cBTypes.Add(new CBTypeModel { Name = item.FType, Count = item.Count, NameCount = item.FType + "  (" + item.Count.ToString() + ") " });
                }

            }
            this.CbType.ItemsSource = cBTypes;
        }



        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = GetFilteredItemSource(ob);
            this.MainDataGrid.ItemsSource = s;
        }

        public IEnumerable<ProductiveTaskListModel> GetFilteredItemSource(ObservableCollection<ProductiveTaskListModel> ob)
        {
            IEnumerable<ProductiveTaskListModel> result = ob;
            if (this.CbType.SelectedValue != null)
                result = result.Where(t => t.FType == this.CbType.SelectedValue.ToString());
            if (!string.IsNullOrWhiteSpace(this.tbProductionType.Text))
                result = result.Where(n => n.FitemName.ToUpper().Contains(this.tbProductionType.Text.ToUpper()));
            if (!string.IsNullOrWhiteSpace(this.tbOrgID.Text))
                result = result.Where(n => n.FOrgID.ToUpper().Contains(this.tbOrgID.Text.ToUpper()));
            if (!string.IsNullOrWhiteSpace(this.tbLable.Text))
                result = result.Where(n => n.FLabel.ToUpper().Contains(this.tbLable.Text.ToUpper()));
            if (!string.IsNullOrWhiteSpace(this.tbBatchNo.Text))
                result = result.Where(n => n.FBatchNo.ToUpper().Contains(this.tbBatchNo.Text.ToUpper()));
            return result;
        }

        private void DP2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DP2.SelectedDate != null && IsLoaded)
            {
                ClearValue();
                GetDataFromDatabase();
                this.DataContext = ob;
                this.MainDataGrid.ItemsSource = ob;
            }
        }

        private void Audit_Click(object sender, RoutedEventArgs e)
        {
            if (this.AuditText.Text == "确认审核")
            {
                DateTime dateTime = this.DP2.SelectedDate.Value;
                // 多个客户共用一个字段
                //var lists = _work.GetProductiveTaskList(this.DP2.SelectedDate.Value);
                var works = _work.GetMutiOrgNoteWorkDetail(dateTime);
                foreach (var item in works)
                {
                    string f1 = GetNewNote(dateTime,item);
                    string f2 = GetNewNote(dateTime,item); 
                    string f3 = GetNewNote(dateTime,item);
                    if (f1!=item.FRequest1 || f2!=item.FRequest2 || f3!=item.FRequest3)
                    {
                        _work.UpdateProductiveTaskWork(new ProductiveTaskWorkModel { FICMONo = item.FICMONo, FRequest1 = f1, FRequest2 = f2, FRequest3 = f3 });
                    }
                }
              
                int rowCount = (int)new ProductiveTaskListService().AuditProductiveTaskList(dateTime);
                if (rowCount > 0)
                {
                    this.AuditText.Text = "已审核";
                    MessageBox.Show($"审核成功...");
                }
                else
                {
                    MessageBox.Show($"审核失败，请联系管理员,,,");
                }
            }
        }

        private void BtnClearIncrem_Click(object sender, RoutedEventArgs e)
        {
            new ProductiveTaskListService().ClearIncrement();
            MessageBox.Show("清除成功");
        }

        private void BtnModifyDate_Click(object sender, RoutedEventArgs e)
        {
            List<string> batches = new List<string>();
            //遍历checkbox获取对应的数据ID，批量插入x打印表
            var itemsSource = this.MainDataGrid.ItemsSource;
            foreach (ProductiveTaskListModel entry in itemsSource)
            {
                if (entry.IsChecked)
                {
                    batches.Add("'" + entry.FBatchNo + "'");
                }
            }
            string batchesIn = string.Join(",", batches);
            if (string.IsNullOrEmpty(batchesIn))
            {
                MessageBox.Show(" 没有选择批号");
            }
            MessageBoxResult result = MessageBox.Show($"确认修改选择批次的领料单和入库单制单日期为【{this.DP3.Text}】?", "温馨提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var r = new ProductiveTaskListService().ModifyBillDateMonthly(batchesIn, Convert.ToDateTime(this.DP2.Text), Convert.ToDateTime(this.DP3.Text), user.ID);
                if (r.Length > 0)
                {
                    MessageBox.Show(r);
                }
                else
                {
                    MessageBox.Show("修改失败，请联系管理员");
                }
            }
        }

        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            var itemsSource = this.MainDataGrid.ItemsSource;
            foreach (ProductiveTaskListModel entry in itemsSource)
            {
                entry.IsChecked = true;
            }
        }

        private void CheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            var itemsSource = this.MainDataGrid.ItemsSource;
            foreach (ProductiveTaskListModel entry in itemsSource)
            {
                entry.IsChecked = false;
            }
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var model = (sender as DataGridRow).Item as ProductiveTaskListModel;
            model.IsChecked = !model.IsChecked;
        }

        private string GetNewNote(DateTime date,ProductiveTaskWorkModel model)
        {
            string note1 = @"/" + model.FOrgNo1 + " " + model.FModal2 + " " + model.FModal3 + " " + model.FRequest1;//   "/2678 38*1kg JW790A(专用4) 华南,HW/2733 3*1kg JW790A 奥凯盛，样油200G ";
            string noteNew = string.Empty;
            Regex rg = new Regex(@"/(\d{4})");
            var matches = rg.Matches(note1);
            foreach (Match item in matches)
            {
                noteNew+=_work.GetProductiveTaskListFNote(date, model.FICMONo, item.Groups[1].ToString());
            }
           
            return noteNew;
        }
    }

    public class CBTypeModel
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public string NameCount { get; set; }

    }
}

