﻿using System;
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

        public List<string> YeWuZhiChi { get; set; }

        public string JiFuZhiChi { get; set; }

        public List<string> ChanPinJingLi { get; set; }

        public List<string> JiShuZhiChi { get; set; }

        public string PinZhiZhiChi { get; set; }

        public string HouDuanZhiChi { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

    }
}
