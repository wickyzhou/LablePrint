using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill1
{
    public class PurchaseReceiptMainModel
    {
        public int FTranType { get; set; }

        public int FInterID { get; set; }

        public DateTime FDate { get; set; }

        public string FBillNo { get; set; }

        public BaseNumberNameModelX FFManagerID { get; set; }

        public BaseNumberNameModelX FSManagerID { get; set; }

        public BaseNumberNameModelX FBillerID { get; set; }

        public BaseNumberNameModelX FDeptID { get; set; }

        public int FROB { get; set; } = 1;

        public BaseNumberNameModelX FSupplyID { get; set; }
    }
}
