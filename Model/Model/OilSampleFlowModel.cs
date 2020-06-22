using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OilSampleFlowModel:NotificationObject
    {
        public decimal Id { get; set; }

        public string Title { get; set; }

		private int expressPrintedCount;

		public int ExpressPrintedCount
		{
			get { return expressPrintedCount; }
			set
			{
				expressPrintedCount = value;
				this.RaisePropertyChanged(nameof(ExpressPrintedCount));
			}
		}

	}
}
