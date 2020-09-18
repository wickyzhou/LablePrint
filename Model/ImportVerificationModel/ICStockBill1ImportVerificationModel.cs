using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportVerificationModel
{
    public class ICStockBill1ImportVerificationModel
    {
        public bool IsPassed { get; set; }

        public int Seq { get; set; }

        public int MaterialId { get; set; }

        public string MaterialFNumber { get; set; }

        public string MaterialFName { get; set; }

        public string StockId { get; set; }

        public string StockFNumber { get; set; }

        public string StockFName { get; set; }
        
        public double Quantity { get; set; }

        public DateTime CreateTime { get; set; }

        public string BatchNo { get; set; }

        public double QtyInserted { get; set; }

        public double QtyUnInsert { get; set; }

    }
}
