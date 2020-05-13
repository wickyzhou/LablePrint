using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.Entity
{
    public class UserEntity : ViewModelBase
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


		private string loginName;

		public string LoginName
		{
			get { return loginName; }
			set
			{
				loginName = value;
				this.RaisePropertyChanged(nameof(LoginName));
			}
		}

		private string password;

		public string Password
		{
			get { return password; }
			set
			{
				password = value;
				this.RaisePropertyChanged(nameof(Password));
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

		private int superAdmin;

		public int SuperAdmin
		{
			get { return superAdmin; }
			set
			{
				superAdmin = value;
				this.RaisePropertyChanged(nameof(SuperAdmin));
			}
		}

    }
}
