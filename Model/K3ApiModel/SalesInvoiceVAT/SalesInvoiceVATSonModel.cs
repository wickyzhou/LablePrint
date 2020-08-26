using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.SalesInvoiceVAT
{
    public class SalesInvoiceVATSonModel
    {
        public string FEntrySelfI0464 { get; set; } = "";

        public string FEntrySelfI0465 { get; set; } = "折扣";

        public string FEntrySelfI0466 { get; set; } = "0"; //客户物料代码

        public string FEntrySelfI0467 { get; set; } = "0";

        public string FEntrySelfI0468 { get; set; } = "折扣";

        public string FEntrySelfI0470 { get; set; } = "";

        public BaseNumberNameModelX FEntrySelfI0471 { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = ""};

        public string FEntrySelfI0474 { get; set; } = "";

        public string FEntrySelfI0475 { get; set; } = "";

        public string FEntrySelfI0476 { get; set; } = "";

        public BaseNumberNameModelX FEntrySelfI0477 { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FEntrySelfI0478 { get; set; } = "";

        public BaseNumberNameModelX FEntrySelfI0487 { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FEntrySelfI0488 { get; set; } = "";

        public string FEntrySelfI0489 { get; set; } = "";

        public string FEntrySelfI0490 { get; set; } = "";

        public string FEntrySelfI0491 { get; set; } = "";

        public string FEntrySelfI0492 { get; set; } = "";

        public int FOutSourceEntryID { get; set; } = 0;

        public int FOutSourceInterID { get; set; } = 0;

        public int FOutSourceTranType { get; set; } = 0;

        public BaseNumberNameModelX FMapNumber { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FMapName { get; set; } = "";

        public BaseNumberNameModelX FItemID { get; set; } = new BaseNumberNameModelX { FNumber = "折扣", FName = "折扣" };

        public string FItemName { get; set; } = "折扣";

        public string FItemModel { get; set; } = "";

        public BaseNumberNameModelX FAuxPropID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FBaseUnit { get; set; } = "kg";

        public int FQty { get; set; } = 0;

        public BaseNumberNameModelX FUnitID { get; set; } = new BaseNumberNameModelX { FNumber = "kg", FName = "kg" };

        public int Fauxqty { get; set; } = 0;

        public int? FSecUnitID { get; set; } = null;

        public int FSecCoefficient { get; set; } = 0;

        public int FSecQty { get; set; } = 0;

        public int FDiscountRate { get; set; } = 0;

        public int FAmtDiscount { get; set; } = 0;

        public int FStdAmtDiscount { get; set; } = 0;

        //public DateTime? FKFDate { get; set; } = null;

        public int FKFperiod { get; set; } = 0;

        //public DateTime? FPeriodDate { get; set; } = null;

        public string FBatchNo { get; set; } = "";

        public string FNote1 { get; set; } = "";

        public int FStdAllAmount { get; set; } = 0;

        public int FOrgBillEntryID { get; set; } = 0;

        public int FOrderPrice { get; set; } = 0;

        public int FAuxOrderPrice { get; set; } = 0;

        public int FClassID_SRC { get; set; } = 0;

        public int FEntryID_SRC { get; set; } = 0;

        public int FSourceTranType { get; set; } = 0;

        public int FSourceInterId { get; set; } = 0;

        public int FSourceEntryID { get; set; } = 0;

        public int FClientEntryID { get; set; } = 0;

        public int FContractInterID { get; set; } = 0;

        public int FContractEntryID { get; set; } = 0;

        public int FOrderInterID { get; set; } = 0;

        public int FOrderEntryID { get; set; } = 0;

        public int FAllHookQTY { get; set; } = 0;

        public int FStdAllHookAmount { get; set; } = 0;

        public int FCurrentHookQTY { get; set; } = 0;

        public int FStdCurrentHookAmount { get; set; } = 0;

        public string FSourceBillNo { get; set; } = "";

        public string FClientOrderNo { get; set; } = "";

        public string FContractBillNo { get; set; } = "";

        public string FOrderBillNo { get; set; } = "";

        public string FMTONo { get; set; } = "";

        public int? FAuxPropCls { get; set; } = null;

        public BaseNumberNameModelX FPlanMode { get; set; } = new BaseNumberNameModelX { FNumber = "MTS", FName = "MTS计划模式" };

        public double Fauxprice { get; set; } = 100;

        public double Famount { get; set; } = 100;

        public double FStdAmount { get; set; } = 100;

        public double FTaxRate { get; set; } = 13;

        public double FTaxAmount { get; set; } = 13;

        public double FStdTaxAmount { get; set; } = 13;

        public double FAllAmount { get; set; } = 113;

        public double FAuxTaxPrice { get; set; } = 113;

        public double FAuxPriceDiscount { get; set; } = 113;

        public double FAmountincludetax { get; set; } = 113;

        public double FStdAmountincludetax { get; set; } = 113;

        public double FRemainAmount { get; set; } = 113;

        public double FRemainAmountFor { get; set; } = 113;
    }
}
