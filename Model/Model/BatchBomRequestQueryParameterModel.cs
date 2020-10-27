using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BatchBomRequestQueryParameterModel:NotificationObject
    {
		private DateTime productionDateBegin;

		public DateTime ProductionDateBegin
		{
			get { return productionDateBegin; }
			set
			{
				productionDateBegin = value;
				this.RaisePropertyChanged(nameof(ProductionDateBegin));
			}
		}

		private DateTime productionDateEnd;

		public DateTime ProductionDateEnd
		{
			get { return productionDateEnd; }
			set
			{
				productionDateEnd = value;
				this.RaisePropertyChanged(nameof(ProductionDateEnd));
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


	}
}
