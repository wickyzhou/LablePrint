using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.ViewModel;

namespace Ui.MVVM.Entity
{
    public class OrganizationEntity:ViewModelBase
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

		private string shortName;

		public string ShortName
		{
			get { return shortName; }
			set
			{
				shortName = value;
				this.RaisePropertyChanged(nameof(ShortName));
			}
		}

		private string fullName;

		public string FullName
		{
			get { return fullName; }
			set
			{
				fullName = value;
				this.RaisePropertyChanged(nameof(FullName));
			}
		}

    }
}
