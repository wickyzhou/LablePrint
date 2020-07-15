using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace K3ApiModel.PurchaseRequisition
{
    public class PurchaseRequisitionSonModel
    {
        public double FQty { get; set; }

        public double Fauxqty { get; set; }

        public string FSecUnitID { get; set; }

        public double FSecCoefficient { get; set; } = 0;

        public double FSecQty { get; set; } = 0;

        public string FAPurchTime { get; set; } // = DateTime.Now.Date.ToString();

        public int FLeadTime { get; set; } // = 1;提前期

        public string FFetchTime { get; set; } // = DateTime.Now.Date.ToString();

        public int FPlanOrderInterID { get; set; }

        public string FSourceBillNo { get; set; }

        public int FSourceTranType { get; set; }

        public int FSourceInterId { get; set; }

        public int FSourceEntryID { get; set; }

        public string FAuxPropCls { get; set; }

        public int FMrpLockFlag { get; set; }

        public double FOrderQty { get; set; }

        public string FMTONo { get; set; }

        public int FIsInquiry { get; set; }

        public int FDetailID2 { get; set; }

        public int FInterID2 { get; set; }

        public int FEntryID2 { get; set; }

        public string FEntrySelfP0131 { get; set; }

        public BaseNumberNameModel FItemID { get; set; }  //

        public BaseNumberNameModel FAuxPropID { get; set; }      //BaseNumberNameModel

        public BaseNumberNameModel FUnitID { get; set; } //BaseNumberNameModel

        public BaseNumberNameModel FBomInterID { get; set; } //BaseNumberNameModel

        public string Fuse { get; set; }

        public BaseNumberNameModel FSupplyID { get; set; } //BaseNumberNameModel

        public BaseNumberNameModel FPlanMode { get; set; } //BaseNumberNameModel

        public string FNumber { get; set; }

    }
}
