using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportVerificationModel
{
    public class MaterialBomModel
    {
        public int Seq { get; set; }

        public string Number { get; set; }

        public string ItemName { get; set; }

        public int BomCount { get; set; }

        public int ItemId { get; set; }
    }
}
