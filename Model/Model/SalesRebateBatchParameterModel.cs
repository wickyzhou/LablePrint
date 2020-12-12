using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateBatchParameterModel : NotificationObject
    {
		private DateTime settleDateBegin;

		public DateTime SettleDateBegin
		{
			get { return settleDateBegin; }
			set
			{
				settleDateBegin = value;
				this.RaisePropertyChanged(nameof(SettleDateBegin));
			}
		}


		private DateTime settleDateEnd;

		public DateTime SettleDateEnd
		{
			get { return settleDateEnd; }
			set
			{
				settleDateEnd = value;
				this.RaisePropertyChanged(nameof(SettleDateEnd));
			}
		}

		private ComboBoxSearchModel organizationSearchedItem;

		public ComboBoxSearchModel OrganizationSearchedItem
		{
			get { return organizationSearchedItem; }
			set
			{
				organizationSearchedItem = value;
				this.RaisePropertyChanged(nameof(OrganizationSearchedItem));
			}
		}

		private EnumModel rebateClassSeletedItem;

		public EnumModel RebateClassSeletedItem
		{
			get { return rebateClassSeletedItem; }
			set
			{
				rebateClassSeletedItem = value;
				this.RaisePropertyChanged(nameof(RebateClassSeletedItem));
			}
		}



	}
}
