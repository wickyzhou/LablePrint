using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialPlanInventoryModel
    {
        public int GroupId { get; set; }

        public int MaterialId { get; set; }

        public string MaterialNumber { get; set; }

        public string MaterialName { get; set; }

        public string MaterialSpec { get; set; }

        public double RequiredQty { get; set; }

        public double InventoryQty { get; set; }

        public double UnPickedQty { get; set; }

        public double LockedQty { get; set; }

        public double RemainQty { get; set; }

        public string BillsAndQty { get; set; }

        public bool IsVisible { get; set; }

        public bool IsWarning { get; set; }


    }
}
