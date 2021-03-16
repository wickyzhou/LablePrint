using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialPlanSeOrderEntryShowModel
    {

        public string FBillNo { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int FDetailId { get; set; }

        public int FitemId { get; set; }

        public string FName { get; set; }

        public int FEntryID { get; set; }

        public int FInterID { get; set; }

        public decimal FQty { get; set; }

    }
}
