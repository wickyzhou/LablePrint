using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Response
{
   public class K3ApiInsertDataMultiResponseModel:NotificationObject
    {
		private string billInterID;

		public string BillInterID
		{
			get { return billInterID; }
			set
			{
				billInterID = value;
				this.RaisePropertyChanged(nameof(BillInterID));
			}
		}

		private string billNo;

		public string BillNo
		{
			get { return billNo; }
			set
			{
				billNo = value;
				this.RaisePropertyChanged(nameof(BillNo));
			}
		}
	}
}
