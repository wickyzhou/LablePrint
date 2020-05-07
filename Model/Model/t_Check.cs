namespace Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class t_Check
    {
        public string Checker { get; set; }

        public string CustCode { get; set; }

        public string CustNo { get; set; }

        public string FBatchNo { get; set; }

        public string FBillNo { get; set; }

        public DateTime FDate { get; set; }

        public int FInterID { get; set; }

        public int FItemID { get; set; }

        public decimal FQty { get; set; }

        public string LabelName { get; set; }

        public string Time { get; set; }



        //2019.11.20 新增产品型号： 绑定到查询界面显示
        public string FItemType { get; set; }
    }
}

