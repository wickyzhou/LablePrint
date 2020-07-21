using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ExportDynamicCheckBoxModel:NotificationObject
    {
		private bool isChecked;

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				isChecked = value;
				this.RaisePropertyChanged(nameof(IsChecked));
			}
		}

	}
}
