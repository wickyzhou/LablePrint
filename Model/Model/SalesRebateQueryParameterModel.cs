using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateQueryParameterModel : NotificationObject
    {
		private string productionModelName;

		public string ProductionModelName
		{
			get { return productionModelName; }
			set
			{
				productionModelName = value;
				this.RaisePropertyChanged(nameof(ProductionModelName));
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
