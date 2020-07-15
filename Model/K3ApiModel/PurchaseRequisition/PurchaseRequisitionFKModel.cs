using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.PurchaseRequisition
{
   public class PurchaseRequisitionFKModel
    {
        public int FAuxClassID { get; set; }

        public int FUnitID { get; set; }

        public string F_102 { get; set; }

        public string FNumber { get; set; }

        public string FModel { get; set; }

        public string FName { get; set; }

        public int FFixLeadTime { get; set; }

        public double FSecCoefficient { get; set; }

        public int FSecUnitID { get; set; }

    }
}
