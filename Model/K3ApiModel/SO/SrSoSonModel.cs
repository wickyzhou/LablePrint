using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.SO
{
    public class SrSoSonModel
    {
        public string FOrderBillNo { get; set; }

        public BaseNumberNameModelX FItemID { get; set; }

        public string FItemName { get; set; }

        public string FBaseUnit { get; set; }

        public BaseNumberNameModelX FUnitID { get; set; }

        public BaseNumberNameModelX FPlanMode { get; set; }

        public string FEntrySelfS0179 { get; set; }

        public string FEntrySelfS0178 { get; set; }

        public decimal FEntrySelfS0177 { get; set; }

        public BaseNumberNameModelX FEntrySelfS0176 { get; set; }

        //public BaseNumberNameModelX FEntrySelfS0175 { get; set; }

        //public BaseNumberNameModelX FEntrySelfS0174 { get; set; }

        public string FAdviceConsignDate { get; set; }

        public string FDate1 { get; set; }

        public decimal FCESS { get; set; }

        public decimal FQty { get; set; }

        public decimal Fauxqty { get; set; }

        public decimal Fauxprice { get; set; }

        public decimal Famount { get; set; }

        public decimal FAuxTaxPrice { get; set; }

        public decimal FAuxPriceDiscount { get; set; }

        public decimal FTaxAmt { get; set; }

        public decimal FAllAmount { get; set; }

        public string Fnote { get; set; }

        public string FMapName { get; set; }

        public string FEntrySelfS0180 { get; set; }

        public string FEntrySelfS0181 { get; set; }
    }
}
