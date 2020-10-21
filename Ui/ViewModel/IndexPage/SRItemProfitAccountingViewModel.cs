using CRMApiModel.QueryXoqlModel;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ui.Command;
using Ui.Helper;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
    public class SRItemProfitAccountingViewModel : CRMApiBaseViewModel
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
                    ItemProfitAccountingQueryCommand.Execute(null);
                });
            });
        }

        private void InitCommand()
        {
            CRMUserSyncCommand = new DelegateCommand((obj) =>
            {
                var r = CRMService.GetQueryXoqlData<SRUserQueryXoqlModel>(@"select  id Id,name Name,dimDepart DeptId  from user ;");
                if (r.code == 200)
                {
                    SqlHelper.ExecuteNonQuerySR(" truncate table SRUserXoqlTable ;", null);
                    SqlHelper.LoadIEnumerableToDBModelTableSR(r.data.records, "SRUserXoqlTable");
                }
            });

            CRMItemSyncCommand = new DelegateCommand((obj) =>
            {
                List<SROpportunityQueryXoqlDbModel> lists = new List<SROpportunityQueryXoqlDbModel>();
                var ss2 = CRMService.GetQueryXoqlData<SROpportunityQueryXoqlModel>(@" select customItem177__c  ItemCode,opportunityName  ItemName
                            ,ownerId XiangMuJingLi,dbcVarchar6 ShiChangZhiChi,customItem172__c YeWuZhiChi,
                            customItem175__c JiShuZhiChi,customItem182__c PinZhiZhiChi,customItem173__c ChanPinJingLi,customItem180__c JiFuZhiChi,customItem181__c SeCaiZhiChi,createdAt BeginDate
                        from opportunity  where  dimDepart=1047572968276306 ");
                if (ss2.code == 200)
                {
                    SROpportunityQueryXoqlModel[] rr = ss2.data.records;
                    foreach (var item in rr)
                    {
                        SROpportunityQueryXoqlDbModel model = new SROpportunityQueryXoqlDbModel
                        {
                            Id = item.Id,
                            BeginDate = TypeConvertHelper.ConvertTimeStampToDateTime(item.BeginDate),
                            EndDate = TypeConvertHelper.ConvertTimeStampToDateTime(item.EndDate),
                            ItemCode = item.ItemCode,
                            ItemName = item.ItemName,
                            HouDuanZhiChi = item.HouDuanZhiChi,
                            JiFuZhiChi = item.JiFuZhiChi,
                            PinZhiZhiChi = item.PinZhiZhiChi,
                            SeCaiZhiChi = item.SeCaiZhiChi,
                            ShiChangZhiChi = item.ShiChangZhiChi,
                            XiangMuJingLi = item.XiangMuJingLi,
                            YeWuZhiChi = item.YeWuZhiChi==null? "": string.Join(",",item.YeWuZhiChi),
                            ChanPinJingLi = item.ChanPinJingLi == null? "" :string.Join(",", item.ChanPinJingLi),
                            JiShuZhiChi = item.JiShuZhiChi == null ?"":string.Join(",", item.JiShuZhiChi),
                        };

                        lists.Add(model);
                    }
                    SqlHelper.ExecuteNonQuerySR(" truncate table SROpportunityXoqlTable ;", null);
                    SqlHelper.LoadIEnumerableToDBModelTableSR(lists, "SROpportunityXoqlTable");
                }
                else
                {
                    MessageBox.Show($"{ss2.code} \r\n{ss2.msg}");
                }
            });

            EmployeeCostImportCommand = new DelegateCommand((obj) =>
            {
                //文件选择窗口
                System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "选择文件",
                    Filter = "EXCEL文件|*.xls*"  //第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
                };

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
                ItemProfitAccountingQueryCommand.Execute(null);
            });

            ItemProfitAccountingQueryCommand = new DelegateCommand((obj) =>
            {
                ItemProfitAccountingLists.Clear();
                _itemProfitAccountingService.GetItemProfitAccountingLists($" and ItemName like '%{ItemNameParameter}%' ").ForEach(x => ItemProfitAccountingLists.Add(x));
                ListsSum = ItemProfitAccountingLists.Sum(x=>x.Profit);
                ListsCount = ItemProfitAccountingLists.Count();
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
        public DelegateCommand CRMItemSyncCommand { get; set; }
        public DelegateCommand CRMUserSyncCommand { get; set; }



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

        private double? listsSum;

        public double? ListsSum
        {
            get { return listsSum; }
            set
            {
                listsSum = value;
                this.RaisePropertyChanged(nameof(ListsSum));
            }
        }



    }
}
