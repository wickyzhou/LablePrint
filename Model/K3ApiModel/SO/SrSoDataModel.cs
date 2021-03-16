using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.SO
{
    public class SrSoDataModel
    {
        public int FBucketCount { get; set; }

        public string FItemNumber { get; set; }

        public string FBillNo { get; set; }

        public string FSEOrderEntryID { get; set; }

        public DateTime FProductionDate { get; set; }

        public string FOrgBillNo { get; set; }

        public string FProductionName { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string FLabel { get; set; }

        public string FOrgCode { get; set; }

        public decimal OrderQty { get; set; }

        public string SpecNumber { get; set; }

        public string SpecName { get; set; }

        public int OrgId { get; set; }

        public string OrgNumber { get; set; }

        public string OrgName { get; set; }

        public int MaterialId { get; set; }

        public string MaterialNumber { get; set; }

        public string MaterialName { get; set; }

        public decimal Price { get; set; }

        public string EmpNumber { get; set; }

        public string EmpName { get; set; }

        public string OrderEntryNote { get; set; }

        public string FBatchNo { get; set; }

        public int FCESS { get; set; }

        public decimal Fauxprice { get; set; }

        public decimal Famount { get; set; }

        public decimal FAuxTaxPrice { get; set; }

        public decimal FAuxPriceDiscount { get; set; }

        public decimal FTaxAmt { get; set; }

        public decimal FAllAmount { get; set; }

    }
}
