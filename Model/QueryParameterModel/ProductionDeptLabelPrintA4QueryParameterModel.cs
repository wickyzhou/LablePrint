using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryParameterModel
{
    public class ProductionDeptLabelPrintA4QueryParameterModel:NotificationObject
    {
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

		private string orgId;

		public string OrgId
		{
			get { return orgId; }
			set
			{
				orgId = value;
				this.RaisePropertyChanged(nameof(OrgId));
			}
		}

		private string batchNo;

		public string BatchNo
		{
			get { return batchNo; }
			set
			{
				batchNo = value;
				this.RaisePropertyChanged(nameof(BatchNo));
			}
		}

		private string productionModel;

		public string ProductionModel
		{
			get { return productionModel; }
			set
			{
				productionModel = value;
				this.RaisePropertyChanged(nameof(ProductionModel));
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

		private string safeCode;

		public string SafeCode
		{
			get { return safeCode; }
			set
			{
				safeCode = value;
				this.RaisePropertyChanged(nameof(SafeCode));
			}
		}

	}
}
