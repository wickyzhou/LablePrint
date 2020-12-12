using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BucketModel:NotificationObject
    {
		private int fBucketID;

		public int FBucketID
		{
			get { return fBucketID; }
			set
			{
				fBucketID = value;
				this.RaisePropertyChanged(nameof(FBucketID));
			}
		}


		private string fName;

		public string FName
		{
			get { return fName; }
			set
			{
				fName = value;
				this.RaisePropertyChanged(nameof(FName));
			}
		}

		private decimal? fWeight;

		public decimal? FWeight
		{
			get { return fWeight; }
			set
			{
				fWeight = value;
				this.RaisePropertyChanged(nameof(FWeight));
			}
		}

		private decimal fOffset;

		public decimal FOffset
		{
			get { return fOffset; }
			set
			{
				fOffset = value;
				this.RaisePropertyChanged(nameof(FOffset));
			}
		}

        public string FKeyword { get; set; }

    }
}
