using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OptionModel:NotificationObject
    {
		private EnumModel enumModel;

		public EnumModel EnumModel
		{
			get { return enumModel; }
			set
			{
				enumModel = value;
				this.RaisePropertyChanged(nameof(EnumModel));
			}
		}

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
