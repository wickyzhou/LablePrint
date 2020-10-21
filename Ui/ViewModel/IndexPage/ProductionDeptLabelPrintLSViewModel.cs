using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class ProductionDeptLabelPrintLSViewModel : BaseViewModel
    {

        private readonly PrintLSService _service;
        private readonly PrintService _print;

        public ProductionDeptLabelPrintLSViewModel()
        {
            _service = new PrintLSService();
            _print = new PrintService(User.ID);
            InitCommand();
            InitData();
        }

        public DelegateCommand GenerationNewDataCommand { get; set; }

        private void InitCommand()
        {
            #region 打印参数
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
                    PirntTemplates = PrintHelper.GetTenderPrintTemplates(fbd.SelectedPath);
                }
            });
            #endregion

            #region 导出

            //DirectorySelectBaseCommand = new DelegateCommand((obj) =>
            //{
            //	// 导出目录选择
            //	System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

            //	if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //	{
            //		HostConfig.HostValue = fbd.SelectedPath;
            //		var result = CommonService.SaveHostConfig(HostConfig);
            //		if (result)
            //		{
            //			HostConfig = CommonService.GetHostConfig(DataGridId, HostName, User.ID);
            //		}
            //	}
            //});

            //ExportBaseCommand = new DelegateCommand((obj) =>
            //{
            //	if (Directory.Exists(HostConfig.HostValue))
            //	{
            //		ExportView view = new ExportView(DataGridId);
            //		(view.DataContext as ExportViewModel).Export(1, (type, outputEntity, checkBoxValue, orderedColumns) =>
            //		{
            //			view.Close();
            //			if (type == 1)
            //			{
            //				DataTable datatable = new DataTable();
            //				if (outputEntity == 1)
            //				{
            //					datatable = _service.GetExportData1("DGPrintA4", User.ID, CommonService.GetSqlWhereString(QueryParameter));
            //					ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(QueryParameter));
            //					MessageBox.Show("导出成功");
            //				}
            //				//else if (outputEntity == 2)
            //				//{
            //				//	datatable = _shippingService.GetShippingBillExprotDataTable2(UserDataId);
            //				//	new Helper.DataTableImportExportHelper().ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption);
            //				//	MessageBox.Show("导出成功");
            //				//}
            //				//else if (outputEntity == 3)
            //				//{
            //				//	datatable = _shippingService.GetShippingBillExprotDataTable3(UserDataId, string.Join(",", orderedColumns));
            //				//	new Helper.DataTableImportExportHelper().ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption, checkBoxValue, 1, orderedColumns);
            //				//	MessageBox.Show("导出成功");
            //				//}
            //			}
            //		});
            //		view.ShowDialog();
            //	}
            //	else
            //	{
            //		MessageBox.Show("目录不存在，请先选择导出的目录");
            //		DirectorySelectBaseCommand.Execute(null);
            //	}
            //	CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ExportBaseCommand", ActionDesc = HostConfig.TypeDesciption + HostConfig.TypeId.ToString(), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            //});
            #endregion

            #region 保存表格
            DataGridManageBaseCommand = new DelegateCommand((obj) =>
            {
                var grid = obj as DataGrid;
                UserDataGridFormatConfigurationView view = new UserDataGridFormatConfigurationView("DGPrintLS");
                (view.DataContext as UserDataGridFormatConfigurationViewModel).WithParam(null, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        // 重新加载DataGrid格式
                        grid.Columns.Clear();
                        CommonService.GetUserDataGridColumn(User.ID, grid, 0);
                    }
                });
                view.ShowDialog();
            });

            DataGridSaveBaseCommand = new DelegateCommand((obj) =>
            {
                DataGridManagementService.SaveColumnConfigurationInUserInterface(obj as DataGrid, User.ID);
                MessageBox.Show("表格格式保存成功");
            });
            #endregion

            #region 其他
            PrintBaseCommand = new DelegateCommand((obj) =>
            {
                string msg = PrintHelper.VerifyPrintConfiguration(PrintConfiguration);
                if (string.IsNullOrEmpty(msg))
                {
                    if (BarcodeTypeSelectedItem.ItemSeq == 1)
                    {
                        MessageBox.Show("工单不能打印，请选择其他类别");
                        return;
					}
                    var selectedLists = ((obj as DataGrid).SelectedItems).Cast<LabelPrintLSModel>().ToList();
                    if (selectedLists.Count > 0)
                    {
                        string result = _print.BarTenderPrintLS(selectedLists, PrintConfiguration);
                        MessageBox.Show(result);
                        QueryBaseCommand.Execute(null);
                    }
                    else
                        MessageBox.Show("先选择行数据，【CTRL】或【SHFIT】 多选行");
                }
                else
                    MessageBox.Show(msg);
            });

            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                PrintLSLists.Clear();
                _service.GetLists(ProductionDate, BarcodeTypeSelectedItem.ItemSeq).ForEach(x => PrintLSLists.Add(x));
            });
            #endregion

            GenerationNewDataCommand = new DelegateCommand((obj) =>
            {
                if (_service.ExistsPrintData(ProductionDate))
                {
                    //提示是否清理
                    MessageBoxResult result = MessageBox.Show("当天已经有数据，重新生成会将打印记录清除", "温馨提示", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        _service.GenerationPrintData(ProductionDate);
                    }
                }
                else
                {
                    _service.GenerationPrintData(ProductionDate);
                }

            });
        }

        private void InitData()
        {

            Task.Factory.StartNew(() =>
            {
                PrintLSLists = new ObservableCollection<LabelPrintLSModel>();
                ProductionDate = DateTime.Now.Date;
                HostConfig = CommonService.GetHostConfig(DataGridId, HostName, User.ID) ?? new HostConfigModel() { TypeId = DataGridId, Host = HostName, UserId = User.ID, TypeDesciption = "蓝思标签打印" };
                ComputerPrinters = PrintHelper.GetComputerPrinters();
                var config = CommonService.GetBarTenderPrintConfigXX(User.ID, DataGridId, HostName);
                if (config == null)
                    PrintConfiguration = new BarTenderPrintConfigModelXX { TemplateTypeId = DataGridId, HostName = HostName, UserId = User.ID, TemplateTypeName = "蓝思标签打印" };
                else
                {
                    PrintConfiguration = config;
                    PirntTemplates = PrintHelper.GetTenderPrintTemplates(config.TemplateSelectedItem.TemplateFolderPath);
                }
                BarcodeTypeLists = CommonService.GetEnumLists(10);
                BarcodeTypeSelectedItem = BarcodeTypeLists.FirstOrDefault();
                UIExecute.RunAsync(() =>
                {
                    QueryBaseCommand.Execute(null);
                });
            });
        }

        private ObservableCollection<LabelPrintLSModel> printLSLists;

        public ObservableCollection<LabelPrintLSModel> PrintLSLists
        {
            get { return printLSLists; }
            set
            {
                printLSLists = value;
                this.RaisePropertyChanged(nameof(PrintLSLists));
            }
        }

        private EnumModel barcodeTypeSelectedItem;

        public EnumModel BarcodeTypeSelectedItem
        {
            get { return barcodeTypeSelectedItem; }
            set
            {
                barcodeTypeSelectedItem = value;
                this.RaisePropertyChanged(nameof(BarcodeTypeSelectedItem));
            }
        }

        private IEnumerable<EnumModel> barcodeTypeLists;

        public IEnumerable<EnumModel> BarcodeTypeLists
        {
            get { return barcodeTypeLists; }
            set
            {
                barcodeTypeLists = value;
                this.RaisePropertyChanged(nameof(BarcodeTypeLists));
            }
        }


        private DateTime productionDate;

        public DateTime ProductionDate
        {
            get { return productionDate; }
            set
            {
                productionDate = value;
                this.RaisePropertyChanged(nameof(ProductionDate));
            }
        }

    }

}
