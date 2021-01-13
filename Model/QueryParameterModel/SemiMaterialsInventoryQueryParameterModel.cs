using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace QueryParameterModel
{
    public class SemiMaterialsInventoryQueryParameterModel: NotificationObject
    {
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

		private string batchQty;

		public string BatchQty
		{
			get { return batchQty; }
			set
			{
				batchQty = value;
				this.RaisePropertyChanged(nameof(BatchQty));
			}
		}


	}
}
