using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.Request
{
    public class K3ApiCheckBillDataRequestMode
    {
        public string FBillNo { get; set; }

        public string FChecker { get; set; }

        public int FCheckDirection { get; set; }

        public string FDealComment { get; set; }
    }
}
