using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.Entity
{
    public class RoleEntity : ViewModelBase
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

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				this.RaisePropertyChanged(nameof(Name));
			}
		}

	}
}
