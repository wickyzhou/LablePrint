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
using System.Windows.Documents;
using System.Windows.Input;
using Ui.Command;
using Ui.Helper;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class SalesRebateCreateAndCopyViewModel : NewDialogViewModel<SalesRebateModel>
    {

        private readonly SalesRebateAmountRangeService _salesRebateAmountRangeService;

        public SalesRebateCreateAndCopyViewModel()
        {
            _salesRebateAmountRangeService = new SalesRebateAmountRangeService();

            //CaseSearchedItem = ComboBoxSearchService.GetCaseSearchItem(model.CaseId);
            //MaterialSearchedItem = ComboBoxSearchService.GetMaterialSearchItem(model.MaterialId);
            //OrganizationSearchedItem = ComboBoxSearchService.GetOrganizationSearchItem(model.OrgId)

            InitCommand();
            InitData();
        }

        private void InitData()
        {
            SalesRebateAmountRangeLists = new ObservableCollection<SalesRebateAmountRangeModel>();
            Task.Factory.StartNew(() =>
            {
                UIExecute.RunAsync(() =>
                {
                    RebateClassLists = CommonService.GetEnumLists(6);
                    TaxAmountTypeLists = CommonService.GetEnumLists(7);
                    RebatePctTypeLists = CommonService.GetEnumLists(8);
                    OrganizationLists = ComboBoxSearchService.GetOrganizationLists();
                    CaseLists = ComboBoxSearchService.GetCaseLists();
                    _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(Entity.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                    CaseSearchedItem = new ComboBoxSearchModel() { Id = Entity.CaseId, SearchText = Entity.CaseName };
                    MaterialSearchedItem = new ComboBoxSearchModel() { Id = Entity.MaterialId, SearchText = Entity.MaterialName };
                    OrganizationSearchedItem = new ComboBoxSearchModel() { Id = Entity.OrgId, SearchText = Entity.OrgName };
                });
            });
        }

        private void InitCommand()
        {
            SalesRebateAmountRangeCreateCommand = new DelegateCommand((obj) =>
            {
                SalesRebateAmountRangeCreateView view = new SalesRebateAmountRangeCreateView();
                SalesRebateAmountRangeModel inputEntity = new SalesRebateAmountRangeModel() { Guid = Entity.Guid, IsValid = 1, EffectiveDate = DateTime.Now.AddMonths(-1).Date, ExpirationDate = DateTime.Now.Date };

                (view.DataContext as SalesRebateAmountRangeCreateViewModel).WithParam(inputEntity, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateAmountRangeService.Insert(outputEntity))
                        {
                            SalesRebateAmountRangeLists.Clear();
                            _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(Entity.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                        }
                        //SalesRebateAmountRangeLists.Add(outputEntity);
                        IsHistory = false;
                    }
                });
                view.ShowDialog();
            });

            SalesRebateAmountRangeModifyCommand = new DelegateCommand((obj) =>
            {
                if (SalesRebateAmountRangeSelectedItem == null)
                    return;

                SalesRebateAmountRangeModifyView view = new SalesRebateAmountRangeModifyView();
                var cloneData = ObjectDeepCopyHelper<SalesRebateAmountRangeModel, SalesRebateAmountRangeModel>.Trans(SalesRebateAmountRangeSelectedItem);
                (view.DataContext as SalesRebateAmountRangeModifyViewModel).WithParam(cloneData, (type, outputEntity) =>
                {
                    view.Close();
                    if (type == 1)
                    {
                        if (_salesRebateAmountRangeService.Update(outputEntity))
                        {
                            SalesRebateAmountRangeLists.Clear();
                            _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(Entity.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                            IsHistory = false;
                        }
                    }
                });
                view.ShowDialog();
            });

            SalesRebateAmountRangeHistoryShowCommand = new DelegateCommand((obj) =>
            {
                bool? isChecked = (obj as CheckBox).IsChecked;
                SalesRebateAmountRangeLists.Clear();
                if (isChecked.Value)
                    _salesRebateAmountRangeService.GetSalesRebateAmountRangeHistoryLists(Entity.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
                else
                    _salesRebateAmountRangeService.GetSalesRebateAmountRangeLists(Entity.Guid).ForEach(x => SalesRebateAmountRangeLists.Add(x));
            });
        }

        public DelegateCommand SalesRebateAmountRangeCreateCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeModifyCommand { get; set; }
        public DelegateCommand OrgNameSearchCommand { get; set; }
        public DelegateCommand EnterCommand { get; set; }
        public DelegateCommand MaterialDatabaseSearchCommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeHistoryShowCommand { get; set; }



        public override void Save(object obj)
        {

            if (Entity.RebateClass == 0 || Entity.RebatePctType == 0 || Entity.TaxAmountType == 0)
            {
                MessageBox.Show("下拉框必须选择");
                return;
            }
            else if (Entity.RebateClass == 5 && Entity.MaterialId == -1)
            {
                MessageBox.Show("产品型号 必须键盘【Enter】或者鼠标 选择");
                return;
            }
            else if (Entity.RebateClass == 3 && Entity.CaseId == -1)
            {
                MessageBox.Show("案子 必须键盘【Enter】或者鼠标 选择");
                return;
            }
            else if (Entity.OrgId == -1)
            {
                MessageBox.Show("客户 必须键盘【Enter】或者鼠标 选择");
                return;
            }
            else if (Entity.RebatePctType == 1 && (Entity.RebatePctValue <= 0 || Entity.RebatePctValue > 80))
            {
                MessageBox.Show("固定返利类型，必须填写正确的数值");
                return;
            }
            else if (Entity.RebatePctType == 2 && SalesRebateAmountRangeLists.Where(m => m.IsValid == 1).Count() == 0)
            {
                MessageBox.Show("分段返利类型，必须添加明细");
                return;
            }
            base.Save(obj);
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

        // 物料条目
        private ComboBoxSearchModel materialSearchedItem;

        public ComboBoxSearchModel MaterialSearchedItem
        {
            get { return materialSearchedItem; }
            set
            {
                materialSearchedItem = value;
                this.RaisePropertyChanged(nameof(MaterialSearchedItem));
            }
        }

        // 分段数据源
        private ObservableCollection<SalesRebateAmountRangeModel> salesRebateAmountRangeLists;

        public ObservableCollection<SalesRebateAmountRangeModel> SalesRebateAmountRangeLists
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

        // 是否查看历史记录
        private bool  isHistory = false;

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
