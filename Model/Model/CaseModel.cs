using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CaseModel:NotificationObject
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

		private string brandName;

		public string BrandName
		{
			get { return brandName; }
			set
			{
				brandName = value;
				this.RaisePropertyChanged(nameof(BrandName));
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



	}
}
