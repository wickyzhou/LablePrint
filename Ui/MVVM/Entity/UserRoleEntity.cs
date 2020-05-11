using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.Entity
{
    public class UserRoleEntity : ViewModelBase
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

		private int roleId;

		public int RoleId
		{
			get { return roleId; }
			set
			{
				roleId = value;
				this.RaisePropertyChanged(nameof(RoleId));
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

		private string roleName;

		public string RoleName
		{
			get { return roleName; }
			set
			{
				roleName = value;
				this.RaisePropertyChanged(nameof(RoleName));
			}
		}


	}
}
