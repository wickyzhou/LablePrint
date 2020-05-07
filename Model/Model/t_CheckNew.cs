namespace Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class t_CheckNew
    {
        public string Checker { get; set; }
        public string SH { get; set; } //审核
        /**unique <FBatchNo,FXCS>*/
        public string FBatchNo { get; set; }
        /**FXCS 记录检测所对应的新产品阶段  IQC，制成中，退货，仓库*/
        public short FXCS { get; set; }
        public string strFXCS { get { return t_CheckNew.GetFXCSString(FXCS); } }
        public string FBillNo { get; set; }

        public DateTime FDate { get; set; }

        public int FInterID { get; set; } //key: FInterID
        /**产品代码*/
        public int FItemID { get; set; }

        public decimal FQty { get; set; }
        /**保质期*/
        public string Time { get; set; }
        public static string DateFmt { get { return "yyyy-MM-dd"; } }
        public static DateTime NullDateTime { get { return Convert.ToDateTime("1970-01-01"); } }
        public static  DateTime String2DateTime(string txt)
        {
            try
            {
                return Convert.ToDateTime(txt);
            }
            catch (Exception)
            {
                return t_CheckNew.NullDateTime;
            }
        }
        public static string DateTime2String(DateTime dt)
        {
            if(dt==t_CheckNew.NullDateTime) { 
                return ""; 
            }
            else {
                return dt.ToString(t_CheckNew.DateFmt);
            }
        }
        public static string[] FXCSStrings { get { return new string[]{"IQC","制成中","退货","仓库"};} }
        public static string GetFXCSString(short fxcs)
        {
            if (fxcs >= 0 && fxcs < 4)
                return t_CheckNew.FXCSStrings[fxcs];
            else return "";
        }
        

    }
    public class t_CheckEntryNew 
    {
        public bool ISOK  {get;set;}

        public string CheckMethodName { get; set; }

        public string CheckName { get; set; }

        public string CheckResults { get; set; }

        public string CheckStanardName { get; set; }

        public int FEntryID { get; set; }

        public int FInterID { get; set; } //key: <FInterID,FEntryID>
        public bool IsSel { get; set; } //仅用来在客户报告中显示是否显示，不做实际的存储

    }
    public class t_UnConform
    {
        public int FInterID{set;get;} //key and ref 2 t_CheckNew
       // public short FXCS { set; get; } //发现场所 0 1 2 3
        public string ZRBM { set; get; } //责任部门
        public string ZRBMFZR { set; get; } //责任部门负责人
        public DateTime ZRBMRQ { set; get; }//责任部门日期
        public string Reason{set;get;} //原因分析	REASON	nvarchar(255)	255		FALSE	FALSE	FALSE
        public short CZCS{set;get;}//处置措施 0 1 2	CZCS	nvarchar(255)	255		FALSE	FALSE	FALSE
        public string GZFA{set;get;}//改造方案	GZFA	nvarchar(255)	255		FALSE	FALSE	FALSE
        public string CZJSGCS{set;get;}//处置技术工程师	CZJSGCS	nvarchar(16)	16		FALSE	FALSE	FALSE
        public DateTime CZRQ{set;get;}//处置日期	CZRQ	datetime			FALSE	FALSE	FALSE
        public string BMFZR {set;get;}//品质部门负责人	BMFZR	nvarchar(16)	16		FALSE	FALSE	FALSE
        public DateTime BMFZRRQ;// 部门负责人日期
        //接收及确认
        public string JSTBQR;//接收贴标签人	JSTBQR	nvarchar(16)	16		FALSE	FALSE	FALSE
        public DateTime JSRQ {set;get;} //接收日期	JSRQ	datetime			FALSE	FALSE	FALSE
        public string JSQRR{set;get;} //接收确认人	JSQRR	nvarchar(16)	16		FALSE	FALSE	FALSE
        public DateTime JSQRRQ{set;get;} //接收确认日期	JSQRRQ	datetime			FALSE	FALSE	FALSE
    }
    public class t_CheckForCust
    {
        public int FInterID { get; set; } //key: <FInterID,CustCode>
        public string CustCode {get;set;} //客户代码	CustCode	nvarchar(50)	50		TRUE	FALSE	TRUE
        public string LabelName {get;set;} //客户标签型号	LabelName	nvarchar(50)	50		FALSE	FALSE	FALSE
        public string CustNo {get;set;} //客户料号	CustNo	nvarchar(50)	50		FALSE	FALSE	FALSE
        public string Mem { get; set; } //备注 MEM
        public decimal FQty {get;set;} //FQty	FQty	float			FALSE	FALSE	FALSE
        public string PRINTENTRIES {get;set;}//打印项集	PRINTENTRIES	nvarchar(255)	255		FALSE	FALSE	FALSE
    }
}

