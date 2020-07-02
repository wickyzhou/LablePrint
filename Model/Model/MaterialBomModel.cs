using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialBomModel:NotificationObject
    {
		private bool isChecked =false;

		public bool  IsChecked
		{
			get { return isChecked; }
			set
			{
				isChecked = value;
				this.RaisePropertyChanged(nameof(IsChecked));
			}
		}

		public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int BomCount { get; set; }

    }
}
