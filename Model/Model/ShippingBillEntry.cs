using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ShippingBillEntry
    {
        public int Id { get; set; }

        public int MainId { get; set; }

        public string CaseName { get; set; }

        public float Quatity { get; set; }

        public float Costs { get; set; }

    }
}
