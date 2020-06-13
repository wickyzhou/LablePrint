using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OilSampleFlowPrintLogModel
    {
        public int Id { get; set; }
        public double FormmainId { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public string TypeDesc { get; set; }
        public int PrintCount { get; set; }
        public float PrintWeight { get; set; }
        public DateTime CreateTime { get; set; }
        public string BatchNo { get; set; }

    }
}
