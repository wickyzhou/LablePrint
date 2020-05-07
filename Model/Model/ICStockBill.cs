namespace Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ICStockBill
    {
        public decimal Famount { get; set; }

        public decimal Fauxprice { get; set; }

        public decimal Fauxqty { get; set; }

        public string FBillNo { get; set; }

        public DateTime FDate { get; set; }

        public int FEntryID { get; set; }

        public int FInterID { get; set; }

        public int FItemID { get; set; }

        public string FName { get; set; }

        public string FNumber { get; set; }
    }
}

