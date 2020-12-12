
using K3ApiModel;
using K3ApiModel.Request;
using K3ApiModel.SalesInvoiceVAT;
using Model;

using QueryParameterModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
    public class SalesRebateViewModel : K3ApiXBaseViewModel, IValidationExceptionHandler
    {

        private readonly SalesRebateService _salesRebateService = new SalesRebateService();
        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService = new SalesRebateAmountRangeService();

        public SalesRebateViewModel()
        {
            InitCommand();
            InitData();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                QueryParameter = new SalesRebateQueryParameterModel() { SettleDateEnd1 = DateTime.Now.AddYears(-1).Date, SettleDateEnd2 = DateTime.Now.Date };
                BatchParameter = new SalesRebateBatchParameterModel()
                {
                    SettleDateBegin = DateTime.Now.AddMonths(-1).Date,
                    SettleDateEnd = DateTime.Now.Date
                };

                var enums = CommonService.GetEnumLists();

                RebateClassLists = enums.Where(x => x.GroupSeq == 6);
                OrganizationLists = ComboBoxSearchService.GetOrganizationLists();

                HostConfig = CommonService.GetHostConfig(DataGridId, HostName, User.ID) ?? new HostConfigModel() { TypeId = DataGridId, Host = HostName, UserId = User.ID, TypeDesciption = "客户案子销售返利" };
                Token = K3ApiXService.GetToken();

                UIExecute.RunAsync(() =>
                {
                    QueryBaseCommand.Execute(null);
                });
            });
        }

        private void InitCommand()
        {

            QueryBaseCommand = new DelegateCommand((obj) =>
            {
                SalesRebateSummaryLists.Clear();
                ListsCount = 0;
                ListsSum = 0;
                SalesRebateLists.Clear();
                RebatePctValueString = string.Empty;
                _salesRebateService.GetSalesRebateSummaryLists(UserDataId, CommonService.GetSqlWhereString(QueryParameter)).ForEach(x => { SalesRebateSummaryLists.Add(x); ListsSum += x.OrgAmount; ListsCount++; });
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
                    ExportView view = new ExportView(DataGridId, 1);
                    (view.DataContext as ExportViewModel).Export((type, outputEntity, checkBoxValue, orderedColumns) =>
                    {
                        view.Close();
                        if (type == 1)
                        {
                            DataTable datatable = new DataTable();

                            if (outputEntity == 1)
                            {    // 获取导出数据名称
                                string viewName = CommonService.GetExportViewName(DataGridId, 1);
                                if (!string.IsNullOrEmpty(viewName))
                                {
                                    datatable = viewName.EndsWith("Proc")? _salesRebateService.GetExportDataProc(viewName, ReportQueryParameter): CommonService.GetExportDataView(viewName, CommonService.GetSqlWhereString(ReportQueryParameter));
                                    ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(ReportQueryParameter));
                                    MessageBox.Show("导出成功");
                                }
                            }
                            else if ( outputEntity == 2)
                            {    // 获取导出数据名称
                                string viewName = CommonService.GetExportViewName(DataGridId, 2);
                                if (!string.IsNullOrEmpty(viewName))
                                {
                                    datatable = viewName.EndsWith("Proc") ? _salesRebateService.GetExportDataProc(viewName, ReportQueryParameter) : CommonService.GetExportDataView(viewName, CommonService.GetSqlWhereString(ReportQueryParameter));
                                    ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(ReportQueryParameter));
                                    MessageBox.Show("导出成功");
                                }
                            }
                            else if (outputEntity == 3)
                            {
                                // 获取导出数据名称
                                string procName = CommonService.GetExportViewName(DataGridId, 3);
                                if (!string.IsNullOrEmpty(procName))
                                {
                                    datatable = CommonService.GetExportDataProcedure(procName, UserDataId, string.Join(",", orderedColumns), CommonService.GetSqlWhereString(ReportQueryParameter));
                                    ExportHelper.ExportDataTableToExcel(datatable, HostConfig.HostValue, HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(ReportQueryParameter), checkBoxValue, orderedColumns, false, "", false);
                                    MessageBox.Show("导出成功");
                                }
                            }
                        }
                    });
                    view.ShowDialog();
                }
                else
                {
                    MessageBox.Show("目录不存在，请先选择导出的目录");
                    DirectorySelectBaseCommand.Execute(null);
                }
                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "ExportBaseCommand", ActionDesc = HostConfig.TypeDesciption + CommonService.GetQueryParameterValueString(QueryParameter), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            SalesRebateRecentParameterConfigCommand = new DelegateCommand((obj) =>
            {
                if (BatchParameter.OrganizationSearchedItem != null && BatchParameter.RebateClassSeletedItem != null)
                {
                    if (BatchParameterVerification( out DateTime maxDate ))
                    {
                        if (_salesRebateService.IfExistsOrgICSaleBill(BatchParameter))
                        {
                            SalesRebateRecentParameterView view = new SalesRebateRecentParameterView(BatchParameter);
                            (view.DataContext as SalesRebateRecentParameterViewModel).WithParam(null, (type, outputEntity, isChanged) =>
                            {
                                view.Close();
                                if (isChanged)
                                {
                                    // 计算刚才配置的参数
                                    _salesRebateService.CalculateAmount(BatchParameter, User.ID,true);

                                    // 查询出刚生成的数据
                                    QueryBaseCommand.Execute(null);
                                }
                            });
                            view.Show();
                            CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "SalesRebateRecentParameterConfigCommand", ActionDesc = CommonService.GetSqlWhereString(BatchParameter), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
                        }
                        else
                            MessageBox.Show($"【{BatchParameter.SettleDateBegin}】至【{BatchParameter.SettleDateEnd}】 ,客户【{BatchParameter.OrganizationSearchedItem.Name}】 没有K3销售单据");
                    }
                    else
                        MessageBox.Show($"【{maxDate}】 该类型【{BatchParameter.RebateClassSeletedItem.ItemValue} 】已经返利，同时间点不能多次返利");
                }
                else
                    MessageBox.Show("请先输入客户和返利类别");

            });

            SalesRebateK3ApiInsertCommand = new DelegateCommand((obj) =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (SalesRebateSummarySelectedItem != null && string.IsNullOrEmpty(SalesRebateSummarySelectedItem.K3BillNo) && SalesRebateLists.Count > 0)
                {
                    var maxDate = _salesRebateService.GetMaxSettleDateByOrgId(BatchParameter.OrganizationSearchedItem.Id, BatchParameter.RebateClassSeletedItem.ItemSeq);
                    if (SalesRebateSummarySelectedItem.SettleDateBegin > maxDate && SalesRebateSummarySelectedItem.SettleDateEnd > maxDate)
                    {
                        SalesInvoiceVATMainModel main = new SalesInvoiceVATMainModel
                        {
                            Fdate = SalesRebateSummarySelectedItem.SettleDateEnd.AddDays(5).ToString("yyyy-MM-dd"),
                            FCustID = K3ApiFKService.GetOrganizationById(SalesRebateSummarySelectedItem.OrgId),
                            FBillerID = new BaseNumberNameModelX { FNumber = User.UserName, FName = User.UserName },
                            FNote = "结算日期：" + SalesRebateSummarySelectedItem.SettleDateBegin.Date.ToString("yyyy-MM-dd") + "至" + SalesRebateSummarySelectedItem.SettleDateEnd.Date.ToString("yyyy-MM-dd")
                        };

                        var son = new SalesInvoiceVATSonModel
                        {
                            Fauxprice = SalesRebateSummarySelectedItem.OrgAmount,
                            Famount = SalesRebateSummarySelectedItem.OrgAmount * main.FROB,
                            FStdAmount = SalesRebateSummarySelectedItem.OrgAmount * main.FROB,
                            FTaxRate = SalesRebateSummarySelectedItem.OrgAmount * 0.13,
                            FTaxAmount = SalesRebateSummarySelectedItem.OrgAmount * 0.13 * main.FROB,
                            FStdTaxAmount = SalesRebateSummarySelectedItem.OrgAmount * 0.13 * main.FROB,
                            FAllAmount = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13),
                            FAuxTaxPrice = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13),
                            FAuxPriceDiscount = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13),
                            FAmountincludetax = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13) * main.FROB,
                            FStdAmountincludetax = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13) * main.FROB,
                            FRemainAmount = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13) * main.FROB,
                            FRemainAmountFor = SalesRebateSummarySelectedItem.OrgAmount * (1 + 0.13) * main.FROB,
                        };

                        var requestModel = new K3ApiInsertRequestModel<SalesInvoiceVATMainModel, SalesInvoiceVATSonModel>()
                        {
                            Data = new K3ApiInsertDataRequestModel<SalesInvoiceVATMainModel, SalesInvoiceVATSonModel>()
                            {
                                Page1 = new List<SalesInvoiceVATMainModel> { main },
                                Page2 = new List<SalesInvoiceVATSonModel> { son }
                            }
                        };

                        string postJson = JsonHelper.ObjectToJson(requestModel);
                        K3ApiInsertResponseModel response = K3ApiXService.Insert(Token, "Sales_Invoice_VAT", postJson);

                        if (response.StatusCode == 200)
                        {
                            // 更新后台数据
                            _salesRebateService.UpdateK3BillNo(SalesRebateSummarySelectedItem.Id, response.Data.BillNo, SalesRebateSummarySelectedItem.SettleDateEnd.AddDays(5));
                            stringBuilder.Append($"成功 BillNo：{response.Data.BillNo}, SettleDateBegin：{SalesRebateSummarySelectedItem.SettleDateBegin}, SettleDateEnd：{SalesRebateSummarySelectedItem.SettleDateEnd},OrgId： {SalesRebateSummarySelectedItem.OrgId}, RebateClass：{SalesRebateSummarySelectedItem.RebateClass}");
                        }
                        else
                        {
                            MessageBox.Show(response.Message);
                            stringBuilder.Append($"{response.Message}");
                            CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "SalesRebateRecentParameterConfigCommand", ActionDesc = stringBuilder.ToString(), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
                            return;
                        }
                    }
                    else
                        MessageBox.Show($"【{maxDate}】 该类型【{BatchParameter.RebateClassSeletedItem.ItemValue} 】已经返利，同时间点不能多次返利");

                }
                //重新加载数据
                QueryBaseCommand.Execute(null);

                CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "SalesRebateK3ApiInsertCommand", ActionDesc = stringBuilder.ToString(), UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
            });

            SalesRebateK3ApiRemoveCommand = new DelegateCommand((obj) =>
            {
                // 弹出删除提示
                if (MessageBox.Show($"此操作会将K3单据和本条记录同时删除", "温馨提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (SalesRebateSummarySelectedItem != null && !string.IsNullOrEmpty(SalesRebateSummarySelectedItem.K3BillNo))
                    {
                        string k3BillNo = SalesRebateSummarySelectedItem.K3BillNo;

                        // 可选参数FInterID，当FBillNo重复时传入,【只删除明细行】
                        string postJson = @"{""Data"": {""FBillNo"":""" + k3BillNo + @"""}}";

                        var response = K3ApiXService.Delete(Token, "Sales_Invoice_VAT", postJson);
                        // 如果Token过期则重新加载
                        if (response.StatusCode == 200)
                        {
                            if (_salesRebateService.SummaryDelete(SalesRebateSummarySelectedItem.Id, SalesRebateSummarySelectedItem.Guid))
                                MessageBox.Show("删除K3单据成功");
                            else
                                MessageBox.Show("后台删除失败，单据不存在");
                        }
                        else if (response.Message.IndexOf("token", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            Token = K3ApiXService.GetToken();
                            response = K3ApiXService.Delete(Token, "Sales_Invoice_VAT", postJson);
                            MessageBox.Show(response.Message);
                        }
                        else
                            MessageBox.Show(response.Message);
                        CommonService.WriteActionLog(new ActionOperationLogModel { ActionName = "SalesRebateK3ApiRemoveCommand", ActionDesc = k3BillNo, UserId = User.ID, MainMenuId = Menu.ID, PKId = -1, HostName = HostName });
                        QueryBaseCommand.Execute(null);
                    }
                    else
                        MessageBox.Show("一次只能选择一个K3单据且已经生成过K3单据");
                }


            });

            DetailSelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSelectedItem != null)
                {
                    var lists = _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(SalesRebateSelectedItem.PGuid);
                    if (lists.Count>0)
                    {
                        StringBuilder result = new StringBuilder();
                        foreach (var item in lists)
                        {
                            result.Append($"金额区间：{item.AmountLower}-{item.AmountUpper} 万元  比例：{item.SalesRebatePctValue}% \t\t");
                        }
                        RebatePctValueString = result.ToString();
                    }
                    else
                        RebatePctValueString = string.Empty;
                }
       
            });

            SummarySelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSummarySelectedItem != null)
                {
                    RebatePctValueString = string.Empty;
                    SalesRebateLists.Clear();
                    _salesRebateService.GetSalesRebateListsByGuid(SalesRebateSummarySelectedItem.Guid).ForEach(x => SalesRebateLists.Add(x));
                }
            });

            SalesRebateSummaryDeleteCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSummarySelectedItem != null && string.IsNullOrEmpty(SalesRebateSummarySelectedItem.K3BillNo))
                {
                    _salesRebateService.SummaryDelete(SalesRebateSummarySelectedItem.Id, SalesRebateSummarySelectedItem.Guid);
                    QueryBaseCommand.Execute(null);
                }
            });

            ReportQueryCommand = new DelegateCommand((obj) =>
            {
                ReportLists.Clear();
                _salesRebateService.GetCaseAmountReport(ReportQueryParameter).ForEach(x=> ReportLists.Add(x));
            });

            AmountCalculateCommand = new DelegateCommand((obj) =>
            {
                if (BatchParameterVerification(out DateTime maxDate))
                {
                    // 计算刚才配置的参数
                    _salesRebateService.CalculateAmount(BatchParameter, User.ID,false);

                    // 查询出刚生成的数据
                    QueryBaseCommand.Execute(null);
                }
                else
                    MessageBox.Show($"【{maxDate}】 该类型【{BatchParameter.RebateClassSeletedItem.ItemValue} 】已经返利，同时间点不能多次返利");

            });
        }


        public DelegateCommand SalesRebateRecentParameterConfigCommand { get; set; }
        public DelegateCommand SalesRebateRemoveCommand { get; set; }
        public DelegateCommand AllCheckedCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand { get; set; }
        public DelegateCommand SalesRebateK3ApiInsertCommand { get; set; }
        public DelegateCommand SalesRebateK3ApiRemoveCommand { get; set; }
        public DelegateCommand SalesRebateParameterShowCommand { get; set; }
        public DelegateCommand ReportQueryCommand { get; set; }
        public DelegateCommand SummarySelectionChangedCommand { get; set; }
        public DelegateCommand SalesRebateSummaryDeleteCommand { get; set; }
        public DelegateCommand DetailSelectionChangedCommand { get; set; }
        public DelegateCommand AmountCalculateCommand { get; set; }

        private bool BatchParameterVerification(out DateTime dt)
        {
            var maxDate = _salesRebateService.GetMaxSettleDateByOrgId(BatchParameter.OrganizationSearchedItem.Id, BatchParameter.RebateClassSeletedItem.ItemSeq);
            dt = maxDate;
            if (BatchParameter.SettleDateBegin > maxDate && BatchParameter.SettleDateEnd > maxDate)
                return true;
            return false;
        }

        private SalesRebateQueryParameterModel queryParameter;

        public SalesRebateQueryParameterModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }


        private ObservableCollection<SalesRebateSummaryModel> salesRebateSummaryLists = new ObservableCollection<SalesRebateSummaryModel>();

        public ObservableCollection<SalesRebateSummaryModel> SalesRebateSummaryLists
        {
            get { return salesRebateSummaryLists; }
            set
            {
                salesRebateSummaryLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateSummaryLists));
            }
        }

        private ObservableCollection<SalesRebateModel> salesRebateLists = new ObservableCollection<SalesRebateModel>();

        public ObservableCollection<SalesRebateModel> SalesRebateLists
        {
            get { return salesRebateLists; }
            set
            {
                salesRebateLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateLists));
            }
        }

        private ObservableCollection<SalesRebateOrgCaseReportModel> reportLists = new ObservableCollection<SalesRebateOrgCaseReportModel>();

        public ObservableCollection<SalesRebateOrgCaseReportModel> ReportLists
        {
            get { return reportLists; }
            set
            {
                reportLists = value;
                this.RaisePropertyChanged(nameof(ReportLists));
            }
        }



        private SalesRebateSummaryModel salesRebateSummarySelectedItem;

        public SalesRebateSummaryModel SalesRebateSummarySelectedItem
        {
            get { return salesRebateSummarySelectedItem; }
            set
            {
                salesRebateSummarySelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateSummarySelectedItem));
            }
        }

        private SalesRebateModel salesRebateSelectedItem;

        public SalesRebateModel SalesRebateSelectedItem
        {
            get { return salesRebateSelectedItem; }
            set
            {
                salesRebateSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateSelectedItem));
            }
        }


        private string k3BillNos;

        public string K3BillNos
        {
            get { return k3BillNos; }
            set
            {
                k3BillNos = value;
                this.RaisePropertyChanged(nameof(K3BillNos));
            }
        }

        private bool isHistory;

        public bool IsHistory
        {
            get { return isHistory; }
            set
            {
                isHistory = value;
                this.RaisePropertyChanged(nameof(IsHistory));
            }
        }

        private bool isCheckedAll = false;

        public bool IsCheckedAll
        {
            get { return isCheckedAll; }
            set
            {
                isCheckedAll = value;
                this.RaisePropertyChanged(nameof(IsCheckedAll));
            }
        }


        // 客户列表
        private IEnumerable<ComboBoxSearchModel> organizationLists;

        public IEnumerable<ComboBoxSearchModel> OrganizationLists
        {
            get { return organizationLists; }
            set
            {
                organizationLists = value;
                this.RaisePropertyChanged(nameof(OrganizationLists));
            }
        }

        //批量生成数据参数
        private SalesRebateBatchParameterModel batchParameter;

        public SalesRebateBatchParameterModel BatchParameter
        {
            get { return batchParameter; }
            set
            {
                batchParameter = value;
                this.RaisePropertyChanged(nameof(BatchParameter));
            }
        }


        //页面验证
        private bool isValid = true;

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                isValid = value;
                this.RaisePropertyChanged(nameof(IsValid));
            }
        }

        //查询行数
        private int listsCount;

        public int ListsCount
        {
            get { return listsCount; }
            set
            {
                listsCount = value;
                this.RaisePropertyChanged(nameof(ListsCount));
            }
        }

        //查询总数
        private double listsSum;

        public double ListsSum
        {
            get { return listsSum; }
            set
            {
                listsSum = value;
                this.RaisePropertyChanged(nameof(ListsSum));
            }
        }


        private string rebatePctValueString;

        public string RebatePctValueString
        {
            get { return rebatePctValueString; }
            set
            {
                rebatePctValueString = value;
                this.RaisePropertyChanged(nameof(RebatePctValueString));
            }
        }

        private IEnumerable<EnumModel> rebateClassLists;

        public IEnumerable<EnumModel> RebateClassLists
        {
            get { return rebateClassLists; }
            set
            {
                rebateClassLists = value;
                this.RaisePropertyChanged(nameof(RebateClassLists));
            }
        }

        private SalesRebateReportQueryParameterModel reportQueryParameter = new SalesRebateReportQueryParameterModel { BillDate1= DateTime.Now.AddMonths(-3).Date, BillDate2 = DateTime.Now.Date };

        public SalesRebateReportQueryParameterModel ReportQueryParameter
        {
            get { return reportQueryParameter; }
            set
            {
                reportQueryParameter = value;
                this.RaisePropertyChanged(nameof(ReportQueryParameter));
            }
        }



    }
}
