using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class SalesRebateModel
    {
        public int Id { get; set; }

        public int ProductionModelId { get; set; }

        public int CaseId { get; set; }

        public int CustId { get; set; }

        public bool IsTaxAmount { get; set; }

        public int RebateClass { get; set; }

        public double RebatePct { get; set; }

        public double RebateAmout { get; set; }

        public bool IsAmountRange { get; set; }

        public string AmountRangeNote { get; set; }

    }
}
