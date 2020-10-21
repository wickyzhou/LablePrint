using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ItemProfitAccountingModel
    {
        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public double? Profit { get; set; }

        public int? RecentMonth { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}
