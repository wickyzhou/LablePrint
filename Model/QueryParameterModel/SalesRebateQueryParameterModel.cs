using Model;
using System;

namespace QueryParameterModel
{
	public class SalesRebateQueryParameterModel : NotificationObject
    {
		private string orgCode;

		public string OrgCode
		{
			get { return orgCode; }
			set
			{
				orgCode = value;
				this.RaisePropertyChanged(nameof(OrgCode));
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

		private DateTime settleDateEnd1;

		public DateTime SettleDateEnd1
		{
			get { return settleDateEnd1; }
			set
			{
				settleDateEnd1 = value;
				this.RaisePropertyChanged(nameof(SettleDateEnd1));
			}
		}

		private DateTime settleDateEnd2;

		public DateTime SettleDateEnd2
		{
			get { return settleDateEnd2; }
			set
			{
				settleDateEnd2 = value;
				this.RaisePropertyChanged(nameof(SettleDateEnd2));
			}
		}


	}
}
