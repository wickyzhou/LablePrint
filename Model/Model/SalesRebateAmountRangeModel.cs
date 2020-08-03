using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateAmountRangeModel
    {
        public int Id { get; set; }

        public double AmountLower { get; set; }

        public double AmountUpper { get; set; }

        public double SalesRebatePctValue { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int IsValid { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid Guid { get; set; }

        public string IsValidName { get; set; }

    }
}
