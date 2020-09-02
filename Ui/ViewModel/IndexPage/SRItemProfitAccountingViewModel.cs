using Common;
using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class SRItemProfitAccountingViewModel : BaseViewModel
    {
        private ItemProfitAccountingService _itemProfitAccountingService;

        public SRItemProfitAccountingViewModel()
        {
            _itemProfitAccountingService = new ItemProfitAccountingService();
            InitCommand();
            InitData();
        }

        private void InitData()
        {

            Task.Factory.StartNew(() =>
            {
                SettleMonthSearchedItem = new ComboBoxSearchModel() { Id = Convert.ToInt32(DateTime.Now.Date.AddMonths(-10).ToString("yyyyMM")), SearchText = DateTime.Now.Date.AddMonths(-10).ToString("yyyy-MM") };
                ItemProfitAccountingLists = new ObservableCollection<ItemProfitAccountingModel>();
                ItemProfitAccountingMonthlyLists = new ObservableCollection<ItemProfitAccountingMonthlyModel>();
                ItemProfitAccountingSelectedItem = new ItemProfitAccountingModel();
                UIExecute.RunAsync(() =>
                {
                    SettleMonthLists = _itemProfitAccountingService.GetSettleMonthLists();
                    _itemProfitAccountingService.GetItemProfitAccountingLists().ForEach(x => ItemProfitAccountingLists.Add(x));
                    //_itemProfitAccountingService.GetItemProfitAccountingMonthlyLists().ForEach(x => ItemProfitAccountingMonthlyLists.Add(x));
                });
            });
        }

        private void InitCommand()
        {


            EmployeeCostImportCommand = new DelegateCommand((obj) =>
            {
                //文件选择窗口
                System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog();
                opd.Title = "选择文件";
                //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
                opd.Filter = "EXCEL文件|*.xls*";

                if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int monthId = Convert.ToInt32(CommonService.ImportExcelToDatabaseTableWithoutSeq(opd.FileName, "SREmployeeCost", "SR"));
                    SettleMonthSearchedItem.Id = monthId;
                    SettleMonthSearchedItem.SearchText = monthId.ToString();
                    // 数据验证后加载验证模型列表到表格
                    //MessageBox.Show("导入成功");
                }
                opd.Dispose();
            });

            ItemProfitSettleCommand = new DelegateCommand((obj) =>
            {
                if (SettleMonthSearchedItem == null)
                {
                    MessageBox.Show("请先导入人员支出Excel");
                    return;
                }

                //同步OA费用数据到松润数据库
                DataTable dataTable = _itemProfitAccountingService.GetOaItemCostDataTable(SettleMonthSearchedItem.Id);
                _itemProfitAccountingService.ImportDataTableToDatabaseTableSR(dataTable, "SROaItemCost");

                // 计算最终的结果
                _itemProfitAccountingService.AccountItemProfit(SettleMonthSearchedItem.Id);
                ItemProfitAccountingLists.Clear();
                _itemProfitAccountingService.GetItemProfitAccountingLists($" and ItemName like '%{ItemNameParameter}%' ").ForEach(x => ItemProfitAccountingLists.Add(x));
            });

            ItemProfitAccountingQueryCommand = new DelegateCommand((obj) =>
            {
                ItemProfitAccountingLists.Clear();
                _itemProfitAccountingService.GetItemProfitAccountingLists($" and ItemName like '%{ItemNameParameter}%' ").ForEach(x => ItemProfitAccountingLists.Add(x));
            });

            ItemProfitDeleteCommand = new DelegateCommand((obj) =>
            {
                MessageBoxResult result = MessageBox.Show($"确认删除【{SettleMonthSearchedItem.Id}】已经结算的数据", "温馨提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                }
            });

            ItemProfitMonthlyQueryCommand = new DelegateCommand((obj) =>
            {
                if (SettleMonthSearchedItem == null)
                    return;
                ItemProfitAccountingMonthlyLists.Clear();
                _itemProfitAccountingService.GetItemProfitAccountingMonthlyLists($" and SettleMonth = {SettleMonthSearchedItem.Id} ").ForEach(x => ItemProfitAccountingMonthlyLists.Add(x));
            });

            SelectionChangedCommand = new DelegateCommand((obj) =>
            {
                if (ItemProfitAccountingSelectedItem == null)
                    return;
                ItemProfitAccountingMonthlyLists.Clear();
                _itemProfitAccountingService.GetItemProfitAccountingMonthlyLists($" and ItemCode = {ItemProfitAccountingSelectedItem.ItemCode} ").ForEach(x => ItemProfitAccountingMonthlyLists.Add(x));

            });
        }

        public DelegateCommand ItemProfitAccountingQueryCommand { get; set; }
        public DelegateCommand EmployeeCostImportCommand { get; set; }
        public DelegateCommand ItemProfitSettleCommand { get; set; }
        public DelegateCommand ItemProfitDeleteCommand { get; set; }
        public DelegateCommand ItemProfitMonthlyQueryCommand { get; set; }
        public DelegateCommand SelectionChangedCommand { get; set; }



        private ObservableCollection<ItemProfitAccountingModel> itemProfitAccountingLists;

        public ObservableCollection<ItemProfitAccountingModel> ItemProfitAccountingLists
        {
            get { return itemProfitAccountingLists; }
            set
            {
                itemProfitAccountingLists = value;
                this.RaisePropertyChanged(nameof(ItemProfitAccountingLists));
            }
        }

        private ObservableCollection<ItemProfitAccountingMonthlyModel> itemProfitAccountingMonthlyLists;

        public ObservableCollection<ItemProfitAccountingMonthlyModel> ItemProfitAccountingMonthlyLists
        {
            get { return itemProfitAccountingMonthlyLists; }
            set
            {
                itemProfitAccountingMonthlyLists = value;
                this.RaisePropertyChanged(nameof(ItemProfitAccountingMonthlyLists));
            }
        }

        private ItemProfitAccountingModel itemProfitAccountingSelectedItem;

        public ItemProfitAccountingModel ItemProfitAccountingSelectedItem
        {
            get { return itemProfitAccountingSelectedItem; }
            set
            {
                itemProfitAccountingSelectedItem = value;
                this.RaisePropertyChanged(nameof(ItemProfitAccountingSelectedItem));
            }
        }

        public List<ComboBoxSearchModel> SettleMonthLists { get; set; }

        private ComboBoxSearchModel settleMonthSearchedItem;

        public ComboBoxSearchModel SettleMonthSearchedItem
        {
            get { return settleMonthSearchedItem; }
            set
            {
                settleMonthSearchedItem = value;
                this.RaisePropertyChanged(nameof(SettleMonthSearchedItem));
            }
        }

        private string itemNameParameter;

        public string ItemNameParameter
        {
            get { return itemNameParameter; }
            set
            {
                itemNameParameter = value;
                this.RaisePropertyChanged(nameof(ItemNameParameter));
            }
        }

    }
}
