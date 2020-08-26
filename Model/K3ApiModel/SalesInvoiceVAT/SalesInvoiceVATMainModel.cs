using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace K3ApiModel.SalesInvoiceVAT
{
    public class SalesInvoiceVATMainModel
    {
        public bool FCancellation { get; set; } = false;

        public string FCompactNo { get; set; } = "";

        public string FHeadSelfI0455 { get; set; } = "";

        public BaseNumberNameModelX FHeadSelfI0457 { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = ""};

        public string FHeadSelfI0459 { get; set; } = "";

        public string FHeadSelfI0461 { get; set; } = "";

        public string FHeadSelfI0462 { get; set; } = "";

        public int MyPropFEnterpriseIDerty { get; set; } = 0;

        public int FROB { get; set; } = -1; //蓝字 1   红字 -1

        public int FSendStatus { get; set; } = 0;

        public int FStatus { get; set; } = 0;

        public int? FVchInterID { get; set; } = null;

        public string FJSBillNo { get; set; } = "";

        public int FOrgBillInterID { get; set; } = 0;

        public int FImport { get; set; } = 0;

        public int FClassTypeID { get; set; } = 1000002;

        public int FTranType { get; set; } = 80;

        public string FPOOrdBillNo { get; set; } = "";

        public int FYtdIntRate { get; set; } = 0;

        public BaseNumberNameModelX FAcctID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public BaseNumberNameModelX FCussentAcctID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public BaseNumberNameModelX FCurrencyID { get; set; } = new BaseNumberNameModelX { FNumber = "RMB", FName = "人民币" };

        public BaseNumberNameModelX FSettleID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FNote { get; set; }

        public BaseNumberNameModelX FManagerID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public int? FPosterID { get; set; } = null;

        public BaseNumberNameModelX FHookerID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public string FYearPeriod { get; set; } = "";
  
        public BaseNumberNameModelX FBrID { get; set; } = new BaseNumberNameModelX { FNumber = "", FName = "" };

        public int FSysStatus { get; set; } = 2;

        public string FHeadSelfI0456 { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");

        public string FHeadSelfI0463 { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");

        public string Fdate { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");

        public BaseNumberNameModelX FItemClassID { get; set; } = new BaseNumberNameModelX { FNumber = "001", FName = "客户" };

        public BaseNumberNameModelX FSaleStyle { get; set; } = new BaseNumberNameModelX { FNumber = "FXF02", FName = "赊销" };

        public BaseNumberNameModelX FPayCondition { get; set; } = new BaseNumberNameModelX { FNumber = "22", FName = "账期为3个月（90天）" };

        public double FExchangeRate { get; set; } = 1.0;

        public string FSettleDate { get; set; } = DateTime.Now.AddMonths(2).Date.ToString("yyyy-MM-dd");

        public BaseNumberNameModelX FCustID { get; set; } = new BaseNumberNameModelX { FNumber = "HD.1006", FName = "通达(厦门)科技有限公司" };

        public BaseNumberNameModelX FExchangeRateType { get; set; } = new BaseNumberNameModelX { FNumber = "01", FName = "公司汇率" };

        public BaseNumberNameModelX FDeptID { get; set; } = new BaseNumberNameModelX { FNumber = "0022.01", FName = "销售部" };

        public BaseNumberNameModelX FEmpID { get; set; } = new BaseNumberNameModelX { FNumber = "231", FName = "王关连" };

        public BaseNumberNameModelX FBillerID { get; set; } = new BaseNumberNameModelX { FNumber = "Administrator", FName = "Administrator" };

        // 带出来的数据，后台Api不赋值的话没有数据：'XSFP20080006','XSFP20080005' ： FSelTranType 21    ； FHeadSelfI0458 A ； FHeadSelfI0464    账期为3个月（90天）
    }
}
