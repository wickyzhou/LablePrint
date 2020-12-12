using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateSummaryModel:NotificationObject
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

		private int orgId;

		public int OrgId
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

		private string rebateClassName;

		public string RebateClassName
		{
			get { return rebateClassName; }
			set
			{
				rebateClassName = value;
				this.RaisePropertyChanged(nameof(RebateClassName));
			}
		}

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

		private double orgAmount;

		public double OrgAmount
		{
			get { return orgAmount; }
			set
			{
				orgAmount = value;
				this.RaisePropertyChanged(nameof(OrgAmount));
			}
		}

		private string k3BillNo;

		public string K3BillNo
		{
			get { return k3BillNo; }
			set
			{
				k3BillNo = value;
				this.RaisePropertyChanged(nameof(K3BillNo));
			}
		}

		private DateTime? k3BillDate;

		public DateTime? K3BillDate
		{
			get { return k3BillDate; }
			set
			{
				k3BillDate = value;
				this.RaisePropertyChanged(nameof(K3BillDate));
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

		private int userId;

		public int UserId
		{
			get { return userId; }
			set
			{
				userId = value;
				this.RaisePropertyChanged(nameof(UserId));
			}
		}

		private string userName;

		public string UserName
		{
			get { return userName; }
			set
			{
				userName = value;
				this.RaisePropertyChanged(nameof(UserName));
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

		private DateTime? billDate;

		public DateTime? BillDate
		{
			get { return billDate; }
			set
			{
				billDate = value;
				this.RaisePropertyChanged(nameof(BillDate));
			}
		}

	}
}
