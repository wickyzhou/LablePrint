using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BatchBomRequestSummaryModel:NotificationObject
    {

		private int id;

		public int Id
		{
			get { return id; }
			set
			{
				id = value;
				this.RaisePropertyChanged(nameof(Id));
			}
		}

		private DateTime productionDate;

		public DateTime ProductionDate
		{
			get { return productionDate; }
			set
			{
				productionDate = value;
				this.RaisePropertyChanged(nameof(ProductionDate));
			}
		}

		private int materialId;

		public int MaterialId
		{
			get { return materialId; }
			set
			{
				materialId = value;
				this.RaisePropertyChanged(nameof(MaterialId));
			}
		}

		private string materialNumber;

		public string MaterialNumber
		{
			get { return materialNumber; }
			set
			{
				materialNumber = value;
				this.RaisePropertyChanged(nameof(MaterialNumber));
			}
		}

		private string materialName;

		public string MaterialName
		{
			get { return materialName; }
			set
			{
				materialName = value;
				this.RaisePropertyChanged(nameof(MaterialName));
			}
		}

		private int stockId;

		public int StockId
		{
			get { return stockId; }
			set
			{
				stockId = value;
				this.RaisePropertyChanged(nameof(StockId));
			}
		}

		private string stockNumber;

		public string StockNumber
		{
			get { return stockNumber; }
			set
			{
				stockNumber = value;
				this.RaisePropertyChanged(nameof(StockNumber));
			}
		}

		private string stockName;

		public string StockName
		{
			get { return stockName; }
			set
			{
				stockName = value;
				this.RaisePropertyChanged(nameof(StockName));
			}
		}


		private int batchTypeId;

		public int BatchTypeId
		{
			get { return batchTypeId; }
			set
			{
				batchTypeId = value;
				this.RaisePropertyChanged(nameof(BatchTypeId));
			}
		}

		private string batchTypeNumber;

		public string BatchTypeNumber
		{
			get { return batchTypeNumber; }
			set
			{
				batchTypeNumber = value;
				this.RaisePropertyChanged(nameof(BatchTypeNumber));
			}
		}

		private string batchTypeName;

		public string BatchTypeName
		{
			get { return batchTypeName; }
			set
			{
				batchTypeName = value;
				this.RaisePropertyChanged(nameof(BatchTypeName));
			}
		}

		private string fBatchNoAndActualQty;

		public string FBatchNoAndActualQty
		{
			get { return fBatchNoAndActualQty; }
			set
			{
				fBatchNoAndActualQty = value;
				this.RaisePropertyChanged(nameof(FBatchNoAndActualQty));
			}
		}


		private double? qtyRequest;

		public double? QtyRequest
		{
			get { return qtyRequest; }
			set
			{
				qtyRequest = value;
				this.RaisePropertyChanged(nameof(QtyRequest));
			}
		}

		private double? qtyWorkshopInventory;

		public double? QtyWorkshopInventory
		{
			get { return qtyWorkshopInventory; }
			set
			{
				qtyWorkshopInventory = value;
				this.RaisePropertyChanged(nameof(QtyWorkshopInventory));
			}
		}

		private double? qtyLack;

		public double? QtyLack
		{
			get { return qtyLack; }
			set
			{
				qtyLack = value;
				this.RaisePropertyChanged(nameof(QtyLack));
			}
		}

		private double? qtyTransfering;

		public double? QtyTransfering
		{
			get { return qtyTransfering; }
			set
			{
				qtyTransfering = value;
				this.RaisePropertyChanged(nameof(QtyTransfering));
			}
		}

		private double? qtyTransfered;

		public double? QtyTransfered
		{
			get { return qtyTransfered; }
			set
			{
				qtyTransfered = value;
				this.RaisePropertyChanged(nameof(QtyTransfered));
			}
		}


		private double? qtyUnPick;

		public double? QtyUnPick
		{
			get { return qtyUnPick; }
			set
			{
				qtyUnPick = value;
				this.RaisePropertyChanged(nameof(QtyUnPick));
			}
		}



	}
}
