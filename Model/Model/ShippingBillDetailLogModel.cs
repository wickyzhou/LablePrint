using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ShippingBillDetailLogModel
    {
        public int MainId { get; set; }
        public int InterId { get; set; }
        public int EntryId { get; set; }
        public int CustId { get; set; }
        public int DeptId { get; set; }
        public int BrandId { get; set; }
        public int CaseId { get; set; }
        public int ItemId { get; set; }
        public string ShippingBillNo { get; set; }
        public string ConsignmentBillNo { get; set; }
        public string CustName { get; set; }
        public string DeptName { get; set; }
        public string BrandName { get; set; }
        public string CaseName { get; set; }
        public string ItemName { get; set; }
        public float CurrencyQuantity { get; set; }
        public string SourceBillNo { get; set; }
        public int SourceInterId { get; set; }
        public int SourceEntryId { get; set; }
        public float SourceEntryQty { get; set; }
    }
}
