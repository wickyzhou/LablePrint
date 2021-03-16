using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.SO
{
    public class SrSoMainModel
    {
        public string Fdate { get; set; }

        public BaseNumberNameModelX FCustID { get; set; }

        public BaseNumberNameModelX FAreaPS { get; set; }

        public int FTranType { get; set; }

        public decimal FExchangeRate { get; set; }

        public BaseNumberNameModelX FCurrencyID { get; set; }

        public BaseNumberNameModelX FSelTranType { get; set; }

        public BaseNumberNameModelX FExchangeRateType { get; set; }

        public BaseNumberNameModelX FPlanCategory { get; set; }

        public BaseNumberNameModelX FDeptID { get; set; }

        public BaseNumberNameModelX FEmpID { get; set; }

        public BaseNumberNameModelX FBillerID { get; set; }

    }
}
