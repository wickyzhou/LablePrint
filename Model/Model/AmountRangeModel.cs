using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class AmountRangeModel
    {
        public int Id { get; set; }

        public int SalesRebateId { get; set; }

        public double TaxAmountLower { get; set; }

        public double TaxAmountUpper { get; set; }

        public double RebatePct { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
