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
                    new ComboBoxSearchModel() { Id=1 , SearchText="a11" },
                    new ComboBoxSearchModel() { Id = 2, SearchText = "a12" },
                    new ComboBoxSearchModel() { Id = 3, SearchText = "b12" },
                    new ComboBoxSearchModel() { Id = 4, SearchText = "a24" },
                    new ComboBoxSearchModel() { Id = 14, SearchText = "a34" },
                    new ComboBoxSearchModel() { Id = 24, SearchText = "a44" },
                    new ComboBoxSearchModel() { Id = 34, SearchText = "a53" },
                    new ComboBoxSearchModel() { Id = 44, SearchText = "a63" },
                    new ComboBoxSearchModel() { Id = 54, SearchText = "a74" },
                    new ComboBoxSearchModel() { Id = 64, SearchText = "a84" },
                    new ComboBoxSearchModel() { Id = 75, SearchText = "a95" },
                    new ComboBoxSearchModel() { Id = 85, SearchText = "a951" },
                    new ComboBoxSearchModel() { Id = 95, SearchText = "a952" },
                    new ComboBoxSearchModel() { Id = 115, SearchText = "a953" },
                    new ComboBoxSearchModel() { Id = 125, SearchText = "a954" },
                    new ComboBoxSearchModel() { Id = 135, SearchText = "a955" },
                    new ComboBoxSearchModel() { Id = 145, SearchText = "a956" },
                    new ComboBoxSearchModel() { Id = 155, SearchText = "a957" },
                    new ComboBoxSearchModel() { Id = 165, SearchText = "a958" },
                    new ComboBoxSearchModel() { Id = 175, SearchText = "a959" },
                    new ComboBoxSearchModel() { Id = 185, SearchText = "a950" },
                    new ComboBoxSearchModel() { Id = 215, SearchText = "a9511" },
                    new ComboBoxSearchModel() { Id = 225, SearchText = "a9512" },
                    new ComboBoxSearchModel() { Id = 235, SearchText = "a9513" },
                    new ComboBoxSearchModel() { Id = 245, SearchText = "a9514" },
                    new ComboBoxSearchModel() { Id = 255, SearchText = "a9515" },
                    new ComboBoxSearchModel() { Id = 265, SearchText = "a9516" },
                    new ComboBoxSearchModel() { Id = 275, SearchText = "a9517" },
                    new ComboBoxSearchModel() { Id = 285, SearchText = "a9518" }

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
