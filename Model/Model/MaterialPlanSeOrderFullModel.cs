using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialPlanSeOrderFullModel
    {
        public DateTime FDate { get; set; }

        public int FInterID { get; set; }

        public string FBillNo { get; set; }

        public int FCustId { get; set; }

        public string FCustName { get; set; }

        public int FEmpId { get; set; }

        public string FEmpName { get; set; }

        public int FDeptId { get; set; }

        public string FDeptName { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int FDetailId { get; set; }

        public int FitemId { get; set; }

        public string FName { get; set; }

        public int FEntryID { get; set; }

        public decimal FQty { get; set; }
    }
}
