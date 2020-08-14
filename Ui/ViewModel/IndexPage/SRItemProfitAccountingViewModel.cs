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
    public class SRItemProfitAccountingViewModel:BaseViewModel
    {
		private  ItemProfitAccountingService _itemProfitAccountingService;

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
				SettleMonthSearchedItem = new ComboBoxSearchModel() { Id = Convert.ToInt32(DateTime.Now.Date.AddMonths(-3).ToString("yyyyMM")), SearchText = DateTime.Now.Date.AddMonths(-3).ToString("yyyy-MM") };
				ItemProfitAccountingLists = new ObservableCollection<ItemProfitAccountingModel>();
				ItemProfitAccountingMonthlyLists = new ObservableCollection<ItemProfitAccountingMonthlyModel>();

				UIExecute.RunAsync(() =>
				{
					SettleMonthLists = _itemProfitAccountingService.GetSettleMonthLists();
					_itemProfitAccountingService.GetItemProfitAccountingLists().ForEach(x => ItemProfitAccountingLists.Add(x));
					_itemProfitAccountingService.GetItemProfitAccountingMonthlyLists().ForEach(x => ItemProfitAccountingMonthlyLists.Add(x));
				});
			});
		}

		private void InitCommand()
		{
			ItemProfitAccountingQueryCommand = new DelegateCommand((obj)=> 
			{
				var monthId= SettleMonthSearchedItem.Id;
			});

			EmployeeCostImportCommand = new DelegateCommand((obj) =>
			{
				//文件选择窗口
				System.Windows.Forms.OpenFileDialog opd = new System.Windows.Forms.OpenFileDialog();
				opd.Title = "选择文件";
				//第一个参数是名称，随意取，第二个是模式匹配， 多个也是用“|”分割
				opd.Filter = "EXCEL文件|*.xls*";

				if (opd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{	
					int monthId = Convert.ToInt32( CommonService.ImportExcelToDatabaseTable(opd.FileName, "SREmployeeCostExcelTemplate") );
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
				

			});
		}

		public DelegateCommand ItemProfitAccountingQueryCommand { get; set; }
		public DelegateCommand EmployeeCostImportCommand { get; set; }
		public DelegateCommand ItemProfitSettleCommand { get; set; }


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

		private ItemProfitAccountingMonthlyModel itemProfitAccountingSelectedItem;

		public ItemProfitAccountingMonthlyModel ItemProfitAccountingSelectedItem
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

	}
}
