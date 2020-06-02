using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SelectedConsignmentBillsModel:NotificationObject
    {
		private int interId;

		public int InterId
		{
			get { return interId; }
			set
			{
				interId = value;
				this.RaisePropertyChanged(nameof(InterId));
			}
		}


		private string billNo;

		public string BillNo
		{
			get { return billNo; }
			set
			{
				billNo = value;
				this.RaisePropertyChanged(nameof(BillNo));
			}
		}

		private float currencyQuatity;

		public float CurrencyQuatity
		{
			get { return currencyQuatity; }
			set
			{
				currencyQuatity = value;
				this.RaisePropertyChanged(nameof(CurrencyQuatity));
			}
		}


	}
}
