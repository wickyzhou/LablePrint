using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel
{
    public class PurchaseRequisitionMainModel
    {
        public int FMRP { get; set; } = 0;

        public int FTranType { get; set; }

        public BaseNumberNameModel FHeadSelfP0131 { get; set; }

        public double FGeneratePurBudQty { get; set; }

        public BaseNumberNameModel FPlanCategory { get; set; }

        public BaseNumberNameModel FBizType { get; set; }

        public BaseNumberNameModel FDeptID { get; set; }

        public string Fdate { get; set; }

        public string Fnote { get; set; }

        public BaseNumberNameModel FCheckerID { get; set; }

        public string FCheckTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public BaseNumberNameModel FRequesterID { get; set; }

        public BaseNumberNameModel FBillerID { get; set; }

        public string FNumber { get; set; }

        public BaseNumberNameModel FSelTranType { get; set; }

        public string FBillNo { get; set; }

        public string FChecker { get; set; }


    }
}
