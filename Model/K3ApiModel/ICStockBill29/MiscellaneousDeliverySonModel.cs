using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill29
{
    public class MiscellaneousDeliverySonModel
    {
        public BaseNumberNameModel FItemID { get; set; }

        public BaseNumberNameModel FUnitID { get; set; }

        public BaseNumberNameModel FDCStockID1 { get; set; }

        public BaseNumberNameModel FDCSPID { get; set; }

        public BaseNumberNameModel FPlanMode { get; set; }

        public double FQty { get; set; }

        public double FScanQty { get; set; }

        public double Fauxqty { get; set; }

        public string FEntrySelfB0947 { get; set; }

        public string FEntrySelfB0948 { get; set; }

        public string FBatchNo { get; set; }

    }
}
