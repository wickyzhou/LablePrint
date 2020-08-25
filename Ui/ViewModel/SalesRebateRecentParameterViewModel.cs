using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class SalesRebateRecentParameterViewModel:NewDialogViewModel<SalesRebateRecentParameterViewModel>
    {
        private readonly SalesRebateService _salesRebateService;
        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService;
        private readonly int _userDataId;
        private readonly SalesRebateModel _pageParameter;
        public SalesRebateRecentParameterViewModel(SalesRebateModel pageParameter)
        {
            _pageParameter = pageParameter;
            _salesRebateService = new SalesRebateService();
            _salesRebateAmountRangeService = new SalesRebateAmountRangeService();
            _userDataId = CommonService.GetUserDataId(User, 10);
            InitData();
            InitCommand();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                QueryParameter = new SalesRebateModel() { OrgName = _pageParameter.OrgName, OrgId = _pageParameter.OrgId,RebateClass = _pageParameter.RebateClass, RebateClassName = _pageParameter.RebateClassName };
                QueryParameter1 = new SalesRebateModel();
                SalesRebateLists = new ObservableCollection<SalesRebateModel>();
                SalesRebateLists1 = new ObservableCollection<SalesRebateModel>();
                AmountRangeLists = new ObservableCollection<SalesRebateAmountRangeModel>();
                AmountRangeLists1 = new ObservableCollection<SalesRebateAmountRangeModel>();

                UIExecute.RunAsync(() =>
                {   
                    _salesRebateService.GetSalesRebateOrgRecentParameterLists(_pageParameter).ForEach(x => SalesRebateLists.Add(x));
                });
            });
        }

        private void InitCommand()
        {
            QueryCommand = new DelegateCommand((obj) =>
            {
                string filter = $" and OrgId = {QueryParameter.OrgId} and RebateClass ={QueryParameter.RebateClass} and OrgCode like '%{QueryParameter.OrgCode}%' and CaseName like '%{QueryParameter.CaseName}%' ";
                SalesRebateLists.Clear();
                _salesRebateService.GetSalesRebateOrgRecentParameterLists(filter).ForEach(x => SalesRebateLists.Add(x));
            });

            QueryCommand1 = new DelegateCommand((obj) =>
            {
                string filter = $" and OrgCode like '%{QueryParameter1.OrgCode}%' and CaseName like '%{QueryParameter1.CaseName}%' and OrgName like '%{QueryParameter1.OrgName}%' ";
                SalesRebateLists1.Clear();
                _salesRebateService.GetSalesRebateHistoryParameterLists(IsHistory, filter).ForEach(x => SalesRebateLists1.Add(x));
            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSelectedItem != null)
                {
                    AmountRangeLists.Clear();
                    if (SalesRebateSelectedItem.RebatePctType==2)
                    _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(SalesRebateSelectedItem.Guid).ForEach(x => AmountRangeLists.Add(x));
                }
            });

            SelectionChangedCommand1 = new DelegateCommand((obj) =>
            {
                if (SalesRebateSelectedItem1 != null)
                {
                    AmountRangeLists1.Clear();
                    if (SalesRebateSelectedItem1.RebatePctType == 2)
                        _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(SalesRebateSelectedItem1.Guid).ForEach(x => AmountRangeLists1.Add(x));
                }
            });

            ModifyCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSelectedItem == null)
                    return;

                var cloneData = ObjectDeepCopyHelper<SalesRebateModel, SalesRebateModel>.Trans(SalesRebateSelectedItem);
                SalesRebateCreateAndCopyView view = new SalesRebateCreateAndCopyView();
                (view.DataContext as SalesRebateCreateAndCopyViewModel).WithParam(cloneData, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        outputEntity.UserId = User.ID;
                        if (_salesRebateService.RecentParameterUpdate(outputEntity))
                        {
                            if (outputEntity.RebatePctType == 1)
                                _salesRebateAmountRangeService.RecentSonParameterDelete(outputEntity.Guid);

                            //界面重新加载省事
                            SalesRebateLists.Clear();
                            _salesRebateService.GetSalesRebateOrgRecentParameterLists(_pageParameter).ForEach(x => SalesRebateLists.Add(x));

                            //SalesRebateSelectedItem.TaxAmountType = outputEntity.TaxAmountType;
                            //SalesRebateSelectedItem.TaxAmountTypeName = outputEntity.TaxAmountTypeName;
                            //SalesRebateSelectedItem.RebatePctValue = outputEntity.RebatePctValue;
                            //SalesRebateSelectedItem.RebatePctType = outputEntity.RebatePctType;
                            //SalesRebateSelectedItem.RebatePctTypeName = outputEntity.RebatePctTypeName;
                            //SalesRebateSelectedItem.MinusLastPeriodRebateType = outputEntity.MinusLastPeriodRebateType;
                            //SalesRebateSelectedItem.MinusLastPeriodRebateTypeName = outputEntity.MinusLastPeriodRebateTypeName;

                            //AmountRangeLists.Clear();
                            //if (SalesRebateSelectedItem.RebatePctType == 2)
                            //    _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(SalesRebateSelectedItem.Guid).ForEach(x => AmountRangeLists.Add(x));

                        }
                    }
                });
                view.ShowDialog();
            });

            RemoveCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateSelectedItem1 != null)
                {
                    if (_salesRebateService.RecentMainParameterDelete(SalesRebateSelectedItem1.Id, SalesRebateSelectedItem1.Guid))
                       // QueryCommand1.Execute(null);
                        SalesRebateLists1.Remove(SalesRebateSelectedItem1);
                }
            });

            CopyCommand = new DelegateCommand((obj) =>
            {
                _salesRebateService.SalesRebateParameterCopy(SalesRebateSelectedItem.Id, SalesRebateSelectedItem1.Id, SalesRebateSelectedItem.Guid);
                //界面重新加载省事
                SalesRebateLists.Clear();
                _salesRebateService.GetSalesRebateOrgRecentParameterLists(_pageParameter).ForEach(x => SalesRebateLists.Add(x));
            });
        }

        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand QueryCommand1 { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }
        public DelegateCommand SelectionChangedCommand1 { get; set; }

        public DelegateCommand ModifyCommand { get; set; }
        public DelegateCommand RemoveCommand { get; set; }
        public DelegateCommand CopyCommand { get; set; }

        private ObservableCollection<SalesRebateAmountRangeModel> amountRangeLists;

        public ObservableCollection<SalesRebateAmountRangeModel> AmountRangeLists
        {
            get { return amountRangeLists; }
            set
            {
                amountRangeLists = value;
                this.RaisePropertyChanged(nameof(AmountRangeLists));
            }
        }

        private ObservableCollection<SalesRebateAmountRangeModel> amountRangeLists1;

        public ObservableCollection<SalesRebateAmountRangeModel> AmountRangeLists1
        {
            get { return amountRangeLists1; }
            set
            {
                amountRangeLists1 = value;
                this.RaisePropertyChanged(nameof(AmountRangeLists1));
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

        private SalesRebateModel salesRebateSelectedItem1;

        public SalesRebateModel SalesRebateSelectedItem1
        {
            get { return salesRebateSelectedItem1; }
            set
            {
                salesRebateSelectedItem1 = value;
                this.RaisePropertyChanged(nameof(SalesRebateSelectedItem1));
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

        private ObservableCollection<SalesRebateModel> salesRebateLists1;

        public ObservableCollection<SalesRebateModel> SalesRebateLists1
        {
            get { return salesRebateLists1; }
            set
            {
                salesRebateLists1 = value;
                this.RaisePropertyChanged(nameof(SalesRebateLists1));
            }
        }

        private SalesRebateModel queryParameter;

        public SalesRebateModel QueryParameter
        {
            get { return queryParameter; }
            set
            {
                queryParameter = value;
                this.RaisePropertyChanged(nameof(QueryParameter));
            }
        }

        private SalesRebateModel queryParameter1;

        public SalesRebateModel QueryParameter1
        {
            get { return queryParameter1; }
            set
            {
                queryParameter1 = value;
                this.RaisePropertyChanged(nameof(QueryParameter1));
            }
        }

        private bool isHistory = false;

        public bool IsHistory
        {
            get { return isHistory; }
            set
            {
                isHistory = value;
                this.RaisePropertyChanged(nameof(IsHistory));
            }
        }

    }
}
