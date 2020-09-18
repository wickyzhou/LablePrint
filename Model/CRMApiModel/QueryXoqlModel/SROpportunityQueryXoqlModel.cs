using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.QueryXoqlModel
{
    public class SROpportunityQueryXoqlModel
    {
        public int Id { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string XiangMuJingLi { get; set; }

        public string ShiChangZhiChi { get; set; }

        public string SeCaiZhiChi { get; set; }

        public string YeWuZhiChi { get; set; }

        public string JiFuZhiChi { get; set; }

        public string ChanPinJingLi { get; set; }

        public string JiShuZhiChi { get; set; }

        public string PinZhiZhiChi { get; set; }

        public string HouDuanZhiChi { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
