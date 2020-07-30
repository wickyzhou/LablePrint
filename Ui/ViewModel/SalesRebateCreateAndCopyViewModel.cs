using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Ui.Command;
using Ui.Service;
using Ui.View.InfoWindow;

namespace Ui.ViewModel
{
    public class SalesRebateCreateAndCopyViewModel : NewDialogViewModel<SalesRebateModel>
    {

        private readonly OrganizationService _organizationService;
        private readonly K3CustomService _k3CustomService;

        public SalesRebateCreateAndCopyViewModel()
        {
            _organizationService = new OrganizationService();
            _k3CustomService = new K3CustomService();
            InitCommand();
            InitData();
        }

        private void InitData()
        {
            Task.Factory.StartNew(() =>
            {
                RebateClassLists = CommonService.GetEnumLists(6);
                isTaxAmountLists = CommonService.GetEnumLists(7);
                RebatePctClassLists = CommonService.GetEnumLists(8);

                CaseLists = new List<ComboBoxSearchModel>()
                {
                    new ComboBoxSearchModel() { Id=1 ,SearchText="xxxxa11" ,Name="a11" },
                    new ComboBoxSearchModel() { Id = 2,SearchText="xxxxaa12" , Name = "a12" },
                    new ComboBoxSearchModel() { Id = 3,SearchText="xxxxb12" , Name = "b12" },
                    new ComboBoxSearchModel() { Id = 4,SearchText="xxxxa24" , Name = "a24" },
                    new ComboBoxSearchModel() { Id = 14,SearchText="xxxxa34" , Name = "a34" },
                    new ComboBoxSearchModel() { Id = 24,SearchText="xxxxa44" , Name = "a44" },
                    new ComboBoxSearchModel() { Id = 34,SearchText="xxxxa53" , Name = "a53" },
                    new ComboBoxSearchModel() { Id = 44,SearchText="xxxxa63" , Name = "a63" },
                    new ComboBoxSearchModel() { Id = 54,SearchText="xxxxaa74" , Name = "a74" },
                    new ComboBoxSearchModel() { Id = 64,SearchText="xxxxaa84" , Name = "a84" },
                    new ComboBoxSearchModel() { Id = 75,SearchText="xxxxaa95" , Name = "a95" },
                    new ComboBoxSearchModel() { Id = 85,SearchText="xxxxa951" , Name = "a951" },
                    new ComboBoxSearchModel() { Id = 95,SearchText="xxxxa952" , Name = "a952" },
                    new ComboBoxSearchModel() { Id = 115,SearchText="xxxxa953" , Name = "a953" },
                    new ComboBoxSearchModel() { Id = 125,SearchText="xxxxa954" , Name = "a954" },
                    new ComboBoxSearchModel() { Id = 135,SearchText="xxxxaa955" , Name = "a955" },
                    new ComboBoxSearchModel() { Id = 145,SearchText="xxxxa11a956" , Name = "a956" },
                    new ComboBoxSearchModel() { Id = 155,SearchText="xxxxa11a957" , Name = "a957" },
                    new ComboBoxSearchModel() { Id = 165,SearchText="xxxxa11a958" , Name = "a958" },
                    new ComboBoxSearchModel() { Id = 175,SearchText="xxxxa11a959" , Name = "a959" },
                    new ComboBoxSearchModel() { Id = 185,SearchText="xxxxa11a950" , Name = "a950" },
                    new ComboBoxSearchModel() { Id = 215,SearchText="xxxxa11a9511" , Name = "a9511" },
                    new ComboBoxSearchModel() { Id = 225,SearchText="xxxxa11a9512" , Name = "a9512" },
                    new ComboBoxSearchModel() { Id = 235,SearchText="xxxxa11a9513" , Name = "a9513" },
                    new ComboBoxSearchModel() { Id = 245,SearchText="xxxxa11a9514" , Name = "a9514" },
                    new ComboBoxSearchModel() { Id = 255,SearchText="xxxxa11a9515" , Name = "a9515" },
                    new ComboBoxSearchModel() { Id = 265,SearchText="xxxxa11a9516" , Name = "a9516" },
                    new ComboBoxSearchModel() { Id = 275,SearchText="xxxxa11a9517" , Name = "a9517" },
                    new ComboBoxSearchModel() { Id = 285,SearchText="xxxxa11a9518" , Name = "a9518" }

                };
                OrganizationLists = ComboBoxSearchService.GetOrganizationLists();
                // CaseLists = ComboBoxSearchService.GetCaseLists();
                MaterialLists = new ObservableCollection<ComboBoxSearchModel>(); //ComboBoxSearchService.GetMaterialLists();
            });
        }

        private void InitCommand()
        {
            SalesRebateAmountRangeCreatecommand = new DelegateCommand((obj) =>
            {
                SalesRebateAmountRangeCreateView view = new SalesRebateAmountRangeCreateView();
                view.ShowDialog();
            });

            SalesRebateAmountRangeModifycommand = new DelegateCommand((obj) =>
            {
                SalesRebateAmountRangeModifyView view = new SalesRebateAmountRangeModifyView();
                view.ShowDialog();
            });
        }

        public DelegateCommand SalesRebateAmountRangeCreatecommand { get; set; }
        public DelegateCommand SalesRebateAmountRangeModifycommand { get; set; }
        public DelegateCommand OrgNameSearchCommand { get; set; }
        public DelegateCommand EnterCommand { get; set; }
        public DelegateCommand MaterialDatabaseSearchCommand { get; set; }


        public override void Save(object obj)
        {

            var s = OrganizationSearchedItem;
            var s1 = CaseSearchedItem;
            var s2 = MaterialSearchedItem;
            var a = Entity;
            base.Save(obj);
        }

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

        private IList<EnumModel> isTaxAmountLists;

        public IList<EnumModel> IsTaxAmountLists
        {
            get { return isTaxAmountLists; }
            set
            {
                isTaxAmountLists = value;
                this.RaisePropertyChanged(nameof(IsTaxAmountLists));
            }
        }

        private IList<EnumModel> rebatePctClassLists;

        public IList<EnumModel> RebatePctClassLists
        {
            get { return rebatePctClassLists; }
            set
            {
                rebatePctClassLists = value;
                this.RaisePropertyChanged(nameof(RebatePctClassLists));
            }
        }

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


        private IEnumerable<ComboBoxSearchModel> materialLists;

        public IEnumerable<ComboBoxSearchModel> MaterialLists
        {
            get { return materialLists; }
            set
            {
                materialLists = value;
                this.RaisePropertyChanged(nameof(MaterialLists));
            }
        }


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

        public string Text { get; set; } = "";



    }
}
