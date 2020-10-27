using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BatchBomRequestDetailModel:NotificationObject
    {
		private int fItemID;

		public int FItemID
		{
			get { return fItemID; }
			set
			{
				fItemID = value;
				this.RaisePropertyChanged(nameof(FItemID));
			}
		}

		private string fItemName;

		public string FItemName
		{
			get { return fItemName; }
			set
			{
				fItemName = value;
				this.RaisePropertyChanged(nameof(FItemName));
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




		private float fQty;

		public float FQty
		{
			get { return fQty; }
			set
			{
				fQty = value;
				this.RaisePropertyChanged(nameof(FQty));
			}
		}

		private string batchQuantity;

		public string BatchQuantity
		{
			get { return batchQuantity; }
			set
			{
				batchQuantity = value;
				this.RaisePropertyChanged(nameof(BatchQuantity));
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


	}
}
