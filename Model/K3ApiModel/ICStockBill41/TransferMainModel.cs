using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3ApiModel.ICStockBill41
{
    public class TransferMainModel
    {
        public int FInterID { get; set; }

        public BaseNumberNameModelX FFManagerID { get; set; }

        public BaseNumberNameModelX FSManagerID { get; set; }

        public BaseNumberNameModelX FBillerID { get; set; }

        public BaseNumberNameModelX FCheckerID { get; set; }

        public BaseNumberNameModelX FRefType { get; set; }

        public int FClassTypeID { get; set; }

        public string FDate { get; set; }

    }
}
