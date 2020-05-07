using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace Model
{
    public class PageSizeModel
    {
        public int Height { get; set; }

        public int Width { get; set; }

        public int RawKind { get; set; }

        public PaperKind Kind { get; set; }

        public string PaperName { get; set; }
    }
}
