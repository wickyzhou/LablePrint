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

        public int RebateClass { get; set; }

        public int TaxAmountType { get; set; }

        public int RebatePctType { get; set; }

        public double RebatePct { get; set; }

        public double RebateAmout { get; set; }

        public string AmountRangeNote { get; set; }
    }
}
