using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DeliveryStockModel:NotificationObject
    {
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

		private string shortName;

		public string ShortName
		{
			get { return shortName; }
			set
			{
				shortName = value;
				this.RaisePropertyChanged(nameof(ShortName));
			}
		}

		private int targetStockId;

		public int TargetStockId
		{
			get { return targetStockId; }
			set
			{
				targetStockId = value;
				this.RaisePropertyChanged(nameof(TargetStockId));
			}
		}



	}
}
