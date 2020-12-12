using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class PrintCommonAdjustmentModel:NotificationObject
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

		private dynamic field1;

		public dynamic Field1
		{
			get { return field1; }
			set
			{
				field1 = value;
				this.RaisePropertyChanged(nameof(Field1));
			}
		}

		private dynamic field2;

		public dynamic Field2
		{
			get { return field2; }
			set
			{
				field2 = value;
				this.RaisePropertyChanged(nameof(Field2));
			}
		}

		private dynamic field3;

		public dynamic Field3
		{
			get { return field3; }
			set
			{
				field3 = value;
				this.RaisePropertyChanged(nameof(Field3));
			}
		}

		private dynamic field4;

		public dynamic Field4
		{
			get { return field4; }
			set
			{
				field4 = value;
				this.RaisePropertyChanged(nameof(Field4));
			}
		}


		private dynamic field5;

		public dynamic Field5
		{
			get { return field5; }
			set
			{
				field5 = value;
				this.RaisePropertyChanged(nameof(Field5));
			}
		}

	}
}
