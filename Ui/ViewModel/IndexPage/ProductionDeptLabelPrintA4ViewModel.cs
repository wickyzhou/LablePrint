using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Model;
using QueryParameterModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class ProductionDeptLabelPrintA4ViewModel : BaseViewModel
    {
        private readonly PrintA4Service _service;
        private readonly PrintService _print;

        public ProductionDeptLabelPrintA4ViewModel()
        {
            _service = new PrintA4Service();
            _print = new PrintService(User.ID);
            InitCommand();
            InitData();
        }

        public DelegateCommand DataGridSaveCommand { get; set; }
        public DelegateCommand PrintCommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand DataGridManageCommand { get; set; }
        public DelegateCommand DataGridColumnHideCommand { get; set; }
        public DelegateCommand DataGridColorChangeCommand { get; set; }
       


        private void InitCommand()
        {
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

            DirectorySelectBaseCommand = new DelegateCommand((obj) =>
            {
                // 导出目录选择
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    HostConfig.HostValue = fbd.SelectedPath;
                    var result = CommonService.SaveHostConfig(HostConfig);
                    if (result)
                    {
                        HostConfig = CommonService.GetHostConfig(DataGridId, HostName, User.ID);
                    }
                }
            });

            ExportBaseCommand = new DelegateCommand((obj) =>
            {
                if (Directory.Exists(HostConfig.HostValue))
                {
                    ExportView view = new ExportView(DataGridId,1);
                    (view.DataContext as ExportViewModel).Export((type, outputEntity, checkBoxValue, orderedColumns) =>
                    {
                        view.Close();
                        if (type == 1)
                        {
                            DataTable datatable = new DataTable();
                            if (outputEntity == 1)
                            {
                                datatable = _service.GetExportData1("DGPrintA4", User.ID, CommonService.GetSqlWhereString(QueryParameter));
                                ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(QueryParameter));
                                MessageBox.Show("导出成功");
                            }
                            //else if (outputEntity == 2)
                            //{
                            //	datatable = _shippingService.GetShippingBillExprotDataTable2(UserDataId);
                            //	new Helper.DataTableImportExportHelper().ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption);
                            //	MessageBox.Show("导出成功");
                            //}
                            //else if (outputEntity == 3)
                            //{
                            //	datatable = _shippingService.GetShippingBillExprotDataTable3(UserDataId, string.Join(",", orderedColumns));
                            //	new Helper.DataTableImportExportHelper().ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption, checkBoxValue, 1, orderedColumns);
                            //	MessageBox.Show("导出成功");
                            //}
                        }
                    });
                    view.ShowDialog();
                }
                else
                {
                    MessageBox.Show("目录不存在，请先选择导出的目录");
                    DirectorySelectBaseCommand.Execute(null);
                }
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ExportBaseCommand", ActionDesc = HostConfig.TypeDesciption + HostConfig.TypeId.ToString(), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            DataGridSaveCommand = new DelegateCommand((obj) =>
            {
                DataGridManagementService.SaveColumnConfigurationInUserInterface(obj as DataGrid, User.ID);
                MessageBox.Show("参数保存成功");
            });

            PrintBaseCommand = new DelegateCommand((obj) =>
            {
                string msg = PrintHelper.VerifyPrintConfiguration(PrintConfiguration);
                if (string.IsNullOrEmpty(msg))
                {
                    var selectedLists = ((obj as DataGrid).SelectedItems).Cast<LabelPrintHistoryModel>().ToList();
                    if (selectedLists.Count > 0)
                    {
                        //_print.PrintTest(PrintConfiguration.PrinterName, PrintConfiguration.TemplateSelectedItem.TemplateFullName,selectedLists.FirstOrDefault(),2);
                        string result = _print.BarTenderPrintA4(selectedLists, PrintConfiguration, selectedLists.Sum(x => x.PrintCount));
                        MessageBox.Show(result);
                        QueryCommand.Execute(null);
                    }
                    else
                        MessageBox.Show("先选择行数据，【CTRL】或【SHFIT】 多选行");
                }
                else
                    MessageBox.Show(msg);
            });

            QueryCommand = new DelegateCommand((obj) =>
            {
                PrintHistoryLists.Clear();
                _service.GetHistoryLists(CommonService.GetSqlWhereString(QueryParameter)).ForEach(x => PrintHistoryLists.Add(x));
            });

            DataGridManageCommand = new DelegateCommand((obj) =>
            {
                var grid = obj as DataGrid;
                UserDataGridFormatConfigurationView view = new UserDataGridFormatConfigurationView("DGPrintA4");
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

        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                QueryParameter = new ProductionDeptLabelPrintA4QueryParameterModel { ProductionDate = DateTime.Now.Date };
                PrintHistoryLists = new ObservableCollection<LabelPrintHistoryModel>();


                HostConfig = CommonService.GetHostConfig(DataGridId, HostName, User.ID) ?? new HostConfigModel() { TypeId = DataGridId, Host = HostName, UserId = User.ID, TypeDesciption = "标签A4打印" };
                ComputerPrinters = PrintHelper.GetComputerPrinters();
                var config = CommonService.GetBarTenderPrintConfigXX(User.ID, DataGridId, HostName);
                if (config == null)
                    PrintConfiguration = new BarTenderPrintConfigModelXX { TemplateTypeId = DataGridId, HostName = HostName, UserId = User.ID, TemplateTypeName = "标签A4打印" };
                else
                {
                    PrintConfiguration = config;
                    PirntTemplates = PrintHelper.GetTenderPrintA4Templates(config.TemplateSelectedItem.TemplateFolderPath);
                }


                var ss = _service.GetHistoryLists(CommonService.GetSqlWhereString(QueryParameter));

                UIExecute.RunAsync(() =>
                {
                    ss.ForEach(x => PrintHistoryLists.Add(x));
                });
            });
        }

        private ProductionDeptLabelPrintA4QueryParameterModel queryParameter;

        public ProductionDeptLabelPrintA4QueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }

        private ObservableCollection<LabelPrintHistoryModel> printHistoryLists;

        public ObservableCollection<LabelPrintHistoryModel> PrintHistoryLists
        {
            get { return printHistoryLists; }
            set
            {
                printHistoryLists = value;
                this.RaisePropertyChanged(nameof(PrintHistoryLists));
            }
        }

        private List<DataGridColumnHeaderUserCustomModel> columnUserCustomLists;

        public List<DataGridColumnHeaderUserCustomModel> ColumnUserCustomLists
        {
            get { return columnUserCustomLists; }
            set
            {
                columnUserCustomLists = value;
                this.RaisePropertyChanged(nameof(ColumnUserCustomLists));
            }
        }

        private bool isColumnVisibility = true;

        public bool IsColumnVisibility
        {
            get { return isColumnVisibility; }
            set
            {
                isColumnVisibility = value;
                this.RaisePropertyChanged(nameof(IsColumnVisibility));
            }
        }

    }



}
