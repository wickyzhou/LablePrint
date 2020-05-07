using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CrystalPrintConfigModel
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string TypeDesciption { get; set; }

        public string HostName { get; set; }

        public string Orientation { get; set; }

        public string PrinterName { get; set; }

        public int MarginLeft { get; set; }

        public int MarginTop { get; set; }

        public int MarginRight { get; set; }

        public int MarginBottom { get; set; }

        public string PaperName { get; set; }
    }
}
