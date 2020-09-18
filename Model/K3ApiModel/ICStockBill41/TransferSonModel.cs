using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill41
{
    public class TransferSonModel
    {
        public int FInterID { get; set; }
        public int FEntryID { get; set; }
    
        public string FBatchNo { get; set; }
        
        public BaseNumberNameModelX FItemID { get; set; }
        public BaseNumberNameModelX FChkPassItem { get; set; }
        public BaseNumberNameModelX FPlanMode { get; set; }
        public BaseNumberNameModelX FUnitID { get; set; }
        public BaseNumberNameModelX FSCStockID1 { get; set; }
        public BaseNumberNameModelX FDCStockID1 { get; set; }
       

        public double FQty { get; set; }
        public double FAuxQty { get; set; }
        //public double FPlanPrice { get; set; }
        //public double FAuxPlanPrice { get; set; }
        //public double FPlanAmount { get; set; }

    }
}
