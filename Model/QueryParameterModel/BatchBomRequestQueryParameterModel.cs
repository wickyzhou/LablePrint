using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BatchBomRequestQueryParameterModel:NotificationObject
    {
		private DateTime productionDate1;

		public DateTime ProductionDate1
		{
			get { return productionDate1; }
			set
			{
				productionDate1 = value;
				this.RaisePropertyChanged(nameof(ProductionDate1));
			}
		}

		private DateTime productionDate2;

		public DateTime ProductionDate2
		{
			get { return productionDate2; }
			set
			{
				productionDate2 = value;
				this.RaisePropertyChanged(nameof(ProductionDate2));
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
