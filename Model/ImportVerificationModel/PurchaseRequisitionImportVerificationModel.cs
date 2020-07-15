using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportVerificationModel
{
    public class PurchaseRequisitionImportVerificationModel
    {
        public int Seq { get; set; }

        public bool IsPassed { get; set; }

        public string FNumber { get; set; }

        public string FName { get; set; }

        public double Quantity { get; set; }

        public int SystemId { get; set; }
    }
}
