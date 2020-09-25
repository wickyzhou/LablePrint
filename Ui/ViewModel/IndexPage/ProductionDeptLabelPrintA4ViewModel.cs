using Model;
using QueryParameterModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel.IndexPage
{
	public class ProductionDeptLabelPrintA4ViewModel:BaseViewModel
    {
		private readonly PrintA4Service _service;
		private readonly DataGridManagementService _dataGridManagementService;
		public ProductionDeptLabelPrintA4ViewModel()
		{
			_service = new PrintA4Service();
			_dataGridManagementService = new DataGridManagementService();
			InitCommand();
			InitData();
		}

		public DelegateCommand PrintCommand { get; set; }
		public DelegateCommand QueryCommand { get; set; }
		public DelegateCommand DataGridManageCommand { get; set; }
		public DelegateCommand DataGridColumnHideCommand { get; set; }
		public DelegateCommand DataGridColorChangeCommand { get; set; }
		public DelegateCommand DataGridSaveCommand { get; set; }


		private void InitCommand()
		{
			DataGridSaveCommand = new DelegateCommand((obj) =>
			{
				var grid = obj as DataGrid;

				// 同步用户自定义后台参数
				_dataGridManagementService.SyncUserDataGridColumnConfiguration(grid.Name,User.ID);

				// 将界面参数更新到后台

				StringBuilder stringBuilder = new StringBuilder(" begin  tran ");
				for (int i = 0; i < grid.Columns.Count; i++)
				{
					//主要是更新顺序以及宽度
					var column = (obj as DataGrid).Columns[i] as DataGridTextColumn;
					int order = column.DisplayIndex;
					double width = column.ActualWidth;
					string fieldName = (column.Binding as Binding).Path.Path;

					stringBuilder.Append($" update SJDataGridUserCustom set ColumnWidth ={width} ,ColumnWidthUnitType='',ColumnOrder = {order}  where DataGridName = {grid.Name} and UserId = {User.ID} and ColumnFieldName ={fieldName} ;");
				}
				stringBuilder.Append("commit;");


			});

			DataGridColumnHideCommand = new DelegateCommand((obj) =>
			{
				IsColumnVisibility = !IsColumnVisibility;

				//var ss= obj as DataGridColumnHeader;
				//string filedName = ((ss.Column as DataGridTextColumn).Binding as Binding).Path.Path;
				//var entry = ColumnUserCustomLists.Find(x => x.ColumnFieldName == filedName);
				//bool v= entry.ColumnVisibility ;
				//entry.ColumnVisibility = !v;
				//IsColumnVisibility = entry.ColumnVisibility;
		});

			DataGridColorChangeCommand = new DelegateCommand((obj) =>
			{
				IsColumnVisibility = !IsColumnVisibility;
			});

			PrintCommand = new DelegateCommand((obj) =>
			{
				IsColumnVisibility = !IsColumnVisibility;
			});

			QueryCommand = new DelegateCommand((obj) =>
			{
				PrintHistoryLists.Clear();
				_service.GetHistoryLists(CommonService.GetSqlWhereString(QueryParameter)).ForEach(x => PrintHistoryLists.Add(x));
			});

			DataGridManageCommand = new DelegateCommand((obj) =>
			{
				// 同步用户自定义后台参数
				_dataGridManagementService.SyncUserDataGridColumnConfiguration("DGPrintA4", User.ID);
				var grid = obj as DataGrid;
				for (int i = 0; i < grid.Columns.Count; i++)
				{
					//主要是更新顺序以及宽度
					var column = (obj as DataGrid).Columns[i] as DataGridTextColumn;
					int order = column.DisplayIndex;
					string fieldName = (column.Binding as Binding).Path.Path;
				}

				string sql = $" update ";


			});

		}

		

		private void InitData()
		{
		
			Task.Factory.StartNew(() =>
			{
				UIExecute.RunAsync(() =>
				{
					PrintHistoryLists = new ObservableCollection<LabelPrintHistoryModel>(); 
					QueryParameter = new ProductionDeptLabelPrintA4QueryParameterModel { ProductionDate = DateTime.Now.Date};
					ColumnUserCustomLists = _dataGridManagementService.GetUserDataGridColumnLists("DGPrintA4", User.ID);
				});
			});
		}

		private ProductionDeptLabelPrintA4QueryParameterModel queryParameter;

		public ProductionDeptLabelPrintA4QueryParameterModel QueryParameter
		{
			get { return queryParameter; }
			set
			{
				queryParameter = value;
				this.RaisePropertyChanged(nameof(QueryParameter));
			}
		}

		private ObservableCollection<LabelPrintHistoryModel> printHistoryLists;

		public ObservableCollection<LabelPrintHistoryModel> PrintHistoryLists
		{
			get { return printHistoryLists; }
			set
			{
				printHistoryLists = value;
				this.RaisePropertyChanged(nameof(PrintHistoryLists));
			}
		}

		private List<DataGridColumnHeaderUserCustomModel> columnUserCustomLists;

		public List<DataGridColumnHeaderUserCustomModel> ColumnUserCustomLists
		{
			get { return columnUserCustomLists; }
			set
			{
				columnUserCustomLists = value;
				this.RaisePropertyChanged(nameof(ColumnUserCustomLists));
			}
		}


		private bool isColumnVisibility = true;

		public bool IsColumnVisibility
		{
			get { return isColumnVisibility; }
			set
			{
				isColumnVisibility = value;
				this.RaisePropertyChanged(nameof(IsColumnVisibility));
			}
		}

	}



}
