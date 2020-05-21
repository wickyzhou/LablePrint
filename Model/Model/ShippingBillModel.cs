using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ShippingBillModel
    {
        public int Id { get; set; }

        public string BillNo { get; set; }

        public string SourceBillNo { get; set; }

        public DateTime CreateTime { get; set; }

        public float QuatityCosts { get; set; }

        public float TotalCosts { get; set; }

        public float TransportCosts { get; set; }

        public float DeliveryCosts { get; set; }

        public float PickUpCosts { get; set; }

        public float ValuationCosts { get; set; }

        public float PremiumCosts { get; set; }

        public float OtherCosts { get; set; }

        public string TransportCompany { get; set; }

    }
}
