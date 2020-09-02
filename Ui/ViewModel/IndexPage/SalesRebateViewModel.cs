using Common;
using K3ApiModel;
using K3ApiModel.Request;
using K3ApiModel.SalesInvoiceVAT;
using Model;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel.IndexPage
{
    public class SalesRebateViewModel : K3ApiBaseViewModel, IValidationExceptionHandler
    {

        private readonly SalesRebateService _salesRebateService;
        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService;
        private readonly int _userDataId;

        public SalesRebateViewModel() : base("Sales_Invoice_VAT")
        {
            _salesRebateService = new SalesRebateService();
            _salesRebateAmountRangeService = new SalesRebateAmountRangeService();
            _userDataId = CommonService.GetUserDataId(User, 10);
            InitCommand();
            InitData();
        }

        private void InitData()
        {

            SalesRebateLists = new ObservableCollection<SalesRebateModel>();
            QueryParameter = new SalesRebateQueryParameterModel() { SettleDateBegin = DateTime.Now.AddYears(-1).Date.ToString("yyyy-MM-dd"), SettleDateEnd = DateTime.Now.Date.ToString("yyyy-MM-dd") };
            RebateClassSeletedItem = new EnumModel();
            BatchParameter = new SalesRebateBatchParameterModel()
            {
                SalesRebateBatchParameter = new SalesRebateModel() { SettleDateBegin = DateTime.Now.AddMonths(-1).Date, SettleDateEnd = DateTime.Now.Date, Guid = Guid.NewGuid(), OrgCode = "", UserId = User.ID, RebatePctValue = 0, RebatePctType = 1 },
                SalesRebateAmountRangeBatchParameter = new ObservableCollection<SalesRebateAmountRangeModel>()
            };


            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(() =>
                {
                    RebateClassLists = CommonService.GetEnumLists(6);
                    TaxAmountTypeLists = CommonService.GetEnumLists(7);
                    RebatePctTypeLists = CommonService.GetEnumLists(8);
                    OrganizationLists = ComboBoxSearchService.GetOrganizationLists();
                    MinusLastPeriodRebateLists = CommonService.GetEnumLists(999);
                    _salesRebateService.GetSalesRebateLists(_userDataId, IsHistory).ForEach(x => SalesRebateLists.Add(x));
                });
            });
        }

        private void InitCommand()
        {

            //SalesRebateAmountCalculateCommand = new DelegateCommand((obj) =>
            //{
            //    var lists = SalesRebateLists.Where(x => x.IsChecked);
            //    if (lists.Count() == 0)
            //    {

            //    }

            //    if (SalesRebateLists.Where(x => x.IsChecked & !string.IsNullOrEmpty(x.K3BillNo)).Count() > 0)   // 如果已有K3单据号，则不可以重算
            //    {
            //        MessageBox.Show("已生成K3发票的数据不能重算");
            //    }
            //    _salesRebateService.ReCalculateSalesRebateAmount(DateTime.Now.Date, DateTime.Now.Date);
            //    GetNewSalesRebateLists();
            //});

            SalesRebateQueryCommand = new DelegateCommand((obj) =>
            {
                string filter = $"  and SettleDateEnd >= '{QueryParameter.SettleDateBegin}' and SettleDateEnd <= '{QueryParameter.SettleDateEnd}' and OrgCode like '%{QueryParameter.OrgCode}%' and CaseName like '%{QueryParameter.CaseName}%' and OrgName like '%{QueryParameter.OrgName}%' ";
                GetNewSalesRebateLists(filter);
            });

            //SalesRebateModifyCommand = new DelegateCommand((obj) =>
            //{
            //    if (SalesRebateLists.Where(x => x.IsChecked).Count() != 1)
            //    {
            //        MessageBox.Show("只能勾选一条记录修改");
            //        return;
            //    }
            //    SalesRebateSelectedItem = SalesRebateLists.Where(x => x.IsChecked).FirstOrDefault();
            //    var cloneData = ObjectDeepCopyHelper<SalesRebateModel, SalesRebateModel>.Trans(SalesRebateSelectedItem);
            //    SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();
            //    (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(cloneData, (type, outputEntity) =>
            //    {
            //        view.Close();
            //        if (type == 1)
            //        {
            //            if (_salesRebateService.Update(outputEntity))
            //            {
            //                SalesRebateSelectedItem.TaxAmountType = outputEntity.TaxAmountType;
            //                SalesRebateSelectedItem.TaxAmountTypeName = outputEntity.TaxAmountTypeName;
            //                SalesRebateSelectedItem.RebatePctValue = outputEntity.RebatePctValue;
            //                SalesRebateSelectedItem.RebatePctType = outputEntity.RebatePctType;
            //                SalesRebateSelectedItem.RebatePctTypeName = outputEntity.RebatePctTypeName;
            //                SalesRebateSelectedItem.MinusLastPeriodRebateType = outputEntity.MinusLastPeriodRebateType;
            //                SalesRebateSelectedItem.MinusLastPeriodRebateTypeName = outputEntity.MinusLastPeriodRebateTypeName;
            //            }

            //        }
            //    });
            //    view.ShowDialog();
            //});

            //SalesRebateCreateCommand = new DelegateCommand((obj) =>
            //{
            //    SalesRebateModel inputEntity = new SalesRebateModel() { Guid = Guid.NewGuid(), OrgId = -1, CaseId = -1, MaterialId = -1 };

            //    SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

            //    (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
            //    {
            //        view.Close();
            //        if (type == 1)
            //        {
            //            if (_salesRebateService.Insert(outputEntity))
            //                SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
            //        }
            //    });
            //    view.ShowDialog();
            //});

            //SalesRebateCopyCommand = new DelegateCommand((obj) =>
            //{
            //    SalesRebateModel inputEntity = _salesRebateService.GetSalesRebate(SalesRebateSelectedItem.Guid);
            //    Guid newGuid = Guid.NewGuid();
            //    // 如果比例类型是分段比例的话，先将分段数据插入到后台表(新增、复制、修改可以共用一个视图)
            //    if (inputEntity.RebatePctType == 2)
            //        _salesRebateService.Copy(inputEntity.Guid, newGuid);
            //    inputEntity.Guid = newGuid;

            //    SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

            //    (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
            //    {
            //        view.Close();
            //        if (type == 1)
            //        {
            //            if (_salesRebateService.Insert(outputEntity))
            //                SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
            //        }
            //    });
            //    view.ShowDialog();
            //});

            SalesRebateRemoveCommand = new DelegateCommand((obj) =>
            {

                string guids = "'" + string.Join("','", SalesRebateLists.Where(m => m.IsChecked).Select(x => x.Guid)) + "'";
                if (_salesRebateService.BatchDelete(guids))
                    GetNewSalesRebateLists();
                else
                    MessageBox.Show("删除失败,系统异常，请联系管理员");
            });

            SalesRebateDiskRemoveCommand = new DelegateCommand((obj) =>
            {
                MessageBoxResult result = MessageBox.Show("此操作不可恢复，确认删除？", "温馨提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    string guids = "'" + string.Join("','", SalesRebateLists.Where(m => m.IsChecked).Select(x => x.Guid)) + "'";
                    if (_salesRebateService.DiskBatchDelete(guids))
                        GetNewSalesRebateLists();
                    else
                        MessageBox.Show("删除失败,系统异常，请联系管理员");
                }
            });

            AllCheckedCommand = new DelegateCommand((obj) =>
            {
                //IsCheckedAll = !IsCheckedAll;
                if (IsCheckedAll)
                {
                    foreach (var item in SalesRebateLists)
                        item.IsChecked = true;
                }
                else
                {
                    foreach (var item in SalesRebateLists)
                        item.IsChecked = false;
                }

            });

            MouseLeftClickCommand = new DelegateCommand((obj) =>
            {

                SalesRebateModel dr = (obj as DataGridRow).Item as SalesRebateModel;
                dr.IsChecked = !dr.IsChecked;
            });

            SalesRebateParameterCopyCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateLists.Where(m => m.IsChecked).Count() == 1)
                {
                    var dr = SalesRebateLists.Where(m => m.IsChecked).FirstOrDefault();
                    // 将选项数据赋值到批处理
                    ModelTypeHelper.PropertyMapper(BatchParameter.SalesRebateBatchParameter, dr);
                    OrganizationSearchedItem = new ComboBoxSearchModel() { Id = dr.OrgId, Name = dr.OrgName, SearchText = dr.OrgName };
                    RebateClassSeletedItem = new EnumModel() { ItemSeq = dr.RebateClass, ItemValue = dr.RebateClassName };
                    BatchParameter.SalesRebateAmountRangeBatchParameter.Clear();
                    if (dr.RebatePctType == 2)
                        BatchParameter.SalesRebateAmountRangeBatchParameter = new ObservableCollection<SalesRebateAmountRangeModel>(_salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(dr.Guid));
                }
                else
                {
                    MessageBox.Show("只允许选中一行数据");
                }
            });

            SalesRebateParameterClearCommand = new DelegateCommand((obj) =>
            {
                BatchParameter.SalesRebateBatchParameter.MinusLastPeriodRebateType = 0;
                BatchParameter.SalesRebateBatchParameter.TaxAmountType = 0;
                BatchParameter.SalesRebateBatchParameter.RebatePctType = 0;
                BatchParameter.SalesRebateBatchParameter.RebatePctValue = null;
                BatchParameter.SalesRebateAmountRangeBatchParameter.Clear();
            });


            SalesRebateHistoryShowCommand = new DelegateCommand((obj) =>
            {
                string filter = $"  and SettleDateEnd >= '{QueryParameter.SettleDateBegin}' and SettleDateEnd <='{QueryParameter.SettleDateEnd}' and OrgCode like '%{QueryParameter.OrgCode}%' and CaseName like '%{QueryParameter.CaseName}%' and OrgName like '%{QueryParameter.OrgName}%' ";
                GetNewSalesRebateLists(filter);
            });

            RebateAmountBiggerThanZeroCheckCommand = new DelegateCommand((obj) =>
            {

            });

            SalesRebateTypedBatchInsertCommand = new DelegateCommand((obj) =>
             {
                //验证界面数据是否都填写，填写无误则将参数导入到数据库模板，后台批插入
                //if (InputVerification())
                //{
                MessageBox.Show($"{_salesRebateService.BatchGenerationSalesRebateEntry(BatchParameter.SalesRebateBatchParameter)}");
                 GetNewSalesRebateLists();
                //}
                //else
                //    MessageBox.Show("界面参数必须全部正确填写");

            });

            SalesRebateAmountRangeCreateCommand = new DelegateCommand((obj) =>
            {
                double lastMaxValue = BatchParameter.SalesRebateAmountRangeBatchParameter.Count == 0 ? 0 : BatchParameter.SalesRebateAmountRangeBatchParameter.Max(x => x.AmountUpper);
                SalesRebateAmountRangeCreateView view = new SalesRebateAmountRangeCreateView(lastMaxValue);
                SalesRebateAmountRangeModel inputEntity = new SalesRebateAmountRangeModel() { Guid = BatchParameter.SalesRebateBatchParameter.Guid };

                (view.DataContext as SalesRebateAmountRangeCreateViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                        BatchParameter.SalesRebateAmountRangeBatchParameter.Add(outputEntity);
                });
                view.ShowDialog();
            });

            SalesRebateAmountRangeRemoveCommand = new DelegateCommand((obj) =>
            {
                BatchParameter.SalesRebateAmountRangeBatchParameter.Remove(SalesRebateAmountRangeSelectedItem);
            });

            SalesRebateRecentParameterShowCommand = new DelegateCommand((obj) =>
            {

                if (OrganizationSearchedItem == null || RebateClassSeletedItem == null || OrganizationSearchedItem.Id == 0 || RebateClassSeletedItem.ItemSeq == 0)
                {
                    MessageBox.Show("请先输入客户和返利类别");
                    return;
                }
                else
                {

                    //后台插入该客户类型的本期所需添加的数据
                    _salesRebateService.LoadBatchParamterToDBTemplate(BatchParameter.SalesRebateBatchParameter, User.ID);
                    if (BatchParameter.SalesRebateBatchParameter.RebatePctType == 2)
                        _salesRebateService.LoadAmountRangeListsToDBTemplate(BatchParameter.SalesRebateAmountRangeBatchParameter);

                    _salesRebateService.InsertCurrentOrgRebateClassParameter(BatchParameter.SalesRebateBatchParameter);
                    BatchParameter.SalesRebateBatchParameter.OrgName = OrganizationSearchedItem.SearchText;
                    BatchParameter.SalesRebateBatchParameter.RebateClassName = RebateClassSeletedItem.ItemValue;
                    SalesRebateRecentParameterView view = new SalesRebateRecentParameterView(BatchParameter.SalesRebateBatchParameter);
                    view.Show();
                }

            });

            RebatePctTypeSelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (BatchParameter.SalesRebateBatchParameter.RebatePctType == 2)
                    BatchParameter.SalesRebateBatchParameter.RebatePctValue = null;
            });

            SalesRebateK3ApiInsertCommand = new DelegateCommand((obj) =>
            {
                var keys = SalesRebateLists.Where(m => m.IsChecked).GroupBy(x => new { x.SettleDateBegin, x.SettleDateEnd, x.OrgId, x.RebateClass, x.OrgTotalAmount }).Select(m => m.Key);

                foreach (var item in keys)
                {
                    SalesInvoiceVATMainModel main = new SalesInvoiceVATMainModel
                    {
                        FCustID = K3ApiFKService.GetOrganizationById(item.OrgId),
                        FBillerID = new BaseNumberNameModelX { FNumber = User.UserName, FName = User.UserName },
                        FNote = "结算日期：" + item.SettleDateBegin.Date.ToString("yyyy-MM-dd") + "至" + item.SettleDateEnd.Date.ToString("yyyy-MM-dd")
                    };

                    var son = new SalesInvoiceVATSonModel
                    {
                        Fauxprice = item.OrgTotalAmount,
                        Famount = item.OrgTotalAmount * main.FROB,
                        FStdAmount = item.OrgTotalAmount * main.FROB,
                        FTaxRate = item.OrgTotalAmount * 0.13,
                        FTaxAmount = item.OrgTotalAmount * 0.13 * main.FROB,
                        FStdTaxAmount = item.OrgTotalAmount * 0.13 * main.FROB,
                        FAllAmount = item.OrgTotalAmount * (1 + 0.13),
                        FAuxTaxPrice = item.OrgTotalAmount * (1 + 0.13),
                        FAuxPriceDiscount = item.OrgTotalAmount * (1 + 0.13),
                        FAmountincludetax = item.OrgTotalAmount * (1 + 0.13) * main.FROB,
                        FStdAmountincludetax = item.OrgTotalAmount * (1 + 0.13) * main.FROB,
                        FRemainAmount = item.OrgTotalAmount * (1 + 0.13) * main.FROB,
                        FRemainAmountFor = item.OrgTotalAmount * (1 + 0.13) * main.FROB,
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
                    K3ApiInsertResponseModel response = K3ApiService.Insert(postJson);

                    if (response.StatusCode == 200)
                        // 更新后台数据
                        _salesRebateService.UpdateK3BillNo(response.Data.BillNo, item.SettleDateBegin, item.SettleDateEnd, item.OrgId, item.RebateClass);
                    else
                    {
                        MessageBox.Show(response.Message);
                        return;
                    }
                }
                // 重新加载页面
                GetNewSalesRebateLists();
            });
        }

        private bool InputVerification()
        {
            if (!IsValid)
                return false;
            else if (BatchParameter.SalesRebateBatchParameter.RebateClass == 0)
                return false;
            else if (BatchParameter.SalesRebateBatchParameter.OrgId == -1)
                return false;
            else if (BatchParameter.SalesRebateBatchParameter.RebatePctType == 1 && (batchParameter.SalesRebateBatchParameter.RebatePctValue.Value <= 0 || batchParameter.SalesRebateBatchParameter.RebatePctValue.Value > 80))
                return false;
            else if (BatchParameter.SalesRebateBatchParameter.RebatePctType == 2 && BatchParameter.SalesRebateAmountRangeBatchParameter.Count == 0)
                return false;
            else
                return true;
        }

        public DelegateCommand SalesRebateAmountCalculateCommand { get; set; }
        public DelegateCommand SalesRebateQueryCommand { get; set; }
        public DelegateCommand SalesRebateModifyCommand { get; set; }
        public DelegateCommand SalesRebateCreateCommand { get; set; }
        public DelegateCommand SalesRebateCopyCommand { get; set; }
        public DelegateCommand SalesRebateRemoveCommand { get; set; }
        public DelegateCommand AllCheckedCommand { get; set; }
        public DelegateCommand MouseLeftClickCommand { get; set; }
        public DelegateCommand SalesRebateHistoryShowCommand { get; set; }
        public DelegateCommand SalesRebateTypedBatchInsertCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeCreateCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeRemoveCommand { get; set; }
        public DelegateCommand SalesRebateDiskRemoveCommand { get; set; }
        public DelegateCommand SalesRebateRecentParameterShowCommand { get; set; }
        public DelegateCommand RebatePctTypeSelectionChangedCommand { get; set; }
        public DelegateCommand SalesRebateK3ApiInsertCommand { get; set; }
        public DelegateCommand SalesRebateParameterCopyCommand { get; set; }
        public DelegateCommand SalesRebateParameterClearCommand { get; set; }
        public DelegateCommand RebateAmountBiggerThanZeroCheckCommand { get; set; }




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


        private ObservableCollection<SalesRebateModel> salesRebateLists;

        public ObservableCollection<SalesRebateModel> SalesRebateLists
        {
            get { return salesRebateLists; }
            set
            {
                salesRebateLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateLists));
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

        private ComboBoxSearchModel organizationSearchedItem;

        public ComboBoxSearchModel OrganizationSearchedItem
        {
            get { return organizationSearchedItem; }
            set
            {
                organizationSearchedItem = value;
                this.RaisePropertyChanged(nameof(OrganizationSearchedItem));
            }
        }

        private EnumModel rebateClassSearchedItem;

        public EnumModel RebateClassSeletedItem
        {
            get { return rebateClassSearchedItem; }
            set
            {
                rebateClassSearchedItem = value;
                this.RaisePropertyChanged(nameof(RebateClassSeletedItem));
            }
        }


        // 分段选择项
        private SalesRebateAmountRangeModel salesRebateAmountRangeSelectedItem;

        public SalesRebateAmountRangeModel SalesRebateAmountRangeSelectedItem
        {
            get { return salesRebateAmountRangeSelectedItem; }
            set
            {
                salesRebateAmountRangeSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateAmountRangeSelectedItem));
            }
        }


        //是否减掉上期折扣
        private IList<EnumModel> minusLastPeriodRebateLists;

        public IList<EnumModel> MinusLastPeriodRebateLists
        {
            get { return minusLastPeriodRebateLists; }
            set
            {
                minusLastPeriodRebateLists = value;
                this.RaisePropertyChanged(nameof(MinusLastPeriodRebateLists));
            }
        }

        // 返利类型
        private IList<EnumModel> rebateClassLists;

        public IList<EnumModel> RebateClassLists
        {
            get { return rebateClassLists; }
            set
            {
                rebateClassLists = value;
                this.RaisePropertyChanged(nameof(RebateClassLists));
            }
        }


        // 含税类型，计算字段
        private IList<EnumModel> taxAmountTypeLists;

        public IList<EnumModel> TaxAmountTypeLists
        {
            get { return taxAmountTypeLists; }
            set
            {
                taxAmountTypeLists = value;
                this.RaisePropertyChanged(nameof(TaxAmountTypeLists));
            }
        }

        // 比例类型
        private IList<EnumModel> rebatePctTypeLists;

        public IList<EnumModel> RebatePctTypeLists
        {
            get { return rebatePctTypeLists; }
            set
            {
                rebatePctTypeLists = value;
                this.RaisePropertyChanged(nameof(RebatePctTypeLists));
            }
        }

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



        private void GetNewSalesRebateLists(string filter = "")
        {
            IsCheckedAll = false;
            SalesRebateLists.Clear();
            _salesRebateService.GetSalesRebateLists(_userDataId, IsHistory, filter).ForEach(x => SalesRebateLists.Add(x));

        }
    }
}
