using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportVerificationModel
{
   public class ICStockBill29ImportVerificationModel
    {

        public bool IsPassed { get; set; }

        public int Seq { get; set; }

        public string MaterialFName { get; set; }
        public string MaterialFNumber { get; set; }


        public string StockFName  { get; set; }
        public string StockFNumber { get; set; }

        public double Quantity { get; set; }

        public string DeptFName { get; set; }
        public string DeptFNumber { get; set; }

        public string BillNo { get; set; }

        public DateTime CreateTime { get; set; }

        public string BatchNo { get; set; }

        public string StockPlaceFNumber { get; set; }
        public string StockPlaceFName { get; set; }

    }
}
