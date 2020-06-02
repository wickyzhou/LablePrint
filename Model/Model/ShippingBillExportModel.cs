using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class ShippingBillExportModel
    {

        public int EntryId { get; set; }

        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string LogisticsTypeName { get; set; }
        public string LogisticsCompanyName { get; set; }
        public string LogisticsBillNo { get; set; }
        public string Demander { get; set; }
        public string Note { get; set; }
        public string GoodsTypeName { get; set; }
        public string CaseName { get; set; }
        public string BrandName { get; set; }
        public string DeptName { get; set; }
        public string CustName { get; set; }

        public float YunShuFei { get; set; }
        public float YouFei { get; set; }
        public float GuoLuFei { get; set; }
        public float ChaiLvFei { get; set; }
        public float WeiXiuFei { get; set; }
        public float GuanShuiFei { get; set; }
        public float TiHuoFei { get; set; }
        public float WeiXianPinFei { get; set; }
        public float QingGuanFei { get; set; }
        public float BaoXianFei { get; set; }
        public float PaiSongFei { get; set; }
        public float OtherCosts { get; set; }
        public float Quatity { get; set; }
        public float ApportionedAmount { get; set; }

        public float TotalQuantity { get; set; }

        public float TotalAmount { get; set; }

    }
}
