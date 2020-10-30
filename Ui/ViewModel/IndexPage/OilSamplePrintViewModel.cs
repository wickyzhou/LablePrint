using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class OilSamplePrintViewModel : BaseViewModel
    {
        private static readonly OilSampleService _oilSampleService = new OilSampleService();

        public OilSamplePrintViewModel()
        {


            OilSampleFlows = new ObservableCollection<OilSampleFlowModel>();
            OilSampleFlowLogs = new ObservableCollection<OilSampleFlowPrintLogModel>();
            OilSampleEntries = new ObservableCollection<OilSampleEntryModel>();
            OilSamplePaperNames = new ObservableCollection<PageSizeModel>();
            ExpressPaperNames = new ObservableCollection<PageSizeModel>();
            OilSampleEntrySelectedItem = new OilSampleEntryModel();
            InitData();


            ExpressPrintCommand = new DelegateCommand(PrintExpress);
            ExpressPrintConfigSaveCommand = new DelegateCommand(SaveExpressPrintConfig);

            OilSamplePrintCommand = new DelegateCommand(PrintOilSample);
            OilSamplePrintConfigSaveCommand = new DelegateCommand(SaveOilSamplePrintConfig);
            OilSampleFlowLogModifyCommand = new DelegateCommand(ModifyOilSampleFlowLog);
            OilSampleFlowSelectionChangedCommand = new DelegateCommand(ChangeOilSampleFlowSelection);
            OilSampleFlowLogSelectionChangedCommand = new DelegateCommand(ChangeOilSampleFlowLogSelection);
            OilSampleEntryModifyCommand = new DelegateCommand(ModifyOilSampleEntry);

            OilSampleTemplateSelectCommand = new DelegateCommand(SelectOilSampleTemplate);
            ExpressTemplateSelectCommand = new DelegateCommand(SelectExpressTemplate);

            TemplateSelectionChangedCommand = new DelegateCommand((obj) =>
            {
                OilSampleTemplateSelectedItem = (BarTenderTemplateModel)obj;
                OilSamplePrintConfig.TemplatePerPage = OilSampleTemplateSelectedItem.TemplatePerPage;
                OilSamplePrintConfig.TemplateFileName = OilSampleTemplateSelectedItem.TemplateFileName;
                OilSamplePrintConfig.TemplateFullName = OilSampleTemplateSelectedItem.TemplateFullName;
                OilSamplePrintConfig.TemplateFolderPath = OilSampleTemplateSelectedItem.TemplateFolderPath;
            });

            OilSampleEntryPrintLogModifyCommand = new DelegateCommand(ModifyOilSampleEntryPrintLog);
            OilSampleEntryCheckedCommand = new DelegateCommand(CheckOilSampleEntry);
            OilSampleEntryMergePrintCommand = new DelegateCommand(MergePrintOilSampleEntry);
            OilSampleEntryOrderPrintCommand = new DelegateCommand(OrderPrintOilSampleEntry);
            DealingFlowShowCommand = new DelegateCommand(ShowDealingFlow);
            DealedFlowShowCommand = new DelegateCommand(ShowDealedFlow);

            A4PrintCommand = new DelegateCommand((obj) =>
            {
                int printCount = 0;
                int previousCounts = 0;
          
                string msg = PrintHelper.VerifyPrintConfiguration(PrintConfiguration);
                if (string.IsNullOrEmpty(msg))
                {
                    if (OilSampleEntries.Count > 0)
                    {
                        int templatePerPage = PrintConfiguration.TemplateSelectedItem.TemplatePerPage;
                        var data = new ObservableCollection<OilSampleEntryModel>();
                        foreach (var item in OilSampleEntries)
                        {
                            if (item.CurrencyPrintCount > 0)
                            {

                                printCount += item.CurrencyPrintCount;

                                if (printCount >= templatePerPage)
                                {
                                    item.CurrencyPrintCount = templatePerPage - previousCounts;
                                    printCount = templatePerPage;
                                    data.Add(item);
                                    break;
                                }
                                else
                                {
                                    previousCounts += item.CurrencyPrintCount;
                                    data.Add(item);
                                }
                            }
                        }
                        if (printCount == 0 || printCount > templatePerPage)
                        {
                            MessageBox.Show("本流程明细已经全部打印完毕");
                            return;
                        }
                        //string result = new PrintService().BarTenderPrintA4(OilSampleEntries, PrintConfiguration, printCount);
                        //if (string.IsNullOrEmpty(result))
                        //{
                        //    // 重新加载明细
                        //    OilSampleEntries.Clear();
                        //    OilSampleFlowLogs.Clear();
                        //    _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
                        //    _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
                        //}
                        //else 
                        //    MessageBox.Show(result);
                    }
                    else
                        MessageBox.Show("先选择行数据，【CTRL】或【SHFIT】 多选行");
                }
                else
                    MessageBox.Show(msg);
            });

            PrintConfigurationSaveBaseCommand = new DelegateCommand((obj) =>
            {
                string msg = PrintHelper.VerifyPrintConfiguration(PrintConfiguration);
                if (string.IsNullOrEmpty(msg))
                {
                    int id = CommonService.SaveBarTenderPrintConfigXX(PrintConfiguration);
                    if (id > 0)
                    {
                        PrintConfiguration.Id = id;
                        MessageBox.Show("保存成功");
                    }

                    else
                        MessageBox.Show("保存失败,请联系管理员");
                }
                else
                    MessageBox.Show(msg);

            });

            TemplateSelectBaseCommand = new DelegateCommand((obj) =>
            {
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PirntTemplates = PrintHelper.GetTenderPrintA4Templates(fbd.SelectedPath);
                }
            });
        }

        private void OrderPrintOilSampleEntry(object obj)
        {
            int printCount = 0;
            int previousCounts = 0;
            if (string.IsNullOrEmpty(OilSamplePrintConfig.PrinterName) || OilSamplePrintConfig.TemplateFileName == null)
            {
                MessageBox.Show("请选择模板和打印机");
                return;
            }

            var data = new ObservableCollection<OilSampleEntryModel>();
            foreach (var item in OilSampleEntries)
            {
                if (item.CurrencyPrintCount > 0)
                {

                    printCount += item.CurrencyPrintCount;

                    if (printCount >= 4)
                    {
                        item.CurrencyPrintCount = 4 - previousCounts;
                        printCount = 4;
                        data.Add(item);
                        break;
                    }
                    else
                    {
                        previousCounts += item.CurrencyPrintCount;
                        data.Add(item);
                    }
                }
            }
            if (printCount == 0 || printCount > 4)
            {
                MessageBox.Show("本流程明细已经全部打印完毕");
                return;
            }


            var r = new PrintService().BarTenderOilSampleEntryMergePrint(OilSamplePrintConfig, data, printCount, OilSampleTemplates);
            if (r)
            {
                // 重新加载明细
                OilSampleEntries.Clear();
                OilSampleFlowLogs.Clear();
                _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
                _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
                MessageBox.Show("打印成功");
            }
            else
                MessageBox.Show("打印过程出错，请联系管理员 ");

        }


        private void ShowDealedFlow(object obj)
        {
            OilSampleFlows.Clear();
            _oilSampleService.GetOilSampleDealedFlow().ToList().ForEach(x => OilSampleFlows.Add(x));
        }

        private void ShowDealingFlow(object obj)
        {
            OilSampleFlows.Clear();
            _oilSampleService.GetOilSampleFlow().ToList().ForEach(x => OilSampleFlows.Add(x));
        }




        private void MergePrintOilSampleEntry(object obj)
        {
            int printCount = 0;
            if (string.IsNullOrEmpty(OilSamplePrintConfig.PrinterName) || OilSamplePrintConfig.TemplateFileName == null)
            {
                MessageBox.Show("请选择模板和打印机");
                return;
            }

            if (OilSampleEntries.Count == 0)
            {
                MessageBox.Show($"请先选择样油明细");
                return;
            }

            var data = new ObservableCollection<OilSampleEntryModel>();
            foreach (var item in OilSampleEntries)
            {
                if (item.IsChecked && item.CurrencyPrintCount > 0)
                {
                    printCount += item.CurrencyPrintCount;
                    data.Add(item);
                }
            }

            if (OilSampleEntries.Count > 1 && printCount > 4)
            {
                MessageBox.Show($"多条明细最多选择打印4小张");
                return;
            }


            if (printCount == 0)
            {
                MessageBox.Show($"打印张数为0，请修改数量");
                return;
            }

            if (printCount > 4)
                printCount = 4;

            var r = new PrintService().BarTenderOilSampleEntryMergePrint(OilSamplePrintConfig, data, printCount, OilSampleTemplates);
            if (r)
            {
                // 重新加载明细
                OilSampleEntries.Clear();
                OilSampleFlowLogs.Clear();
                _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
                _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
                MessageBox.Show("打印成功");
            }
            else
                MessageBox.Show("打印过程出错，请联系管理员 ");
        }

        private void CheckOilSampleEntry(object obj)
        {
            // OilSampleEntrySelectedItem.IsChecked = !OilSampleEntrySelectedItem.IsChecked;
        }

        private void ModifyOilSampleEntryPrintLog(object obj)
        {

            if (OilSampleEntrySelectedItem == null)
                return;
            var r = _oilSampleService.UpdateOilSampleFlowLog(OilSampleEntrySelectedItem.Id);
            if (r)
            {
                // 重新加载entries 和log
                OilSampleEntries.Clear();
                OilSampleFlowLogs.Clear();
                _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
                _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
                MessageBox.Show("该行打印张数已全部清0");
            }
            else
            {
                MessageBox.Show("清空日志失败，请联系管理员");
            }
        }

        private void ModifyOilSampleEntry(object obj)
        {
            if (OilSampleEntrySelectedItem != null)
            {
                OilSampleEntryModifyView edit = new OilSampleEntryModifyView();
                var cloneData = ObjectDeepCopyHelper<OilSampleEntryModel, OilSampleEntryModel>.Trans(OilSampleEntrySelectedItem);
                (edit.DataContext as OilSampleEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                {
                    edit.Close();
                    if (type == 1)
                    {
                        OilSampleEntrySelectedItem.CurrencyPrintCount = entry.CurrencyPrintCount;
                        OilSampleEntrySelectedItem.WeightPerBucket = entry.WeightPerBucket;
                        OilSampleEntrySelectedItem.TotalWeight = entry.TotalWeight;
                        OilSampleEntrySelectedItem.PrintTotalCount = entry.PrintTotalCount;
                        OilSampleEntrySelectedItem.ProductionModel = entry.ProductionModel;
                        OilSampleEntrySelectedItem.ProductionName = entry.ProductionName;
                    }
                });
                edit.ShowDialog();
            }
        }

        private void SelectExpressTemplate(object obj)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ExpressTemplates = CommonService.GetTenderPrintTemplates(fbd.SelectedPath);
            }
        }

        private void SelectOilSampleTemplate(object obj)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                OilSampleTemplates = CommonService.GetTenderPrintTemplates(fbd.SelectedPath);
            }
        }

        private void InitData()
        {
            _oilSampleService.GetOilSampleFlow().ToList().ForEach(x => OilSampleFlows.Add(x));
            _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
            ComputerPrinters = CommonService.GetComputerPrinters();

            GetOilSamplePrintConfig();
            GetExpressPrintConfig();
        }

        private void GetExpressPrintConfig()
        {
            ExpressPrintConfig = CommonService.GetBarTenderPrintConfigXX(User.ID, 3, HostName);

            if (ExpressPrintConfig != null)
            {
                //ExpressTemplateSelectedItem = ExpressPrintConfig.ExpressTemplateSelectedItem;

                //验证打印机和模板路径是否存在

                if (!File.Exists(ExpressPrintConfig.TemplateSelectedItem.TemplateFullName))
                {
                    MessageBox.Show(" 模板路径不存在，请手动选择模板目录 \r\n");
                    ExpressPrintConfig.TemplateSelectedItem = null;
                    return;
                }

                if (!ComputerPrinters.Contains(ExpressPrintConfig.PrinterName))
                {
                    MessageBox.Show("打印机错误或不存在，请手动选择 打印机 \r\n");
                    ExpressPrintConfig.PrinterName = null;
                    return;
                }

                ExpressTemplates = CommonService.GetTenderPrintTemplates(ExpressPrintConfig.TemplateSelectedItem.TemplateFolderPath);
            }
            else
            {
                ExpressPrintConfig = new BarTenderPrintConfigModelXX();
            }

        }

        private void GetOilSamplePrintConfig()
        {
            OilSampleTemplateSelectedItem = new BarTenderTemplateModel();

            OilSamplePrintConfig = CommonService.GetBarTenderPrintConfig(User.ID, 2, HostName);
            if (OilSamplePrintConfig != null)
            {
                OilSampleTemplates = CommonService.GetTenderPrintTemplates(OilSamplePrintConfig.TemplateFolderPath);

                //验证打印机和模板路径是否存在
                if (!File.Exists(OilSamplePrintConfig.TemplateFullName))
                {
                    MessageBox.Show(" 模板路径不存在，请手动选择模板目录 \r\n");
                    OilSampleTemplateSelectedItem = null;
                    OilSamplePrintConfig.TemplateFullName = null;
                    OilSamplePrintConfig.TemplateFileName = null;
                    OilSamplePrintConfig.TemplateFolderPath = null;
                    OilSamplePrintConfig.TemplatePerPage = 0;
                    return;
                }
                if (!ComputerPrinters.Contains(OilSamplePrintConfig.PrinterName))
                {
                    MessageBox.Show("打印机错误或不存在，请手动选择 打印机 \r\n");
                    OilSamplePrintConfig.PrinterName = null;
                    return;
                }
                OilSampleTemplateSelectedItem.TemplatePerPage = OilSamplePrintConfig.TemplatePerPage;
                OilSampleTemplateSelectedItem.TemplateFileName = OilSamplePrintConfig.TemplateFileName;
                OilSampleTemplateSelectedItem.TemplateFullName = OilSamplePrintConfig.TemplateFullName;
                OilSampleTemplateSelectedItem.TemplateFolderPath = OilSamplePrintConfig.TemplateFolderPath;
            }
            else
            {
                OilSamplePrintConfig = new BarTenderPrintConfigModel();
            }

        }

        private void ChangeOilSampleFlowSelection(object obj)
        {
            if (obj != null)
            {
                OilSampleFlowSelectedItem = (OilSampleFlowModel)obj;
                OilSampleEntries.Clear();
                _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
            }
        }

        private void ChangeOilSampleFlowLogSelection(object obj)
        {
            if (obj != null)
            {
                OilSampleFlowLogSelectedItem = (OilSampleFlowPrintLogModel)obj;
            }
        }

        private void ModifyOilSampleFlowLog(object obj)
        {
            if (OilSampleFlowLogSelectedItem != null)
            {
                if (OilSampleFlowLogSelectedItem.TypeId == 1)
                {
                    MessageBox.Show("快递单次数不允许修改");
                    return;
                }
                OilSampleFlowLogModifyView edit = new OilSampleFlowLogModifyView();
                var cloneData = ObjectDeepCopyHelper<OilSampleFlowPrintLogModel, OilSampleFlowPrintLogModel>.Trans(OilSampleFlowLogSelectedItem);
                (edit.DataContext as OilSampleFlowLogModifyViewModel).WithParam(cloneData, (type, entry) =>
                {
                    edit.Close();
                    if (type == 1)
                    {
                        var r = _oilSampleService.UpdateOilSampleFlowLog(entry);
                        if (r)
                        {
                            OilSampleFlowLogSelectedItem.PrintedCount = entry.PrintedCount;
                            //重新加载明细
                            if (OilSampleFlowSelectedItem != null)
                            {
                                OilSampleEntries.Clear();
                                _oilSampleService.GetOilSampleEntries(OilSampleFlowSelectedItem.Id).ToList().ForEach(x => OilSampleEntries.Add(x));
                            }
                        }
                    }
                });
                edit.ShowDialog();
            }
        }

        private void SaveOilSamplePrintConfig(object obj)
        {
            var m = CommonService.GetBarTenderPrintConfig(User.ID, 2, HostName);
            if (m == null)
            {
                if (OilSampleTemplateSelectedItem == null || string.IsNullOrEmpty(OilSamplePrintConfig.PrinterName))
                {
                    MessageBox.Show("请选择打印机和模板");
                    return;
                }
                OilSamplePrintConfig.UserId = User.ID;
                OilSamplePrintConfig.TemplateTypeId = 2;
                OilSamplePrintConfig.TemplateTypeName = "松润样油明细打印";
                OilSamplePrintConfig.HostName = HostName;
                OilSamplePrintConfig.TemplatePerPage = OilSampleTemplateSelectedItem.TemplatePerPage;
                OilSamplePrintConfig.TemplateFileName = OilSampleTemplateSelectedItem.TemplateFileName;
                OilSamplePrintConfig.TemplateFullName = OilSampleTemplateSelectedItem.TemplateFullName;
                OilSamplePrintConfig.TemplateFolderPath = OilSampleTemplateSelectedItem.TemplateFolderPath;
                OilSamplePrintConfig.TemplateTotalPage = OilSampleTemplateSelectedItem.TemplateTotalPage;
                var r = CommonService.InsertBarTenderPrintConfig(OilSamplePrintConfig);
                if (r > 0)
                {
                    OilSamplePrintConfig.Id = r;
                    MessageBox.Show("保存成功");
                }

                else
                    MessageBox.Show("保存失败,请联系管理员");
            }
            else
            {
                var r = CommonService.UpdateBarTenderPrintConfig(OilSamplePrintConfig);
                if (r)
                    MessageBox.Show("保存成功");
                else
                    MessageBox.Show("保存失败,请联系管理员");
            }

        }

        private void SaveExpressPrintConfig(object obj)
        {

            //var ss = ExpressPrintConfig;

            var m = CommonService.GetBarTenderPrintConfigXX(User.ID, 3, HostName);
            if (m == null)
            {
                ExpressPrintConfig.UserId = User.ID;
                ExpressPrintConfig.TemplateTypeId = 3;
                ExpressPrintConfig.TemplateTypeName = "松润样油快递单打印";
                ExpressPrintConfig.HostName = HostName;
                var r = CommonService.InsertBarTenderPrintConfigXX(ExpressPrintConfig);
                if (r > 0)
                {
                    ExpressPrintConfig.Id = r;
                    MessageBox.Show("保存成功");
                }
                else
                    MessageBox.Show("保存失败,请联系管理员");
            }
            else
            {
                var r = CommonService.UpdateBarTenderPrintConfigXX(ExpressPrintConfig);
                if (r)
                    MessageBox.Show("保存成功");
                else
                    MessageBox.Show("保存失败,请联系管理员");
            }
        }

        private void PrintOilSample(object obj)
        {

            if (OilSampleEntrySelectedItem == null)
                return;


            if (string.IsNullOrEmpty(OilSamplePrintConfig.PrinterName) || OilSamplePrintConfig.TemplateFileName == null)
            {
                MessageBox.Show("请选择模板和打印机");
                return;
            }

            if (OilSampleEntrySelectedItem.CurrencyPrintCount <= 0 || OilSampleEntrySelectedItem.CurrencyPrintCount > 4)
            {
                MessageBox.Show("一次最大打印张数为4");
                return;
            }

            var batchNo = new PrintService().BarTenderOilSamplePrint(OilSamplePrintConfig, OilSampleEntrySelectedItem);
            //var batchNo = "p020340";
            if (!string.IsNullOrEmpty(batchNo))
            {
                var log = new OilSampleFlowPrintLogModel
                {
                    FormsonId = OilSampleEntrySelectedItem.Id,
                    Title = OilSampleFlowSelectedItem.Title,
                    FormmainId = OilSampleEntrySelectedItem.FormmainId,
                    TypeId = OilSamplePrintConfig.TemplateTypeId,
                    TypeDesc = OilSamplePrintConfig.TemplateTypeName,
                    BatchNo = batchNo,
                    PrintCount = OilSampleEntrySelectedItem.CurrencyPrintCount,
                    PrintedCount = OilSampleEntrySelectedItem.PrintedCount + OilSampleEntrySelectedItem.CurrencyPrintCount,
                    EntryId = OilSampleEntrySelectedItem.EntryId

                };
                OilSampleEntrySelectedItem.PrintedCount = log.PrintedCount;
                OilSampleEntrySelectedItem.CurrencyPrintCount = OilSampleEntrySelectedItem.PrintTotalCount - log.PrintedCount;

                // 写日志
                int id = _oilSampleService.InsertOilSampleFlowLog(log);
                if (id > 0)
                {
                    log.Id = id;
                    OilSampleFlowLogs.Insert(0, log);
                    MessageBox.Show("打印成功");
                }
                else

                    MessageBox.Show("插入日志数据失败，请联系管理员");
            }
            else
            {
                MessageBox.Show($"打印失败");
            }
        }

        private void PrintExpress(object obj)
        {
            if (OilSampleFlowSelectedItem == null)
                return;

            if (string.IsNullOrEmpty(ExpressPrintConfig.PrinterName) || ExpressPrintConfig.TemplateSelectedItem == null)
            {
                MessageBox.Show("请选择模板和打印机");
                return;
            }


            var printResult = new PrintService().BarTenderExpressPrint(ExpressPrintConfig, OilSampleFlowSelectedItem.Id);
            //bool printResult = true;
            if (printResult)
            {
                var log = new OilSampleFlowPrintLogModel
                {
                    FormsonId = 0,
                    Title = OilSampleFlowSelectedItem.Title,
                    FormmainId = OilSampleFlowSelectedItem.Id,
                    TypeId = ExpressPrintConfig.TemplateTypeId,
                    TypeDesc = ExpressPrintConfig.TemplateTypeName,
                    BatchNo = "0",
                    PrintCount = 1,
                    PrintedCount = OilSampleFlowSelectedItem.ExpressPrintedCount + 1,
                    EntryId = 0
                };
                OilSampleFlowSelectedItem.ExpressPrintedCount = log.PrintedCount;

                // 写日志
                int id = _oilSampleService.InsertOilSampleFlowLog(log);
                if (id > 0)
                {
                    log.Id = id;
                    OilSampleFlowLogs.Insert(0, log);
                    MessageBox.Show("打印成功");
                }
                else

                    MessageBox.Show("插入日志数据失败，请联系管理员");
            }
            else
            {
                MessageBox.Show($"打印失败");
            }
        }

        public DelegateCommand ExpressPreviewCommand { get; set; }
        public DelegateCommand ExpressPrintCommand { get; set; }
        public DelegateCommand ExpressPrintConfigSaveCommand { get; set; }
        public DelegateCommand OilSamplePrintCommand { get; set; }
        public DelegateCommand OilSamplePrintConfigSaveCommand { get; set; }
        public DelegateCommand OilSampleFlowLogModifyCommand { get; set; }
        public DelegateCommand OilSampleFlowSelectionChangedCommand { get; set; }
        public DelegateCommand OilSampleFlowLogSelectionChangedCommand { get; set; }
        public DelegateCommand OilSamplePrinterNameSelectionChangedCommand { get; set; }
        public DelegateCommand ExpressPrinterNameSelectionChangedCommand { get; set; }
        public DelegateCommand OilSampleTemplateSelectCommand { get; set; }
        public DelegateCommand TemplateSelectionChangedCommand { get; set; }
        public DelegateCommand ExpressTemplateSelectCommand { get; set; }
        public DelegateCommand OilSampleEntryModifyCommand { get; set; }
        public DelegateCommand OilSampleEntryPrintLogModifyCommand { get; set; }
        public DelegateCommand OilSampleEntryCheckedCommand { get; set; }
        public DelegateCommand OilSampleEntryMergePrintCommand { get; set; }
        public DelegateCommand DealingFlowShowCommand { get; set; }
        public DelegateCommand DealedFlowShowCommand { get; set; }
        public DelegateCommand OilSampleEntryOrderPrintCommand { get; set; }
        public DelegateCommand A4PrintCommand { get; set; }


        private ObservableCollection<OilSampleFlowModel> oilSampleFlows;

        public ObservableCollection<OilSampleFlowModel> OilSampleFlows
        {
            get { return oilSampleFlows; }
            set
            {
                oilSampleFlows = value;
                this.RaisePropertyChanged(nameof(OilSampleFlows));
            }
        }

        private OilSampleFlowModel oilSampleFlowSelectedItem;

        public OilSampleFlowModel OilSampleFlowSelectedItem
        {
            get { return oilSampleFlowSelectedItem; }
            set
            {
                oilSampleFlowSelectedItem = value;
                this.RaisePropertyChanged(nameof(OilSampleFlowSelectedItem));
            }
        }

        public ObservableCollection<PageSizeModel> OilSamplePaperNames { get; set; }

        public ObservableCollection<PageSizeModel> ExpressPaperNames { get; set; }

        //public List<string> ComputerPrinters { get; set; }

        private ObservableCollection<OilSampleFlowPrintLogModel> oilSampleFlowLogs;

        public ObservableCollection<OilSampleFlowPrintLogModel> OilSampleFlowLogs
        {
            get { return oilSampleFlowLogs; }
            set
            {
                oilSampleFlowLogs = value;
                this.RaisePropertyChanged(nameof(OilSampleFlowLogs));
            }
        }

        public OilSampleFlowPrintLogModel OilSampleFlowLogSelectedItem { get; set; }

        private ObservableCollection<OilSampleEntryModel> oilSampleEntries;

        public ObservableCollection<OilSampleEntryModel> OilSampleEntries
        {
            get { return oilSampleEntries; }
            set
            {
                oilSampleEntries = value;
                this.RaisePropertyChanged(nameof(OilSampleEntries));
            }
        }

        private ObservableCollection<OilSampleEntryModel> oilSampleEntrySelectedItems;

        public ObservableCollection<OilSampleEntryModel> OilSampleEntrySelectedItems
        {
            get { return oilSampleEntrySelectedItems; }
            set
            {
                oilSampleEntrySelectedItems = value;
                this.RaisePropertyChanged(nameof(OilSampleEntrySelectedItems));
            }
        }

        private List<BarTenderTemplateModel> oilSampleTemplates;

        public List<BarTenderTemplateModel> OilSampleTemplates
        {
            get { return oilSampleTemplates; }
            set
            {
                oilSampleTemplates = value;
                this.RaisePropertyChanged(nameof(OilSampleTemplates));
            }
        }

        private List<BarTenderTemplateModel> expressTemplates;

        public List<BarTenderTemplateModel> ExpressTemplates
        {
            get { return expressTemplates; }
            set
            {
                expressTemplates = value;
                this.RaisePropertyChanged(nameof(ExpressTemplates));
            }
        }

        private BarTenderTemplateModel oilSampleTemplateSelectedItem;

        public BarTenderTemplateModel OilSampleTemplateSelectedItem
        {
            get { return oilSampleTemplateSelectedItem; }
            set
            {
                oilSampleTemplateSelectedItem = value;
                this.RaisePropertyChanged(nameof(OilSampleTemplateSelectedItem));
            }
        }

        private BarTenderPrintConfigModel oilSamplePrintConfig;

        public BarTenderPrintConfigModel OilSamplePrintConfig
        {
            get { return oilSamplePrintConfig; }
            set
            {
                oilSamplePrintConfig = value;
                this.RaisePropertyChanged(nameof(OilSamplePrintConfig));
            }
        }

        private BarTenderPrintConfigModelXX expressPrintConfig;

        public BarTenderPrintConfigModelXX ExpressPrintConfig
        {
            get { return expressPrintConfig; }
            set
            {
                expressPrintConfig = value;
                this.RaisePropertyChanged(nameof(ExpressPrintConfig));
            }
        }

        private OilSampleEntryModel oilSampleEntrySelectedItem;

        public OilSampleEntryModel OilSampleEntrySelectedItem
        {
            get { return oilSampleEntrySelectedItem; }
            set
            {
                oilSampleEntrySelectedItem = value;
                this.RaisePropertyChanged(nameof(OilSampleEntrySelectedItem));
            }
        }


        private int printTotalNum = 0;

        public int PrintTotalNum
        {
            get { return printTotalNum; }
            set
            {
                printTotalNum = value;
                this.RaisePropertyChanged(nameof(PrintTotalNum));
            }
        }

    }
}
