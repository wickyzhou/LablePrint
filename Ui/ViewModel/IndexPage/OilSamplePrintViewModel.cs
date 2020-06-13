using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class OilSamplePrintViewModel : NotificationObject
    {
        private static readonly OilSampleService _oilSampleService = new OilSampleService();
        private static readonly CommonService _commonService = new CommonService();
        private static readonly UserModel _user = MemoryCache.Default["user"] as UserModel;
        private static readonly string _hostName = Dns.GetHostName();

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
            OilSampleFlowLogDeleteCommand = new DelegateCommand(DeleteOilSampleFlowLog);
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
        }

        private void ModifyOilSampleEntry(object obj)
        {

            if (OilSampleEntrySelectedItem != null)
            {
                OilSampleEntryModifyView edit = new OilSampleEntryModifyView();
                var cloneData = TransExpV2<OilSampleEntryModel, OilSampleEntryModel>.Trans(OilSampleEntrySelectedItem);
                (edit.DataContext as OilSampleEntryModifyViewModel).WithParam(cloneData, (type, entry) =>
                {
                    edit.Close();
                    if (type == 1)
                    {
                        OilSampleEntrySelectedItem.PrintCount = entry.PrintCount;
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
                ExpressTemplates = _commonService.GetTenderPrintTemplates(fbd.SelectedPath);
            }
        }

        private void SelectOilSampleTemplate(object obj)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                OilSampleTemplates = _commonService.GetTenderPrintTemplates(fbd.SelectedPath);
            }
        }

        private void InitData()
        {

            _oilSampleService.GetOilSampleFlow().ToList().ForEach(x => OilSampleFlows.Add(x));
            _oilSampleService.GetOilSampleFlowLog().ToList().ForEach(x => OilSampleFlowLogs.Add(x));
            ComputerPrinters = _commonService.GetComputerPrinters();
            GetOilSamplePrintConfig();
            GetExpressPrintConfig();
        }

        private void GetExpressPrintConfig()
        {
            ExpressPrintConfig = _commonService.GetBarTenderPrintConfigXX(_user.ID, 3, _hostName);

            if (ExpressPrintConfig != null)
            {
                //ExpressTemplateSelectedItem = ExpressPrintConfig.ExpressTemplateSelectedItem;

                //验证打印机和模板路径是否存在

                if (!File.Exists(ExpressPrintConfig.ExpressTemplateSelectedItem.TemplateFullName))
                {
                    MessageBox.Show(" 模板路径不存在，请手动选择模板目录 \r\n");
                    return;
                }

                if (!ComputerPrinters.Contains(ExpressPrintConfig.PrinterName))
                {
                    MessageBox.Show("打印机错误或不存在，请手动选择 打印机 \r\n");
                    return;
                }

                ExpressTemplates = _commonService.GetTenderPrintTemplates(ExpressPrintConfig.ExpressTemplateSelectedItem.TemplateFolderPath);
            }
            else
            {
                //ExpressPrintConfigExpressTemplateSelectedItem = new BarTenderTemplateModel();
                ExpressPrintConfig = new BarTenderPrintConfigModelXX();
            }

        }

        private void GetOilSamplePrintConfig()
        {
            OilSampleTemplateSelectedItem = new BarTenderTemplateModel();

            OilSamplePrintConfig = _commonService.GetBarTenderPrintConfig(_user.ID, 2, _hostName);
            if (OilSamplePrintConfig != null)
            {
                OilSampleTemplateSelectedItem.TemplatePerPage = OilSamplePrintConfig.TemplatePerPage;
                OilSampleTemplateSelectedItem.TemplateFileName = OilSamplePrintConfig.TemplateFileName;
                OilSampleTemplateSelectedItem.TemplateFullName = OilSamplePrintConfig.TemplateFullName;
                OilSampleTemplateSelectedItem.TemplateFolderPath = OilSamplePrintConfig.TemplateFolderPath;
                //验证打印机和模板路径是否存在

                string r = "";
                if (!File.Exists(OilSamplePrintConfig.TemplateFullName))
                {
                    r += " 模板路径不存在，请手动选择模板目录 \r\n";
                }
                if (!ComputerPrinters.Contains(OilSamplePrintConfig.PrinterName))
                {
                    r += "打印机错误或不存在，请手动选择 打印机 \r\n";
                }

                if (string.IsNullOrEmpty(r))
                {
                    OilSampleTemplates = _commonService.GetTenderPrintTemplates(OilSamplePrintConfig.TemplateFolderPath);
                }
                else
                {
                    MessageBox.Show(r);
                }

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

        private void DeleteOilSampleFlowLog(object obj)
        {
            if (OilSampleFlowLogSelectedItem != null)
            {
                _oilSampleService.DeleteOilSampleFlowLog(OilSampleFlowLogSelectedItem.Id);
                OilSampleFlowLogs.Remove(OilSampleFlowLogSelectedItem);
            }
        }

        private void SaveOilSamplePrintConfig(object obj)
        {
            var m = _commonService.GetBarTenderPrintConfig(_user.ID, 2, _hostName);
            if (m == null)
            {
                if (OilSampleTemplateSelectedItem == null || string.IsNullOrEmpty(OilSamplePrintConfig.PrinterName))
                {
                    MessageBox.Show("请选择打印机和模板");
                    return;
                }
                OilSamplePrintConfig.UserId = _user.ID;
                OilSamplePrintConfig.TemplateTypeId = 2;
                OilSamplePrintConfig.TemplateTypeName = "松润样油明细打印";
                OilSamplePrintConfig.HostName = _hostName;
                OilSamplePrintConfig.TemplatePerPage = OilSampleTemplateSelectedItem.TemplatePerPage;
                OilSamplePrintConfig.TemplateFileName = OilSampleTemplateSelectedItem.TemplateFileName;
                OilSamplePrintConfig.TemplateFullName = OilSampleTemplateSelectedItem.TemplateFullName;
                OilSamplePrintConfig.TemplateFolderPath = OilSampleTemplateSelectedItem.TemplateFolderPath;
                var r = _commonService.InsertBarTenderPrintConfig(OilSamplePrintConfig);
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
                var r = _commonService.UpdateBarTenderPrintConfig(OilSamplePrintConfig);
                if (r)
                    MessageBox.Show("保存成功");
                else
                    MessageBox.Show("保存失败,请联系管理员");
            }

        }

        private void SaveExpressPrintConfig(object obj)
        {

            //var ss = ExpressPrintConfig;

            var m = _commonService.GetBarTenderPrintConfigXX(_user.ID, 3, _hostName);
            if (m == null)
            {
                ExpressPrintConfig.UserId = _user.ID;
                ExpressPrintConfig.TemplateTypeId = 3;
                ExpressPrintConfig.TemplateTypeName = "松润样油快递单打印";
                ExpressPrintConfig.HostName = _hostName;

                var r = _commonService.InsertBarTenderPrintConfigXX(ExpressPrintConfig);
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
                var r = _commonService.UpdateBarTenderPrintConfigXX(ExpressPrintConfig);
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
            //var batchNo = new PrintHelper().BarTenderOilSamplePrint(OilSamplePrintConfig, OilSampleEntrySelectedItem);
            var batchNo = "p020340";
            if (!string.IsNullOrEmpty(batchNo))
            {
                var log = new OilSampleFlowPrintLogModel
                {
                    Title = OilSampleFlowSelectedItem.Title,
                    FormmainId = OilSampleFlowSelectedItem.Id,
                    TypeId = OilSamplePrintConfig.TemplateTypeId,
                    TypeDesc = OilSamplePrintConfig.TemplateTypeName,
                    BatchNo = batchNo,
                    PrintCount = OilSampleEntrySelectedItem.PrintCount,
                    PrintWeight = OilSampleEntrySelectedItem.PrintCount * OilSampleEntrySelectedItem.WeightPerBucket
                };

                // 写日志

                var r = _oilSampleService.InsertOilSampleFlowLog(log);
                if (r)
                {
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



            //var model = new OilSampleFlowLogModel();
            //_oilSampleService.InsertOilSampleFlowLog(model);
        }

        private void PrintExpress(object obj)
        {
            if (OilSampleFlowSelectedItem == null)
                return;
            //var model = new OilSampleFlowLogModel();
            //_oilSampleService.InsertOilSampleFlowLog(model);
        }




        public DelegateCommand ExpressPreviewCommand { get; set; }
        public DelegateCommand ExpressPrintCommand { get; set; }
        public DelegateCommand ExpressPrintConfigSaveCommand { get; set; }
        public DelegateCommand OilSamplePrintCommand { get; set; }
        public DelegateCommand OilSamplePrintConfigSaveCommand { get; set; }
        public DelegateCommand OilSampleFlowLogDeleteCommand { get; set; }
        public DelegateCommand OilSampleFlowSelectionChangedCommand { get; set; }
        public DelegateCommand OilSampleFlowLogSelectionChangedCommand { get; set; }
        public DelegateCommand OilSamplePrinterNameSelectionChangedCommand { get; set; }
        public DelegateCommand ExpressPrinterNameSelectionChangedCommand { get; set; }
        public DelegateCommand OilSampleTemplateSelectCommand { get; set; }
        public DelegateCommand TemplateSelectionChangedCommand { get; set; }
        public DelegateCommand ExpressTemplateSelectCommand { get; set; }
        public DelegateCommand OilSampleEntryModifyCommand { get; set; }

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

        public List<string> ComputerPrinters { get; set; }

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


    }
}
