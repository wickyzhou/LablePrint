using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryParameterModel
{
    public class SalesRebateRecentQueryParameterModel: NotificationObject
    {
		private int? orgId;

		public int? OrgId
		{
			get { return orgId; }
			set
			{
				orgId = value;
				this.RaisePropertyChanged(nameof(OrgId));
			}
		}

		private int rebateClass;

		public int RebateClass
		{
			get { return rebateClass; }
			set
			{
				rebateClass = value;
				this.RaisePropertyChanged(nameof(RebateClass));
			}
		}

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

	}
}
