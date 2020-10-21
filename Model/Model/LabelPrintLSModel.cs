using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
	public class LabelPrintLSModel : NotificationObject
	{
		private int? printCount;

		public int? PrintCount
		{
			get { return printCount; }
			set
			{
				printCount = value;
				this.RaisePropertyChanged(nameof(PrintCount));
			}
		}

		private double? paintSamplePerBucket;

		public double? PaintSamplePerBucket
		{
			get { return paintSamplePerBucket; }
			set
			{
				paintSamplePerBucket = value;
				this.RaisePropertyChanged(nameof(PaintSamplePerBucket));
			}
		}

		private int historyId;

		public int HistoryId
		{
			get { return historyId; }
			set
			{
				historyId = value;
				this.RaisePropertyChanged(nameof(HistoryId));
			}
		}

		private DateTime productionDate;

		public DateTime ProductionDate
		{
			get { return productionDate; }
			set
			{
				productionDate = value;
				this.RaisePropertyChanged(nameof(ProductionDate));
			}
		}

		private string label;

		public string Label
		{
			get { return label; }
			set
			{
				label = value;
				this.RaisePropertyChanged(nameof(Label));
			}
		}

		private string lSSpecification;

		public string LSSpecification
		{
			get { return lSSpecification; }
			set
			{
				lSSpecification = value;
				this.RaisePropertyChanged(nameof(LSSpecification));
			}
		}

		private string lSBatchNo;

		public string LSBatchNo
		{
			get { return lSBatchNo; }
			set
			{
				lSBatchNo = value;
				this.RaisePropertyChanged(nameof(LSBatchNo));
			}
		}

		private double quantity;

		public double Quantity
		{
			get { return quantity; }
			set
			{
				quantity = value;
				this.RaisePropertyChanged(nameof(Quantity));
			}
		}

		private string historyBatchNo;

		public string HistoryBatchNo
		{
			get { return historyBatchNo; }
			set
			{
				historyBatchNo = value;
				this.RaisePropertyChanged(nameof(HistoryBatchNo));
			}
		}

		private string historyPackage;

		public string HistoryPackage
		{
			get { return historyPackage; }
			set
			{
				historyPackage = value;
				this.RaisePropertyChanged(nameof(HistoryPackage));
			}
		}

		private int historyBucketCount;

		public int HistoryBucketCount
		{
			get { return historyBucketCount; }
			set
			{
				historyBucketCount = value;
				this.RaisePropertyChanged(nameof(HistoryBucketCount));
			}
		}

		private double historySpecificationValue;

		public double HistorySpecificationValue
		{
			get { return historySpecificationValue; }
			set
			{
				historySpecificationValue = value;
				this.RaisePropertyChanged(nameof(HistorySpecificationValue));
			}
		}

		private DateTime historyModifyTime;

		public DateTime HistoryModifyTime
		{
			get { return historyModifyTime; }
			set
			{
				historyModifyTime = value;
				this.RaisePropertyChanged(nameof(HistoryModifyTime));
			}
		}

		private DateTime createTime;

		public DateTime CreateTime
		{
			get { return createTime; }
			set
			{
				createTime = value;
				this.RaisePropertyChanged(nameof(CreateTime));
			}
		}

		private DateTime? printTime;

		public DateTime? PrintTime
		{
			get { return printTime; }
			set
			{
				printTime = value;
				this.RaisePropertyChanged(nameof(PrintTime));
			}
		}

		private int? bucketId;

		public int? BucketId
		{
			get { return bucketId; }
			set
			{
				bucketId = value;
				this.RaisePropertyChanged(nameof(BucketId));
			}
		}

		private int? maxBucketPerBox;

		public int? MaxBucketPerBox
		{
			get { return maxBucketPerBox; }
			set
			{
				maxBucketPerBox = value;
				this.RaisePropertyChanged(nameof(MaxBucketPerBox));
			}
		}

		private string productionName;

		public string ProductionName
		{
			get { return productionName; }
			set
			{
				productionName = value;
				this.RaisePropertyChanged(nameof(ProductionName));
			}
		}

		private string twoDimensionCode;

		public string TwoDimensionCode
		{
			get { return twoDimensionCode; }
			set
			{
				twoDimensionCode = value;
				this.RaisePropertyChanged(nameof(TwoDimensionCode));
			}
		}

		private byte[] rowHashValue;

		public byte[] RowHashValue
		{
			get { return rowHashValue; }
			set
			{
				rowHashValue = value;
				this.RaisePropertyChanged(nameof(RowHashValue));
			}
		}

		private string orgCode;

		public string OrgCode
		{
			get { return orgCode; }
			set
			{
				orgCode = value;
				this.RaisePropertyChanged(nameof(OrgCode));
			}
		}


	}
}
