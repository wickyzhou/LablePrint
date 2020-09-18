using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill1
{
    public class PurchaseReceiptSonModel
    {
        public int FInterID { get; set; }

        public int FEntryID { get; set; }

        public BaseNumberNameModelX FItemID { get; set; }

        public double FQtyMust { get; set; }

        public double FQty { get; set; }

        public double FPrice { get; set; }

        public string FBatchNo { get; set; }

        public double FAmount { get; set; }

        public double FEntrySelfA0162 { get; set; }

        public BaseNumberNameModelX FDCStockID { get; set; }

        public BaseNumberNameModelX FPlanMode { get; set; } = new BaseNumberNameModelX { FNumber = "MTS", FName = "MTS计划模式" };

        public double FPurchasePrice { get; set; }

        public double FPurchaseAmount { get; set; }

        public BaseNumberNameModelX FUnitID { get; set; } = new BaseNumberNameModelX { FName = "kg", FNumber = "kg" };

        public int FSourceInterId { get; set; }

        public int FSourceEntryID { get; set; }

        public int FSourceTranType { get; set; }

        public string FSourceBillNo { get; set; }

        public int FOrderInterID { get; set; }

        public int FOrderEntryID { get; set; }

        public string FOrderBillNo { get; set; }

    }
}
