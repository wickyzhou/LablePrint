using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryParameterModel
{
    public class PrintCommonAdjustmentQueryParameterModel:NotificationObject
    {
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
    }
}
