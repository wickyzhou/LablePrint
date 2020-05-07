using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SellOrderListModel
    {
        public string FBillNo { get; set; }

        public int ID { get; set; }

        public int FSEOrderID { get; set; }

        public int FSEOrderEntryID { get; set; }

        public int FItemID { get; set; }

        public string OrgID { get; set; }

        public string Label { get; set; }

        public string OrgCode { get; set; }

        public string ProductName { get; set; }

        public int BucketID { get; set; }
        
        public string BucketName { get; set; }

        public int SpecificationID { get; set; }

        public decimal SpecificationValue { get; set; }

        public string OrgOrderNo { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
