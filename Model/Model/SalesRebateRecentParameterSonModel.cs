using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateRecentParameterSonModel:NotificationObject
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

		private Guid guid;

		public Guid Guid
		{
			get { return guid; }
			set
			{
				guid = value;
				this.RaisePropertyChanged(nameof(Guid));
			}
		}

		private double amountLower;

		public double AmountLower
		{
			get { return amountLower; }
			set
			{
				amountLower = value;
				this.RaisePropertyChanged(nameof(AmountLower));
			}
		}

		private double amountUpper;

		public double AmountUpper
		{
			get { return amountUpper; }
			set
			{
				amountUpper = value;
				this.RaisePropertyChanged(nameof(AmountUpper));
			}
		}

		private double salesRebatePctValue;

		public double SalesRebatePctValue
		{
			get { return salesRebatePctValue; }
			set
			{
				salesRebatePctValue = value;
				this.RaisePropertyChanged(nameof(SalesRebatePctValue));
			}
		}
    }
}
