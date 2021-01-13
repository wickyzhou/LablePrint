using Model;
using QueryParameterModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Ui.Command;
using Ui.Service;

namespace Ui.ViewModel
{
    public class ProductionDeptSemiMaterialsInventoryViewModel:NotificationObject
    {
		private readonly SemiMaterialsInventoryService _service;
		public ProductionDeptSemiMaterialsInventoryViewModel()
		{
			_service = new SemiMaterialsInventoryService();
			InitCommand();
		}

		private void InitCommand()
		{
			QueryCommand = new DelegateCommand((obj) => {
				SemiMaterialsLists.Clear();
				MaterialTimelyInventoryLists.Clear();
				_service.GetSemiMaterialsLists(QueryParameter.ProductionDate,QueryParameter.MaterialName,QueryParameter.MaterialNumber,QueryParameter.BatchQty).ForEach(x=>semiMaterialsLists.Add(x));
			});

			SelectionChangedCommand = new DelegateCommand((obj)=> {
				if (SemiMaterialsSelectedItem != null)
				{
					MaterialTimelyInventoryLists.Clear();
					_service.GetTimelyInventoryLists(SemiMaterialsSelectedItem.MaterialId).ForEach(x=> MaterialTimelyInventoryLists.Add(x));
				}
				
			});
		}

	

		private ObservableCollection<SemiMaterialsInventoryModel> semiMaterialsLists = new ObservableCollection<SemiMaterialsInventoryModel>();

		public ObservableCollection<SemiMaterialsInventoryModel> SemiMaterialsLists
		{
			get { return semiMaterialsLists; }
			set
			{
				semiMaterialsLists = value;
				this.RaisePropertyChanged(nameof(SemiMaterialsLists));
			}
		}


		private SemiMaterialsInventoryModel semiMaterialsSelectedItem;

		public SemiMaterialsInventoryModel SemiMaterialsSelectedItem
		{
			get { return semiMaterialsSelectedItem; }
			set
			{
				semiMaterialsSelectedItem = value;
				this.RaisePropertyChanged(nameof(SemiMaterialsSelectedItem));
			}
		}

		private ObservableCollection<MaterialTimelyInventoryModel> materialTimelyInventoryLists = new ObservableCollection<MaterialTimelyInventoryModel>();

		public ObservableCollection<MaterialTimelyInventoryModel> MaterialTimelyInventoryLists
		{
			get { return materialTimelyInventoryLists; }
			set
			{
				materialTimelyInventoryLists = value;
				this.RaisePropertyChanged(nameof(MaterialTimelyInventoryLists));
			}
		}


		private SemiMaterialsInventoryQueryParameterModel queryParameter = new SemiMaterialsInventoryQueryParameterModel { ProductionDate = DateTime.Now.Date, BatchQty = "", MaterialName = "", MaterialNumber = "" };

		public SemiMaterialsInventoryQueryParameterModel QueryParameter
		{
			get { return queryParameter; }
			set
			{
				queryParameter = value;
				this.RaisePropertyChanged(nameof(QueryParameter));
			}
		}

		public DelegateCommand QueryCommand { get; set; }

		public DelegateCommand SelectionChangedCommand { get; set; }

	}
}
