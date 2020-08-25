using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
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

		private string settleDateBegin;

		public string SettleDateBegin
		{
			get { return settleDateBegin; }
			set
			{
				settleDateBegin = value;
				this.RaisePropertyChanged(nameof(SettleDateBegin));
			}
		}

		private string settleDateEnd;

		public string SettleDateEnd
		{
			get { return settleDateEnd; }
			set
			{
				settleDateEnd = value;
				this.RaisePropertyChanged(nameof(SettleDateEnd));
			}
		}


	}
}
