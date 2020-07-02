using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialDemandModel
    {
        public int Id { get; set; }

        public string MaterialName { get; set; }

        public float QuantityDemand { get; set; }

        public float InventoryQuantity { get; set; }

        public DateTime InventoryTime { get; set; }
        
    }
}
