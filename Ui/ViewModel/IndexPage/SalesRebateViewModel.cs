using Common;
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
    public class SalesRebateViewModel : BaseViewModel
    {

        private readonly SalesRebateService _salesRebateService;
        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService;
        private readonly int _userDataId;


        public SalesRebateViewModel()
        {
            _salesRebateService = new SalesRebateService();
            _salesRebateAmountRangeService = new SalesRebateAmountRangeService();
            _userDataId = CommonService.GetUserDataId(User, 10);
            InitCommand();
            InitData();
        }

        private void InitData()
        {
            Filter = new GeneralParameterModel() { ParamBeginDate = DateTime.Now.Date, ParamEndDate = DateTime.Now.Date };
            SalesRebateLists = new ObservableCollection<SalesRebateModel>();
            QueryParameter = new SalesRebateQueryParameterModel();
            SalesRebateSelectedItem = new SalesRebateModel();
            BatchParamter = new SalesRebateModel() { SettleDateBegin = DateTime.Now.AddMonths(-1).Date, SettleDateEnd = DateTime.Now.Date, Guid = Guid.NewGuid() };
            SalesRebateAmountRangeLists = new ObservableCollection<SalesRebateAmountRangeModel>();
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

            SalesRebateAmountCalculateCommand = new DelegateCommand((obj) =>
            {
                _salesRebateService.CalculateSalesRebateAmount(Filter.ParamBeginDate, Filter.ParamEndDate);
                GetNewSalesRebateLists();
            });

            SalesRebateQueryCommand = new DelegateCommand((obj) =>
            {
                string filter = $" where MaterialName like '%{QueryParameter.ProductionModelName}%' and CaseName like '%{QueryParameter.CaseName}%' and OrgName like '%{QueryParameter.OrgName}%' ";
                GetNewSalesRebateLists(filter);
            });

            SalesRebateModifyCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateLists.Where(x => x.IsChecked).Count() != 1)
                {
                    MessageBox.Show("只能勾选一条记录修改");
                    return;
                }

                var cloneData = ObjectDeepCopyHelper<SalesRebateModel, SalesRebateModel>.Trans(SalesRebateLists.Where(x => x.IsChecked).FirstOrDefault());
                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();
                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(cloneData, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Update(outputEntity))
                            GetNewSalesRebateLists();
                    }
                });
                view.ShowDialog();
            });

            SalesRebateCreateCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel inputEntity = new SalesRebateModel() { Guid = Guid.NewGuid(), OrgId = -1, CaseId = -1, MaterialId = -1 };

                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Insert(outputEntity))
                            SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
                    }
                });
                view.ShowDialog();
            });

            SalesRebateCopyCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel inputEntity = _salesRebateService.GetSalesRebate(SalesRebateSelectedItem.Guid);
                Guid newGuid = Guid.NewGuid();
                // 如果比例类型是分段比例的话，先将分段数据插入到后台表(新增、复制、修改可以共用一个视图)
                if (inputEntity.RebatePctType == 2)
                    _salesRebateService.Copy(inputEntity.Guid, newGuid);
                inputEntity.Guid = newGuid;

                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();

                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateService.Insert(outputEntity))
                            SalesRebateLists.Insert(0, _salesRebateService.GetSalesRebate(outputEntity.Guid));
                    }
                });
                view.ShowDialog();
            });

            SalesRebateRemoveCommand = new DelegateCommand((obj) =>
            {
                string guids = "'"+string.Join("','", SalesRebateLists.Where(m => m.IsChecked).Select(x=>x.Guid))+"'";
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
                if (IsCheckedAll)
                {
                    foreach (var item in SalesRebateLists)
                        item.IsChecked = false;
                }
                else
                {
                    foreach (var item in SalesRebateLists)
                        item.IsChecked = true;
                }
                IsCheckedAll = !IsCheckedAll;
            });

            MouseLeftClickCommand = new DelegateCommand((obj) =>
            {
                SalesRebateModel dr = (obj as DataGridRow).Item as SalesRebateModel;
                dr.IsChecked = !dr.IsChecked;
                // 将选项数据赋值到批处理
                ModelTypeHelper.PropertyMapper(BatchParamter,dr);
                OrganizationSearchedItem = new ComboBoxSearchModel() { Id = dr.OrgId, Name = dr.OrgName, SearchText = dr.OrgName };

                if (dr.RebatePctType == 2)
                {
                    SalesRebateAmountRangeLists.Clear();
                    _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(dr.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                }
            });

            SalesRebateHistoryShowCommand = new DelegateCommand((obj) =>
            {
                GetNewSalesRebateLists();
                IsCheckedAll = false;
            });

            SalesRebateTypedBatchInsertCommand = new DelegateCommand((obj) =>
            {
                //验证界面数据是否都填写，填写无误则将参数导入到数据库模板，后台批插入
                if (InputVerification())
                {
                    var guid = Guid.NewGuid();
                    _salesRebateService.LoadBatchParamterToDBTemplate(BatchParamter, guid);
                    if (BatchParamter.RebatePctType == 2)
                        _salesRebateService.LoadAmountRangeListsToDBTemplate(SalesRebateAmountRangeLists, guid);
                    _salesRebateService.BatchGenerationSalesRebateEntry(User.ID);
                    GetNewSalesRebateLists();
                }
                else
                    MessageBox.Show("界面参数必须全部正确填写");

            });

            SalesRebateAmountRangeCreateCommand = new DelegateCommand((obj) =>
            {
                SalesRebateAmountRangeCreateView view = new SalesRebateAmountRangeCreateView();
                SalesRebateAmountRangeModel inputEntity = new SalesRebateAmountRangeModel() { IsValid = 1, EffectiveDate = DateTime.Now.AddMonths(-1).Date, ExpirationDate = DateTime.Now.Date, Guid = BatchParamter.Guid };

                (view.DataContext as SalesRebateAmountRangeCreateViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                        SalesRebateAmountRangeLists.Add(outputEntity);
                });
                view.ShowDialog();
            });

            SalesRebateAmountRangeRemoveCommand = new DelegateCommand((obj) =>
            {
                SalesRebateAmountRangeLists.Remove(SalesRebateAmountRangeSelectedItem);
            });
        }

        private bool InputVerification()
        {
            if (batchParamter.RebateClass == 0 || batchParamter.RebatePctType == 0 || batchParamter.TaxAmountType == 0 || batchParamter.MinusLastPeriodRebateType == 0)
                return false;
            else if (batchParamter.OrgId == -1)
                return false;
            else if (batchParamter.RebatePctType == 1 && (batchParamter.RebatePctValue <= 0 || batchParamter.RebatePctValue > 80))
                return false;
            else if (batchParamter.RebatePctType == 2 && SalesRebateAmountRangeLists.Where(m => m.IsValid == 1).Count() == 0)
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






        private GeneralParameterModel filter;

        public GeneralParameterModel Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                this.RaisePropertyChanged(nameof(Filter));
            }
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

        private bool isCheckedAll;

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
        private SalesRebateModel batchParamter;

        public SalesRebateModel BatchParamter
        {
            get { return batchParamter; }
            set
            {
                batchParamter = value;
                this.RaisePropertyChanged(nameof(BatchParamter));
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


        // 分段数据源
        private IList<SalesRebateAmountRangeModel> salesRebateAmountRangeLists;

        public IList<SalesRebateAmountRangeModel> SalesRebateAmountRangeLists
        {
            get { return salesRebateAmountRangeLists; }
            set
            {
                salesRebateAmountRangeLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateAmountRangeLists));
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


        private void GetNewSalesRebateLists(string filter="")
        {
            SalesRebateLists.Clear();
            _salesRebateService.GetSalesRebateLists(_userDataId, IsHistory,filter).ForEach(x => SalesRebateLists.Add(x));
        }
    }
}
