﻿using Bll.Services;
using Common;
using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Ui.View.InfoWindow;
using Expression = System.Linq.Expressions.Expression;
using wf = System.Windows.Forms;

namespace Ui.View.IndexPage
{
    /// <summary>
    /// ProductionDeptLabelPrintPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionDeptLabelPrintPage : Page
    {
        //ObservableCollection<LabelPrintHistoryModel> HistoryRecords;
        List<LabelPrintHistoryModel> lists;
        readonly static string printTemplatesFolderPath = ConfigurationManager.AppSettings["PrintTemplatesFolderPath"];
        readonly static string printerSelectedIndex = ConfigurationManager.AppSettings["PrinterSelectedIndex"];
        readonly static string orientationIndex = ConfigurationManager.AppSettings["OrientationIndex"];
        readonly static UserModel user = (MemoryCache.Default["UserCache"] as UserCacheModel).User;
        List<string> PrinterNames;

        List<QuerySchemaCurrentStatusModel> schemaList;
        List<QuerySchemaCurrentStatusModel> pageSizeList;
        // 用来识别是单击还是双击
        int i = 0; bool doubleClick = false;

        // 当前打印纸张
        string currentPageSize = string.Empty;

        // 用来识别是由双击方案按钮生成的数据还是单独打印的数据
        int globalSchemaId = 0;

        public ProductionDeptLabelPrintPage()
        {
            InitializeComponent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //将选择索引写入配置文件
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (CbPrinterName.SelectedIndex > -1)
            {
                cfa.AppSettings.Settings["PrinterSelectedIndex"].Value = CbPrinterName.SelectedIndex.ToString();
            }

            if (CbOrientation.SelectedIndex > -1)
            {
                cfa.AppSettings.Settings["OrientationIndex"].Value = CbOrientation.SelectedIndex.ToString();
            }

            cfa.Save();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PrinterNames = PrintHelper.GetComputerPrinter();
            InitializeComponentDefaultValue();
            this.MainGrid.Height = SystemParameters.PrimaryScreenHeight - 160;
        }

        private void InitializeComponentDefaultValue()
        {
            DateTime date = this.DP1.SelectedDate.Value;
            Task.Factory.StartNew(() =>
            {
                lists = GetDataFromDatabase(date);
                Application.Current.Dispatcher?.Invoke(new Action(() =>
               {
                   if (lists.Count == 0)
                       MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintHistoryModel>(new List<LabelPrintHistoryModel> { new LabelPrintHistoryModel { ProductionModel = $"请先审核 {date.ToString("yyyy-MM-dd")}", Label = " 生产任务清单" } });
                   else
                       MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintHistoryModel>(lists); //HistoryRecords;
                   this.TbSum.Text = lists.Count().ToString();
               }));
            });

            SetQuerySchemaBtn(user.ID);



            //GetDataFromDatabase(date);
            globalSchemaId = 0;
            //MainDataGrid.DataContext = HistoryRecords;
            this.tbFolderPath.Text = printTemplatesFolderPath;
            this.CbPrintTemplate.ItemsSource = FileHelper.GetTenderPrintTemplates(printTemplatesFolderPath);
            this.CbPrintTemplate.SelectedIndex = 0;

            this.CbPrinterName.ItemsSource = PrinterNames;
            this.CbPrinterName.SelectedIndex = string.IsNullOrEmpty(printerSelectedIndex) ? 0 : int.Parse(printerSelectedIndex);

            this.CbOrientation.ItemsSource = new List<string> { "横向", "纵向" };
            this.CbOrientation.SelectedIndex = string.IsNullOrEmpty(orientationIndex) ? 0 : int.Parse(orientationIndex);

        }

        private void ClearValue()
        {
            this.tbBatchNo.Text = string.Empty;
            this.tbLabel.Text = string.Empty;
            this.tbOrgID.Text = string.Empty;
            this.tbProductionModel.Text = string.Empty;
            this.tbSafeCode.Text = string.Empty;
            this.cbSampleOil.IsChecked = false;
        }

        private void DP1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DP1.SelectedDate != null && this.IsLoaded)
            {
                ClearValue();
                DateTime date = this.DP1.SelectedDate.Value;
                lists = GetDataFromDatabase(date);
                if (lists.Count == 0)
                    MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintHistoryModel>(new List<LabelPrintHistoryModel> { new LabelPrintHistoryModel { ProductionModel = $"请先审核 {date.ToString("yyyy-MM-dd")}", Label = " 生产任务清单" } });
                else
                    MainDataGrid.ItemsSource = new ObservableCollection<LabelPrintHistoryModel>(lists); //HistoryRecords;
                this.TbSum.Text = lists.Count().ToString();
            }
        }

        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            var filterData = GetFilteredItemSource();
            this.MainDataGrid.ItemsSource = filterData;
            this.TbSum.Text = filterData.Count().ToString();
        }

        private void BtnSpecialRequest_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintSpecialRequestWindow window = new LabelPrintSpecialRequestWindow();
            window.RefreshEvent += RefreshDataGrid;
            window.Owner = System.Windows.Window.GetWindow(this);
            window.ShowDialog();
        }

        private void BtnPrintAdjustment_Click(object sender, RoutedEventArgs e)
        {
            PrintCommonAdjustmentView view = new PrintCommonAdjustmentView();
            view.ShowDialog();
            view.Owner = System.Windows.Window.GetWindow(this);

            //LabelPrintCommonAdjustmentWindow window = new LabelPrintCommonAdjustmentWindow(user);
            //window.RefreshEvent += RefreshDataGrid;
            //window.Owner = System.Windows.Window.GetWindow(this);
            //window.ShowDialog();

          
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintCurrentPrintDataWindow window = new LabelPrintCurrentPrintDataWindow(user, this.DP1.SelectedDate.Value, new PrintSchemaParameterModel
            {
                UserId = user.ID,
                SchemaId = globalSchemaId,
                TemplateFullName = this.CbPrintTemplate.Text,
                TemplateFileName = Path.GetFileName(this.CbPrintTemplate.Text),
                Orientation = this.CbOrientation.Text,
                PrinterName = this.CbPrinterName.Text,
                FolderPath = this.tbFolderPath.Text
            });
            window.RefreshEvent += RefreshDataGrid;
            window.Owner = System.Windows.Window.GetWindow(this);
            window.ShowDialog();
        }

        public void RefreshDataGrid()
        {
            Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher?.Invoke(new Action(() =>
                {
                    lists = GetDataFromDatabase(this.DP1.SelectedDate.Value);
                    var filterData = GetFilteredItemSource();
                    this.MainDataGrid.ItemsSource = filterData;
                    this.TbSum.Text = filterData.Count().ToString();
                }));
            });
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (this.CbPrintTemplate.Text.Length == 0 || this.CbPrinterName.Text.Length == 0 || this.DP1.SelectedDate == null)
            {
                MessageBox.Show("请选择日期、模板以及Tender打印机");
                return;
            }
            //var printHelp = new PrintHelper(user, this.DP1.SelectedDate);

            /* 获取打印数据和配置 */
            var data = new LabelPrintDAL().GetPrintResultRecord(this.DP1.SelectedDate.Value, user.ID, "未打印").OrderBy(m => m.PrintOrder).ToList();
            var config = new PrintSchemaParameterModel
            {
                UserId = user.ID,
                SchemaId = globalSchemaId,
                TemplateFullName = this.CbPrintTemplate.Text,
                TemplateFileName = Path.GetFileName(this.CbPrintTemplate.Text),
                Orientation = this.CbOrientation.Text,
                PrinterName = this.CbPrinterName.Text,
                FolderPath = this.tbFolderPath.Text
            };
            string r = new PrintHelper().PrintLabel(config, data);
            if (string.IsNullOrEmpty(r))
            {
                // 打印参数插入数据库
                if (globalSchemaId > 0)
                {
                    // 将对应的按钮的背景色设置为绿色
                    var btn = (Button)this.scrollViewer.FindName("Btn" + globalSchemaId.ToString());
                    btn.Foreground = System.Windows.Media.Brushes.GhostWhite;
                    var border = (Border)btn.Template.FindName("back", btn);
                    border.Background = System.Windows.Media.Brushes.Green;

                    var message = new LabelPrintService().SavePrintSchemaParameter(config);
                    if (!string.IsNullOrEmpty(r))
                    {
                        MessageBox.Show(r);
                        return;
                    }
                }
                RefreshDataGrid();
                globalSchemaId = 0;
                MessageBox.Show("打印成功");
            }
            else
            {
                MessageBox.Show(r);
            }

        }

        private void BtnOilPrint_Click(object sender, RoutedEventArgs e)
        {
            if (this.CbPrintTemplate.Text.Length == 0 || this.CbPrinterName.Text.Length == 0 || this.DP1.SelectedDate == null)
            {
                MessageBox.Show("请选择日期、模板以及Tender打印机");
                return;
            }

            /* 获取打印数据和配置 */
            var data = new LabelPrintDAL().GetPrintResultRecord(this.DP1.SelectedDate.Value, user.ID, "未打印").OrderBy(m => m.PrintOrder).ToList();
            var config = new PrintSchemaParameterModel
            {
                UserId = user.ID,
                SchemaId = globalSchemaId,
                TemplateFullName = this.CbPrintTemplate.Text,
                TemplateFileName = Path.GetFileName(this.CbPrintTemplate.Text),
                Orientation = this.CbOrientation.Text,
                PrinterName = this.CbPrinterName.Text,
                FolderPath = this.tbFolderPath.Text
            };
            string r = new PrintHelper().PrintLabelSampleOil(config, data);
            if (string.IsNullOrEmpty(r))
            {
                RefreshDataGrid();
                MessageBox.Show("打印成功");
            }
            else
            {
                MessageBox.Show(r);
            }
        }

        public List<LabelPrintHistoryModel> GetDataFromDatabase(DateTime date)
        {
            return new LabelPrintService().GetAllLabelPrintHistoryDataByDate(date, user.ID);
        }


        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            var itemsSource = this.MainDataGrid.ItemsSource;
            foreach (LabelPrintHistoryModel entry in itemsSource)
            {
                if (!entry.IsPassed)
                    continue;
                entry.IsChecked = true;
            }
        }

        private void CheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            var itemsSource = this.MainDataGrid.ItemsSource;
            foreach (LabelPrintHistoryModel entry in itemsSource)
            {
                entry.IsChecked = false;
            }
        }

        public IEnumerable<LabelPrintHistoryModel> GetFilteredItemSource()
        {

            var expression = GetFilterExpression(new List<LabelPrintHistoryModel> {
                new LabelPrintHistoryModel{
                    OrgID=this.tbOrgID.Text.ToUpper(),
                    ProductionModel=this.tbProductionModel.Text.ToUpper(),
                    Label=this.tbLabel.Text.ToUpper(),
                    BatchNo=this.tbBatchNo.Text.ToUpper(),
                    SafeCode= this.tbSafeCode.Text.ToUpper(),
                    SpecificationValueBegin = decimal.TryParse(this.tbNetWeightBegin.Text, out decimal result) ? result: 0,
                    SpecificationValueEnd= decimal.TryParse(this.tbNetWeightEnd.Text, out decimal r) ? r : (decimal)999999.99,
                    SampleOilIsChecked = this.cbSampleOil.IsChecked.Value
                }
            });
            if (expression != null)
            {
                return new ObservableCollection<LabelPrintHistoryModel>(lists.AsQueryable().Where(expression));
            }
            return new ObservableCollection<LabelPrintHistoryModel>(lists);
        }

        private void BtnAddData_Click(object sender, RoutedEventArgs e)
        {
            List<int> ids = new List<int>();
            //遍历checkbox获取对应的数据ID，批量插入x打印表
            var itemsSource = this.MainDataGrid.ItemsSource;
            //(ObservableCollection<LabelPrintHistoryModel>)
            foreach (LabelPrintHistoryModel entry in itemsSource)
            {
                if (entry.IsChecked)
                {
                    ids.Add(entry.ProductiveTaskListID);
                    //entry.SelectCount += 1;
                    //entry.SelectTotalCount += entry.LastPrintCount;
                }
            }
            var r = new LabelPrintService(this.DP1.SelectedDate.Value, user).AddCurrentPrintData(ids, this.DP1.SelectedDate.Value);
            if (r.Length > 0)
            {
                MessageBox.Show(r);
                lists = GetDataFromDatabase(this.DP1.SelectedDate.Value);
                var filterData = GetFilteredItemSource();
                this.MainDataGrid.ItemsSource = filterData;
                this.TbSum.Text = filterData.Count().ToString();
            }
            else
            {
                MessageBox.Show("没有新数据生成");
            }

        }

        private void BtnPrintTemplateFolder_Click(object sender, RoutedEventArgs e)
        {
            wf.FolderBrowserDialog fbd = new wf.FolderBrowserDialog();

            if (fbd.ShowDialog() == wf.DialogResult.OK)
            {

                //将数据写入配置文件
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings["PrintTemplatesFolderPath"].Value = fbd.SelectedPath;
                cfa.Save();
                this.tbFolderPath.Text = fbd.SelectedPath;
                this.CbPrintTemplate.ItemsSource = FileHelper.GetTenderPrintTemplates(fbd.SelectedPath);
            }
        }

        private void BtnTemplateRef_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintTemplateRefWindow window = new LabelPrintTemplateRefWindow
            {
                Owner = System.Windows.Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void BtnRefreshTemplate_Click(object sender, RoutedEventArgs e)
        {
            this.CbPrintTemplate.ItemsSource = FileHelper.GetTenderPrintTemplates(this.tbFolderPath.Text);
            MessageBox.Show("模板刷新成功");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void BtnSchemeConfig_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintSchemaConfigWindow window = new LabelPrintSchemaConfigWindow(user)
            {
                Owner = System.Windows.Window.GetWindow(this)
            };
            window.RefreshEvent += () => { SetQuerySchemaBtn(user.ID); };
            window.ShowDialog();

        }

        private void BtnRefreshPrinter_Click(object sender, RoutedEventArgs e)
        {
            PrinterNames = PrintHelper.GetComputerPrinter();
            this.CbPrinterName.ItemsSource = PrinterNames;
            this.CbPrinterName.SelectedIndex = string.IsNullOrEmpty(printerSelectedIndex) ? 0 : int.Parse(printerSelectedIndex);
            MessageBox.Show("打印机刷新成功");
        }

        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            var itemsSource = this.MainDataGrid.ItemsSource;
            int count = 0;
            var data = new LabelPrintHistoryModel();
            foreach (LabelPrintHistoryModel entry in itemsSource)
            {
                if (entry.IsChecked)
                {
                    count++;
                    data = entry;
                    if (count > 1)
                    {
                        MessageBox.Show("只能修改一条数据");
                        return;
                    }
                }
            }
            if (count == 1)
            {
                LabelPrintPrintCountModifyWindow window = new LabelPrintPrintCountModifyWindow(data);
                window.Owner = System.Windows.Window.GetWindow(this);
                //window.RefreshEvent += () => { /* RefreshDataGrid();*/ };
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("请选择数据再修改");
            }

        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            LabelPrintHelpWindow window = new LabelPrintHelpWindow
            {
                Owner = System.Windows.Window.GetWindow(this)
            };
            window.ShowDialog();
        }

        private void SetQuerySchemaBtn(int id)
        {
            var model = new LabelPrintService().GetQuerySchemaModelByUserId(user.ID).OrderBy(m => m.SchemaPageSize);

            if (model.Count() > 0)
            {
                this.spSchema.Children.Clear();
                string pageSize = string.Empty;

                foreach (QuerySchemaModel item in model)
                {
                    // 同一个页面
                    if (pageSize.Equals(item.SchemaPageSize))
                    {
                        var wrapPanel = (WrapPanel)this.spSchema.FindName(item.SchemaPageSize);
                        var btn = CreateCustomButtion(item);
                        wrapPanel.Children.Add(btn);
                        if (wrapPanel.FindName("Btn" + item.Id.ToString()) == null)
                        {
                            wrapPanel.RegisterName("Btn" + item.Id.ToString(), btn);
                        }
                    }
                    else // 不是同一组
                    {
                        pageSize = item.SchemaPageSize;
                        GroupBox groupBox = new GroupBox();
                        groupBox.Header = " " + item.SchemaPageSize + " ";
                        groupBox.Margin = new Thickness { Bottom = 15 };
                        groupBox.Padding = new Thickness { Bottom = 3, Top = 3, Left = 3, Right = 3 };
                        groupBox.BorderThickness = new Thickness { Bottom = 2, Top = 2, Left = 2, Right = 2 };

                        Binding color = new Binding("PageSizeStatus");
                        color.Converter = FindResource("PageSizeStatusConverter") as IValueConverter;
                        groupBox.SetBinding(GroupBox.BorderBrushProperty, "PageSizeStatus");
                        groupBox.SetBinding(GroupBox.ForegroundProperty, "PageSizeStatus");
                        // groupBox.Name =  item.SchemaPageSize; // 如此赋值FINDNAME找不到
                        WrapPanel wrapPanel = new WrapPanel();
                        wrapPanel.Name = item.SchemaPageSize;
                        var btn = CreateCustomButtion(item);
                        wrapPanel.Children.Add(btn);
                        groupBox.Content = wrapPanel;
                        this.spSchema.Children.Add(groupBox);
                        if (this.spSchema.FindName(item.SchemaPageSize) == null)
                        {
                            this.spSchema.RegisterName(item.SchemaPageSize, wrapPanel);
                        }
                        if (wrapPanel.FindName("Btn" + item.Id.ToString()) == null)
                        {
                            wrapPanel.RegisterName("Btn" + item.Id.ToString(), btn);
                        }
                    }
                }

            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int schemaId = int.Parse(button.Name.Replace("Btn", ""));
            string schemaName = (sender as Button).ToolTip.ToString();
            var pageSize = (((sender as Button).Parent) as WrapPanel).Name;
            i += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; SingleClick(schemaId, schemaName); doubleClick = false; };
            timer.IsEnabled = true;
            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                doubleClick = true;

                //双击时执行的代码
                DoubleClick(schemaId, schemaName, button.Tag.ToString(), pageSize);
            }
        }

        private void SingleClick(int schemaId, string schemaName)
        {
            if (!doubleClick)
            {
                QuerySchemaEntryModel entryModel = GetQueryModel(schemaId);

                if (string.IsNullOrWhiteSpace(entryModel.OrgId) && string.IsNullOrWhiteSpace(entryModel.Label) && string.IsNullOrWhiteSpace(entryModel.BatchNo) && string.IsNullOrWhiteSpace(entryModel.ProductionModel) && string.IsNullOrWhiteSpace(entryModel.SafeCode))
                {
                    MessageBox.Show("必须添加一个或多个条件，同时筛选");
                    return;
                }

                string exists = new LabelPrintService().IfEntryExists(user.ID, entryModel);
                if (exists != null)
                {
                    MessageBox.Show(exists);
                    return;
                }

                string message = string.Empty;
                if (!string.IsNullOrWhiteSpace(entryModel.OrgId))
                    message += "  客户ID 包含 【" + entryModel.OrgId + "】\r\n";
                if (!string.IsNullOrWhiteSpace(entryModel.Label))
                    message += "  标签型号 包含 【" + entryModel.Label + "】\r\n";
                if (!string.IsNullOrWhiteSpace(entryModel.BatchNo))
                    message += "  批号 包含【" + entryModel.BatchNo + "】\r\n";
                if (!string.IsNullOrWhiteSpace(entryModel.ProductionModel))
                    message += "  产品型号 包含 【" + entryModel.ProductionModel + "】\r\n";
                if (!string.IsNullOrWhiteSpace(entryModel.SafeCode))
                    message += "  安全标签 包含 【" + entryModel.SafeCode + "】\r\n";
                message += $"  净重 【 {entryModel.SpecificationValueBegin}】 到 【{entryModel.SpecificationValueEnd}】 \r\n";

                string con = checkBox.IsChecked.Value ? "-------------排除条件-------" : " 【筛选条件】";
                MessageBoxResult result = MessageBox.Show($" 类别：{con} \r\n 方案: 【 {schemaName} 】 \r\n\r\n {message} ", "温馨提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string r = new LabelPrintService().AddQuerySchemaEntry(entryModel);
                    if (r == null)
                    {
                        MessageBox.Show("添加成功");
                    }
                    else
                    {
                        MessageBox.Show(r);
                    }
                }
            }
        }

        private void DoubleClick(int schemaId, string schemaName, string templateFullName, string pageSize)
        {
            MessageBoxResult result = MessageBox.Show($"生成【 {schemaName} 】 全部数据到待打印记录 \r\n 生产日期：【 {this.DP1.SelectedDate.Value} 】", "温馨提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                #region 设置界面颜色表现
                var btn = (Button)this.scrollViewer.FindName("Btn" + schemaId.ToString());
                btn.Foreground = System.Windows.Media.Brushes.Red;
                #endregion

                if (!string.IsNullOrEmpty(currentPageSize) && currentPageSize != pageSize)
                {
                    MessageBox.Show($" 当前方案纸张【 {pageSize} 】和之前的不一样，请确认打印纸张 ");
                    currentPageSize = pageSize;
                    return;
                }
                currentPageSize = pageSize;
                // 获取查询明细，组合条件
                var schemaEntries = new LabelPrintService().GetQuerySchemaEntryBySchemaId(schemaId);
                if (schemaEntries.Count() == 0)
                {
                    MessageBox.Show("该方案没有设置查询条件方案");
                    return;
                }
                var shemaentries1 = schemaEntries.Where(m => !m.IsConditionOut).ToList(); // 正常条件
                var shemaentries2 = schemaEntries.Where(m => m.IsConditionOut).ToList(); // 排除条件
                var ids1 = GetProductiveTaskListIds(shemaentries1);
                var ids2 = GetProductiveTaskListIds(shemaentries2);
                var ids = ids1.Except(ids2).ToList();
                if (ids.Count() > 0)
                {
                    // 将对应的ID，插入到待打印表，同步界面
                    globalSchemaId = schemaId;
                    string message = new LabelPrintService(this.DP1.SelectedDate.Value, user).AddCurrentPrintDataBatch(ids, this.DP1.SelectedDate.Value);

                    if (File.Exists(templateFullName))
                    {
                        string path = Path.GetDirectoryName(templateFullName);
                        tbFolderPath.Text = path;
                        CbPrintTemplate.ItemsSource = FileHelper.GetTenderPrintTemplates(path);
                        CbPrintTemplate.Text = templateFullName;
                    }
                    else
                    {
                        message += "模板路径不存在，请手动选择目录-- > 模板 \r\n";
                    }

                    PrintSchemaParameterModel config = new LabelPrintService().GetLastPrintSchemaParameter(user.ID, schemaId);
                    if (config != null) //如果存在，则覆盖
                    {
                        CbPrintTemplate.Text = config.TemplateFullName;
                        CbOrientation.Text = config.Orientation;
                        CbPrinterName.Text = config.PrinterName;
                        tbFolderPath.Text = config.FolderPath;
                        message += ValidateConfig(config);
                    }
                    RefreshDataGrid();
                    MessageBox.Show(message);


                }
                else
                {
                    MessageBox.Show(" 当天没有符合条件的数据");
                    return;
                }

            }
        }

        public List<int> GetProductiveTaskListIds(List<QuerySchemaEntryModel> schemaEntries)
        {
            List<LabelPrintHistoryModel> models = new List<LabelPrintHistoryModel>();
            foreach (var item in schemaEntries)
            {
                models.Add(new LabelPrintHistoryModel
                {
                    OrgID = item.OrgId,
                    BatchNo = item.BatchNo,
                    Label = item.Label,
                    ProductionModel = item.ProductionModel,
                    SafeCode = item.SafeCode,
                    SpecificationValueBegin = item.SpecificationValueBegin,
                    SpecificationValueEnd = item.SpecificationValueEnd
                });
            }

            var expression = GetFilterExpression(models);
            // 2.筛选过滤条件，获取所有唯一ID(当天打印数据 HistoryRecords )
            if (expression != null)
            {
                return lists.AsQueryable().Select(m => new LabelPrintHistoryModel
                {
                    OrgID = m.OrgID.ToUpper(),
                    ProductionModel = m.ProductionModel.ToUpper(),
                    Label = m.Label.ToUpper(),
                    BatchNo = m.BatchNo.ToUpper(),
                    ProductiveTaskListID = m.ProductiveTaskListID,
                    SafeCode = m.SafeCode,
                    SpecificationValueBegin = m.SpecificationValueBegin,
                    SpecificationValueEnd = m.SpecificationValueEnd
                }
                ).Where(expression).Select(m => m.ProductiveTaskListID).ToList<int>();
            }
            else
            {
                return new List<int> { -1 };
            }

        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var model = (sender as DataGridRow).Item as LabelPrintHistoryModel;
            if (!model.IsPassed)
                return;
            model.IsChecked = !model.IsChecked;
            model.Selected ^= 1;
        }

        private void BtnGenPrintData_Click(object sender, RoutedEventArgs e)
        {
            int rowCount = (int)new ProductiveTaskListService().AuditProductiveTaskList(this.DP1.SelectedDate.Value);
            if (rowCount > 0)
            {
                RefreshDataGrid();
                MessageBox.Show($"生成成功...");
            }
            else
            {
                MessageBox.Show($"生成失败，请联系管理员,,,");
            }
        }

        public QuerySchemaEntryModel GetQueryModel(int schemaId)
        {

            return new QuerySchemaEntryModel
            {
                SchemaId = schemaId,
                OrgId = string.IsNullOrWhiteSpace(this.tbOrgID.Text) ? "" : this.tbOrgID.Text.ToUpper(),
                Label = string.IsNullOrWhiteSpace(this.tbLabel.Text) ? "" : this.tbLabel.Text.ToUpper(),
                BatchNo = string.IsNullOrWhiteSpace(this.tbBatchNo.Text) ? "" : this.tbBatchNo.Text.ToUpper(),
                ProductionModel = string.IsNullOrWhiteSpace(this.tbProductionModel.Text) ? "" : this.tbProductionModel.Text.ToUpper(),
                IsConditionOut = this.checkBox.IsChecked.Value,
                SafeCode = string.IsNullOrWhiteSpace(this.tbSafeCode.Text) ? "" : this.tbSafeCode.Text.ToUpper(),
                SpecificationValueBegin = decimal.TryParse(this.tbNetWeightBegin.Text, out decimal result) ? result : 0,
                SpecificationValueEnd = decimal.TryParse(this.tbNetWeightEnd.Text, out decimal r) ? r : (decimal)999999.99
            };
        }

        #region 动态拼接lamda表达式

        public static Expression<Func<LabelPrintHistoryModel, bool>> GetFilterExpression(List<LabelPrintHistoryModel> entries)
        {
            Expression<Func<LabelPrintHistoryModel, bool>> condition = null;
            try
            {
                if (entries != null && entries.Count > 0)
                {
                    foreach (LabelPrintHistoryModel entryModel in entries)
                    {
                        Expression<Func<LabelPrintHistoryModel, bool>> tempCondition = CreateLambda(entryModel);
                        condition = condition == null ? tempCondition : condition.Or(tempCondition);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return condition;
        }

        public static Expression<Func<LabelPrintHistoryModel, bool>> CreateLambda(LabelPrintHistoryModel entryModel)
        {
            Expression<Func<LabelPrintHistoryModel, bool>> condition;
            if (entryModel.SampleOilIsChecked)
                condition = s => s.SpecificationValue >= entryModel.SpecificationValueBegin && s.SpecificationValue <= entryModel.SpecificationValueEnd && s.SampleOilPrintCount > 0;
            else
                 condition = s => s.SpecificationValue >= entryModel.SpecificationValueBegin && s.SpecificationValue <= entryModel.SpecificationValueEnd;
            //var parameter = Expression.Parameter(typeof(T), "p");//创建参数i
            //var constant = Expression.Constant(filterCondition.value);//创建常数
            //MemberExpression member = Expression.PropertyOrField(parameter, filterCondition.column);        
            //return GetExpressionWithMethod<T>("Contains", filterCondition);
            /*
                ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
                MethodCallExpression methodExpression = GetMethodExpression(methodName, filterCondition.column, filterCondition.value, parameterExpression);
                return Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
             */
            if (!string.IsNullOrEmpty(entryModel.OrgID))
                condition = AndCondition("OrgID", entryModel.OrgID, condition);

            if (!string.IsNullOrEmpty(entryModel.BatchNo))
                condition = AndCondition("BatchNo", entryModel.BatchNo, condition);

            if (!string.IsNullOrEmpty(entryModel.Label))
                condition = AndCondition("Label", entryModel.Label, condition);

            if (!string.IsNullOrEmpty(entryModel.ProductionModel))
                condition = AndCondition("ProductionModel", entryModel.ProductionModel, condition);
            if (!string.IsNullOrEmpty(entryModel.SafeCode))
                condition = AndCondition("SafeCode", entryModel.SafeCode, condition);

            return condition;
        }


        private static Expression<Func<T, bool>> AndCondition<T>(string name, string value, Expression<Func<T, bool>> condition = null)
        {

            ParameterExpression p1 = Expression.Parameter(typeof(T), "p");
            GetMethodNameAndValue(value, out string methodName, out string methodValue);
            MethodCallExpression m1 = GetMethodExpression(methodName, name, methodValue, p1);
            return condition == null ? Expression.Lambda<Func<T, bool>>(m1, p1) : condition.And(Expression.Lambda<Func<T, bool>>(m1, p1));
        }

        private static void GetMethodNameAndValue(string inValue, out string methodName, out string outValue)
        {
            methodName = "Contains";
            outValue = inValue;
            if (inValue.StartsWith("%"))
            {
                methodName = "EndsWith";
                outValue = inValue.Replace("%", "");
            }
            else if (inValue.EndsWith("%"))
            {
                methodName = "StartsWith";
                outValue = inValue.Replace("%", "");
            }
            else if (inValue.StartsWith("="))
            {
                methodName = "Equals";
                outValue = inValue.Substring(1);
            }
        }



        private static MethodCallExpression GetMethodExpression(string methodName, string propertyName, string propertyValue, ParameterExpression parameterExpression)
        {
            var ss = typeof(int).GetMethods();
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            return Expression.Call(propertyExpression, method, someValue);
        }

        #endregion

        private void BtnGenQuerySchema_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder("成功生成方案 \r\n 纸张尺寸 \t 方案名称 \r\n");
            if (string.IsNullOrEmpty(this.tbFolderPath.Text))
            {
                MessageBox.Show("请先选择模板所在文件夹");
                return;
            }

            if (this.CbPrintTemplate.ItemsSource == null || !this.CbPrintTemplate.HasItems)
            {
                MessageBox.Show("没有模板可供生成查询方案");
                return;
            }

            // 获取模板名称，将新的模板名称加入到方案名称，有的则排除
            var userSchemas = new LabelPrintService().GetMySchema(user.ID).OrderBy(m => m.SchemaSeq).ToList();

            List<QuerySchemaModel> newSchema = new List<QuerySchemaModel>();

            foreach (var s in this.CbPrintTemplate.Items)
            {
                string fileName = Path.GetFileName(s.ToString()).Replace(".btw", "");
                // 方案名称存在的话，不变，不存在的话，新增新的解决方案
                var exists = userSchemas.FirstOrDefault(m => m.TemplateFileName == fileName);
                if (exists == null)
                {
                    string pageSize = string.Empty; string schemaName = string.Empty;
                    try
                    {
                        pageSize = fileName.Split(new char[] { ' ' }, 2)[0];
                        schemaName = fileName.Split(new char[] { ' ' }, 2)[1];
                    }
                    catch
                    {
                        MessageBox.Show(" 生成方案失败： 所有文件模板必须以【空格】分割页面大小、方案名称");
                        return;
                    }

                    QuerySchemaModel newModel = new QuerySchemaModel
                    {
                        UserId = user.ID,
                        SchemaSeq = 0,
                        TemplateFullName = s.ToString(),
                        TemplateFileName = fileName,
                        SchemaName = schemaName,
                        SchemaPageSize = pageSize
                    };
                    sb.Append(pageSize + "\t");
                    sb.AppendLine(schemaName);
                    newSchema.Add(newModel);
                    // userSchemas.Insert(seq - 1, newModel);
                }
            }

            // 添加到数据库
            if (newSchema.Count > 1)
            {
                new LabelPrintService().BatchInsertSchema(newSchema, "SJUserQuerySchema");
                // 刷新panel
                SetQuerySchemaBtn(user.ID);
            }
            MessageBox.Show(sb.ToString());
        }

        public int GetContinuousSeq(List<QuerySchemaModel> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].SchemaSeq != i + 1)
                {
                    return i + 1;
                }
            }
            return names.Count == 0 ? 1 : names.Max(m => m.SchemaSeq) + 1;
        }

        public string ValidateConfig(PrintSchemaParameterModel config)
        {
            string r = "";
            if (!File.Exists(config.TemplateFullName))
            {
                r += "模板路径不存在，请手动选择目录--> 模板 \r\n";
            }

            if (config.Orientation != "横向" && config.Orientation != "纵向")
            {
                r += "打印方向不存在，请手动选择 打印方向 \r\n";
            }

            if (!PrinterNames.Contains(config.PrinterName))
            {
                r += "打印机错误，请手动选择 打印机 \r\n";
            }
            return r;
        }

        public double GetMainDataGridHeight(double height)
        {
            double result = 300;
            if (height > 1080)
                result = height * 0.7;
            else if (height == 1080)
                result = 700;
            else if (height >= 1000 && height < 1080)
                result = height * 0.6;
            else if (height >= 900 && height < 1000)
                result = height * 0.55;
            else if (height >= 800 && height < 900)
                result = height * 0.48;
            else if (height < 800)
                result = height * 0.43;
            return result;

        }

        private Button CreateCustomButtion(QuerySchemaModel model)
        {
            Button btn = new Button();
            btn.Name = "Btn" + model.Id.ToString();
            btn.ToolTip = model.SchemaName;
            btn.Content = System.Web.HttpUtility.HtmlDecode(model.SchemaName);
            btn.Style = FindResource("SchemaSeqButton") as Style;
            btn.Tag = model.TemplateFullName;
            btn.Click += Btn_Click;
            return btn;
        }

    }

}
