using Common;
using Model;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class SalesRebateRecentParameterModifyViewModel : NewDialogViewModel<SalesRebateRecentParameterMainModel>
    {

        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService;

        public SalesRebateRecentParameterModifyViewModel()
        {
            _salesRebateAmountRangeService = new SalesRebateAmountRangeService();
            InitCommand();
            InitData();
        }

        private void InitData()
        {

            Task.Factory.StartNew(() =>
            {
                SalesRebateAmountRangeLists = new ObservableCollection<SalesRebateRecentParameterSonModel>();
                var enums = CommonService.GetEnumLists();
                RebateClassLists = enums.Where(x => x.GroupSeq == 6);
                TaxAmountTypeLists = enums.Where(x => x.GroupSeq == 7);
                RebatePctTypeLists = enums.Where(x => x.GroupSeq == 8);
                OrganizationLists = ComboBoxSearchService.GetOrganizationLists();
                CaseLists = ComboBoxSearchService.GetCaseLists();
                MinusLastPeriodRebateLists = enums.Where(x => x.GroupSeq == 999);
                AmountRangeCalculateTypeLists = enums.Where(x => x.GroupSeq == 11);
                UIExecute.RunAsync(() =>
                {
                    if (Entity.RebatePctType == 2)
                        _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(Entity.PGuid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                });
            });
        }

        private void InitCommand()
        {
            SalesRebateAmountRangeCreateCommand = new DelegateCommand((obj) =>
            {
                double lastMaxValue = SalesRebateAmountRangeLists.Count == 0 ? 0 : SalesRebateAmountRangeLists.Max(x => x.AmountUpper);
                SalesRebateAmountRangeCreateView view = new SalesRebateAmountRangeCreateView(lastMaxValue);
                SalesRebateRecentParameterSonModel inputEntity = new SalesRebateRecentParameterSonModel() { Guid = Entity.PGuid };

                (view.DataContext as SalesRebateAmountRangeCreateViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        var guid = _salesRebateAmountRangeService.RecentSonParameterInsert(outputEntity, inputEntity.Guid);
                        Entity.PGuid = guid;
                        SalesRebateAmountRangeLists.Clear();
                        _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(Entity.PGuid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                    }
                });
                view.ShowDialog();
            });

            SalesRebateAmountRangeRemoveCommand = new DelegateCommand((obj) =>
            {
                //删除操作不能直接把分段表数据删除，因为计算结果表SJSalesRebate也引用了这个参数表，所以只能先复制一份然后再做增删改
                //var newGuid = _salesRebateAmountRangeService.RecentSonParameterClear();
                Entity.PGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                SalesRebateAmountRangeLists.Clear();
            });

            RebatePctTypeSelectionChangedCommand = new DelegateCommand((obj) =>
           {
               if (Entity.RebatePctType == 2)
               {
                   Entity.RebatePctValue = null;
                   SalesRebateAmountRangeLists.Clear();
                   _salesRebateAmountRangeService.GetSalesRebateAmountRangeRecentParameterLists(Entity.PGuid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
               }
               else
               {
                   Entity.AmountRangeCalculateType = -1;
                   Entity.PGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                   SalesRebateAmountRangeLists.Clear();

               }

           });
        }

        public DelegateCommand SalesRebateAmountRangeCreateCommand { get; set; }
        public DelegateCommand OrgNameSearchCommand { get; set; }
        public DelegateCommand EnterCommand { get; set; }
        public DelegateCommand MaterialDatabaseSearchCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeHistoryShowCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeRemoveCommand { get; set; }
        public DelegateCommand RebatePctTypeSelectionChangedCommand { get; set; }



        public override void Save(object obj)
        {

            if (Entity.RebatePctType == 0 || Entity.TaxAmountType == 0 || Entity.MinusLastPeriodRebateType == 0)
            {
                MessageBox.Show("下拉框必须选择");
                return;
            }
            else if (Entity.RebatePctType == 1 && (Entity.RebatePctValue == null || Entity.RebatePctValue.Value <= 0 || Entity.RebatePctValue.Value > 80))
            {
                MessageBox.Show("固定返利类型，必须填写正确的数值");
                return;
            }
            else if (Entity.RebatePctType == 2 && (SalesRebateAmountRangeLists.Count() == 0 || Entity.AmountRangeCalculateType < 1))
            {
                MessageBox.Show("分段返利类型，必须添加明细和指定分段金额类型");
                return;
            }
            base.Save(obj);
        }


        // 返利类型
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


        // 含税类型，计算字段
        private IEnumerable<EnumModel> taxAmountTypeLists;

        public IEnumerable<EnumModel> TaxAmountTypeLists
        {
            get { return taxAmountTypeLists; }
            set
            {
                taxAmountTypeLists = value;
                this.RaisePropertyChanged(nameof(TaxAmountTypeLists));
            }
        }

        // 比例类型
        private IEnumerable<EnumModel> rebatePctTypeLists;

        public IEnumerable<EnumModel> RebatePctTypeLists
        {
            get { return rebatePctTypeLists; }
            set
            {
                rebatePctTypeLists = value;
                this.RaisePropertyChanged(nameof(RebatePctTypeLists));
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

        // 客户条目
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


        // 案子列表
        private IEnumerable<ComboBoxSearchModel> caseLists;

        public IEnumerable<ComboBoxSearchModel> CaseLists
        {
            get { return caseLists; }
            set
            {
                caseLists = value;
                this.RaisePropertyChanged(nameof(CaseLists));
            }
        }

        // 案子条目
        private ComboBoxSearchModel caseSearchedItem;

        public ComboBoxSearchModel CaseSearchedItem
        {
            get { return caseSearchedItem; }
            set
            {
                caseSearchedItem = value;
                this.RaisePropertyChanged(nameof(CaseSearchedItem));
            }
        }

        //// 物料条目
        //private ComboBoxSearchModel materialSearchedItem;

        //public ComboBoxSearchModel MaterialSearchedItem
        //{
        //    get { return materialSearchedItem; }
        //    set
        //    {
        //        materialSearchedItem = value;
        //        this.RaisePropertyChanged(nameof(MaterialSearchedItem));
        //    }
        //}

        // 分段数据源
        private ObservableCollection<SalesRebateRecentParameterSonModel> salesRebateAmountRangeLists;

        public ObservableCollection<SalesRebateRecentParameterSonModel> SalesRebateAmountRangeLists
        {
            get { return salesRebateAmountRangeLists; }
            set
            {
                salesRebateAmountRangeLists = value;
                this.RaisePropertyChanged(nameof(SalesRebateAmountRangeLists));
            }
        }

        // 分段选择项

        private SalesRebateRecentParameterSonModel salesRebateAmountRangeSelectedItem;

        public SalesRebateRecentParameterSonModel SalesRebateAmountRangeSelectedItem
        {
            get { return salesRebateAmountRangeSelectedItem; }
            set
            {
                salesRebateAmountRangeSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateAmountRangeSelectedItem));
            }
        }

        // 是否查看历史记录
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

        //是否减掉上期折扣
        private IEnumerable<EnumModel> minusLastPeriodRebateLists;

        public IEnumerable<EnumModel> MinusLastPeriodRebateLists
        {
            get { return minusLastPeriodRebateLists; }
            set
            {
                minusLastPeriodRebateLists = value;
                this.RaisePropertyChanged(nameof(MinusLastPeriodRebateLists));
            }
        }


        //逐层累积还是总额返利
        private IEnumerable<EnumModel> amountRangeCalculateTypeLists;

        public IEnumerable<EnumModel> AmountRangeCalculateTypeLists
        {
            get { return amountRangeCalculateTypeLists; }
            set
            {
                amountRangeCalculateTypeLists = value;
                this.RaisePropertyChanged(nameof(AmountRangeCalculateTypeLists));
            }
        }



    }
}
