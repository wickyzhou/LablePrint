using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BatchBomRequestDeliveryExportModel
    {

        public string BatchTypeName { get; set; }

        public string MaterialName { get; set; }

        public double QtyRequest { get; set; }

        public double QtyWorkshopInventory { get; set; }

        public string FModel { get; set; }

        public string FBatchNoAndActualQty { get; set; }

    }
}
