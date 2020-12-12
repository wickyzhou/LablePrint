using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryParameterModel
{
    public class SalesRebateReportQueryParameterModel:NotificationObject
    {
		private DateTime billDate1;

		public DateTime BillDate1
		{
			get { return billDate1; }
			set
			{
				billDate1 = value;
				this.RaisePropertyChanged(nameof(BillDate1));
			}
		}

		private DateTime billDate2;

		public DateTime BillDate2
		{
			get { return billDate2; }
			set
			{
				billDate2 = value;
				this.RaisePropertyChanged(nameof(BillDate2));
			}
		}

		private string orgName;

		public string OrgName
		{
			get { return orgName; }
			set
			{
				orgName = value;
				this.RaisePropertyChanged(nameof(OrgName));
			}
		}

		private string caseName;

		public string CaseName
		{
			get { return caseName; }
			set
			{
				caseName = value;
				this.RaisePropertyChanged(nameof(CaseName));
			}
		}

		private string brandName;

		public string BrandName
		{
			get { return brandName; }
			set
			{
				brandName = value;
				this.RaisePropertyChanged(nameof(BrandName));
			}
		}
	}
}
