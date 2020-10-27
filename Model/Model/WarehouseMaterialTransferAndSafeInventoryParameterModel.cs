using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WarehouseMaterialTransferAndSafeInventoryParameterModel:NotificationObject
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

		public DateTime ProductionDateEndDate
		{
			get { return productionDateEnd; }
			set
			{
				productionDateEnd = value;
				this.RaisePropertyChanged(nameof(ProductionDateEndDate));
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

		private string batchTypeCode;

		public string BatchTypeCode
		{
			get { return batchTypeCode; }
			set
			{
				batchTypeCode = value;
				this.RaisePropertyChanged(nameof(BatchTypeCode));
			}
		}


		private bool isTransfering;

		public bool IsTransfering
		{
			get { return isTransfering; }
			set
			{
				isTransfering = value;
				this.RaisePropertyChanged(nameof(IsTransfering));
			}
		}

		private bool isTransfered;

		public bool IsTransfered
		{
			get { return isTransfered; }
			set
			{
				isTransfered = value;
				this.RaisePropertyChanged(nameof(IsTransfered));
			}
		}

		private bool isDeleted;

		public bool IsDeleted
		{
			get { return isDeleted; }
			set
			{
				isDeleted = value;
				this.RaisePropertyChanged(nameof(IsDeleted));
			}
		}


	}
}
