using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill29
{
    public class MiscellaneousDeliverySonModel
    {
        public BaseNumberNameModelX FItemID { get; set; }

        public BaseNumberNameModelX FUnitID { get; set; }

        public BaseNumberNameModelX FDCStockID1 { get; set; }

        public BaseNumberNameModelX FDCSPID { get; set; }

        public BaseNumberNameModelX FPlanMode { get; set; }

        public double FQty { get; set; }

        public double FScanQty { get; set; }

        public double Fauxqty { get; set; }

        public string FEntrySelfB0947 { get; set; }

        public string FEntrySelfB0948 { get; set; }

        public string FBatchNo { get; set; }

        public string BrandName { get; set; }

        public string CaseName { get; set; }

        public string FNote { get; set; }
    }
}
