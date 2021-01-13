using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PrintCommonAdjustmentModel:AGenericFieldModel
    {

        public int Id { get; set; }

		private int typeId;

		public int TypeId
		{
			get { return typeId; }
			set
			{
				typeId = value;
				this.RaisePropertyChanged(nameof(TypeId));
			}
		}

		private string orgId;

		public string OrgId
		{
			get { return orgId; }
			set
			{
				orgId = value;
				this.RaisePropertyChanged(nameof(OrgId));
			}
		}

		private string label;

		public string Label
		{
			get { return label; }
			set
			{
				label = value;
				this.RaisePropertyChanged(nameof(Label));
			}
		}

		private string productionName;

		public string ProductionName
		{
			get { return productionName; }
			set
			{
				productionName = value;
				this.RaisePropertyChanged(nameof(ProductionName));
			}
		}

		private int? expirationMonth;

		public int? ExpirationMonth
		{
			get { return expirationMonth; }
			set
			{
				expirationMonth = value;
				this.RaisePropertyChanged(nameof(ExpirationMonth));
			}
		}

		private int? expirationDay;

		public int? ExpirationDay
		{
			get { return expirationDay; }
			set
			{
				expirationDay = value;
				this.RaisePropertyChanged(nameof(ExpirationDay));
			}
		}

		private double? netWeight;

		public double? NetWeight
		{
			get { return netWeight; }
			set
			{
				netWeight = value;
				this.RaisePropertyChanged(nameof(NetWeight));
			}
		}

		private DateTime createTime;

		public DateTime CreateTime
		{
			get { return createTime; }
			set
			{
				createTime = value;
				this.RaisePropertyChanged(nameof(CreateTime));
			}
		}


	}
}
