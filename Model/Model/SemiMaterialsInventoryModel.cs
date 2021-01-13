using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SemiMaterialsInventoryModel:NotificationObject
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

		private double totalQtyRequestOfDay;

		public double TotalQtyRequestOfDay
		{
			get { return totalQtyRequestOfDay; }
			set
			{
				totalQtyRequestOfDay = value;
				this.RaisePropertyChanged(nameof(TotalQtyRequestOfDay));
			}
		}

		private double pQtyRequestOfDay;

		public double PQtyRequestOfDay
		{
			get { return pQtyRequestOfDay; }
			set
			{
				pQtyRequestOfDay = value;
				this.RaisePropertyChanged(nameof(PQtyRequestOfDay));
			}
		}

		private double pQtyUnPicked;

		public double PQtyUnPicked
		{
			get { return pQtyUnPicked; }
			set
			{
				pQtyUnPicked = value;
				this.RaisePropertyChanged(nameof(PQtyUnPicked));
			}
		}

		private double yQtyRequestOfDay;

		public double YQtyRequestOfDay
		{
			get { return yQtyRequestOfDay; }
			set
			{
				yQtyRequestOfDay = value;
				this.RaisePropertyChanged(nameof(YQtyRequestOfDay));
			}
		}

		private double yQtyUnPicked;

		public double YQtyUnPicked
		{
			get { return yQtyUnPicked; }
			set
			{
				yQtyUnPicked = value;
				this.RaisePropertyChanged(nameof(YQtyUnPicked));
			}
		}

		private double wQtyRequestOfDay;

		public double WQtyRequestOfDay
		{
			get { return wQtyRequestOfDay; }
			set
			{
				wQtyRequestOfDay = value;
				this.RaisePropertyChanged(nameof(WQtyRequestOfDay));
			}
		}

		private double wQtyUnPicked;

		public double WQtyUnPicked
		{
			get { return wQtyUnPicked; }
			set
			{
				wQtyUnPicked = value;
				this.RaisePropertyChanged(nameof(WQtyUnPicked));
			}
		}

		private double fQtyRequestOfDay;

		public double FQtyRequestOfDay
		{
			get { return fQtyRequestOfDay; }
			set
			{
				fQtyRequestOfDay = value;
				this.RaisePropertyChanged(nameof(FQtyRequestOfDay));
			}
		}

		private double fQtyUnPicked;

		public double FQtyUnPicked
		{
			get { return fQtyUnPicked; }
			set
			{
				fQtyUnPicked = value;
				this.RaisePropertyChanged(nameof(FQtyUnPicked));
			}
		}

		private double qtyInventoryTimely;

		public double QtyInventoryTimely
		{
			get { return qtyInventoryTimely; }
			set
			{
				qtyInventoryTimely = value;
				this.RaisePropertyChanged(nameof(QtyInventoryTimely));
			}
		}

		private double qtyLack;

		public double QtyLack
		{
			get { return qtyLack; }
			set
			{
				qtyLack = value;
				this.RaisePropertyChanged(nameof(QtyLack));
			}
		}

    }
}
