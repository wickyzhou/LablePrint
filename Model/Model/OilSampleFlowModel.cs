using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OilSampleFlowModel:NotificationObject
    {
        public Int64 Id { get; set; }

        public string Title { get; set; }

		private int oilPrintCount;

		public int OilPrintCount
		{
			get { return oilPrintCount; }
			set
			{
				oilPrintCount = value;
				this.RaisePropertyChanged(nameof(OilPrintCount));
			}
		}

		private int expressPrintCount;

		public int ExpressPrintCount
		{
			get { return expressPrintCount; }
			set
			{
				expressPrintCount = value;
				this.RaisePropertyChanged(nameof(ExpressPrintCount));
			}
		}

		private string currentBatchNo;

		public string CurrentBatchNo
		{
			get { return currentBatchNo; }
			set
			{
				currentBatchNo = value;
				this.RaisePropertyChanged(nameof(CurrentBatchNo));
			}
		}

	}
}
