using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LabelPrintCurrencyModel: NotificationObject
    {

        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged(nameof(ID));
            }
        }

        private int labelPrintHistoryID;

        public int LabelPrintHistoryID
        {
            get { return labelPrintHistoryID; }
            set
            {
                labelPrintHistoryID = value;
                this.RaisePropertyChanged(nameof(LabelPrintHistoryID));
            }
        }

        private DateTime productionDate;

        public DateTime ProductionDate
        {
            get { return productionDate; }
            set
            {
                productionDate = value;
                this.RaisePropertyChanged(nameof(ProductionDate));
            }
        }


        private string workNo;

        public string WorkNo
        {
            get { return workNo; }
            set
            {
                workNo = value;
                this.RaisePropertyChanged(nameof(WorkNo));
            }
        }

        private string productionModel;

        public string ProductionModel
        {
            get { return productionModel; }
            set
            {
                productionModel = value;
                this.RaisePropertyChanged(nameof(ProductionModel));
            }
        }

        private string productionName;

        public string ProductionName
        {
            get { return productionName; }
            set
            {
                productionName = value;
                this.RaisePropertyChanged(nameof(ProductionName));
            }
        }


        private string orgID;

        public string OrgID
        {
            get { return orgID; }
            set
            {
                orgID = value;
                this.RaisePropertyChanged(nameof(OrgID));
            }
        }

        private string label;

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                this.RaisePropertyChanged(nameof(Label));
            }
        }

        private string orgCode;

        public string OrgCode
        {
            get { return orgCode; }
            set
            {
                orgCode = value;
                this.RaisePropertyChanged(nameof(OrgCode));
            }
        }

        private string batchNo;

        public string BatchNo
        {
            get { return batchNo; }
            set
            {
                batchNo = value;
                this.RaisePropertyChanged(nameof(BatchNo));
            }
        }

        private string roughWeight;

        public string RoughWeight
        {
            get { return roughWeight; }
            set
            {
                roughWeight = value;
                this.RaisePropertyChanged(nameof(RoughWeight));
            }
        }

        private string netWeight;

        public string NetWeight
        {
            get { return netWeight; }
            set
            {
                netWeight = value;
                this.RaisePropertyChanged(nameof(NetWeight));
            }
        }

        private string checkNo;

        public string CheckNo
        {
            get { return checkNo; }
            set
            {
                checkNo = value;
                this.RaisePropertyChanged(nameof(CheckNo));
            }
        }

        private int seqIncrement = 1;

        public int SeqIncrement
        {
            get { return seqIncrement; }
            set
            {
                seqIncrement = value;
                this.RaisePropertyChanged(nameof(SeqIncrement));
            }
        }

        //public int ID { get; set; } // 自增ID

        //public int LabelPrintHistoryID { get; set; } // 历史打印表ID

        //public DateTime ProductionDate { get; set; } // 生产日期

        //public string WorkNo { get; set; } // 工单号

        //public string ProductionModel { get; set; }  // 产品型号

        //public string ProductionName { get; set; }    // 产品名称

        //public string OrgID { get; set; }  // 客户编号

        //public string Label { get; set; }  // 标签型号

        //public string OrgCode { get; set; }    // 客户料号

        //public string BatchNo { get; set; }    // 生产批号

        //public string RoughWeight { get; set; }    // 毛重

        //public string NetWeight { get; set; }  // 净重

        //public string CheckNo { get; set; }    // 检验号


        private DateTime expirationDate;

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                this.RaisePropertyChanged(nameof(ExpirationDate));
            }
        }

        private string expirationMonth;

        public string ExpirationMonth
        {
            get { return expirationMonth; }
            set
            {
                expirationMonth = value;
                this.RaisePropertyChanged(nameof(ExpirationMonth));
            }
        }

        private int printCount;

        public int PrintCount
        {
            get { return printCount; }
            set
            {
                printCount = value;
                this.RaisePropertyChanged(nameof(PrintCount));
            }
        }

        private string twoDimensionCode;

        public string TwoDimensionCode
        {
            get { return twoDimensionCode; }
            set
            {
                twoDimensionCode = value;
                this.RaisePropertyChanged(nameof(TwoDimensionCode));
            }
        }

        private string specialRequest;

        public string SpecialRequest
        {
            get { return specialRequest; }
            set
            {
                specialRequest = value;
                this.RaisePropertyChanged(nameof(SpecialRequest));
            }
        }

        private string caseName;

        public string CaseName
        {
            get { return caseName; }
            set
            {
                caseName = value;
                this.RaisePropertyChanged(nameof(CaseName));
            }
        }


        private string printStatus;

        public string PrintStatus
        {
            get { return printStatus; }
            set
            {
                printStatus = value;
                this.RaisePropertyChanged(nameof(PrintStatus));
            }
        }

        //public DateTime ExpirationDate { get; set; } // 有效期至

        //public string ExpirationMonth { get; set; }  // 有效月份

        //public int PrintCount { get; set; }    // 打印张数

        //public string TwoDimensionCode { get; set; }   // 二维码

        //public string SpecialRequest { get; set; }   // 备注

        //public string CaseName { get; set; } // 案子名称

        //public string PrintStatus { get; set; } // 打印状态


        private int userID;

        public int UserID
        {
            get { return userID; }
            set
            {
                userID = value;
                this.RaisePropertyChanged(nameof(UserID));
            }
        }

        private DateTime createTime;

        public DateTime CreateTime
        {
            get { return createTime; }
            set
            {
                createTime = value;
                this.RaisePropertyChanged(nameof(CreateTime));
            }
        }

        private int productiveTaskListID;

        public int ProductiveTaskListID
        {
            get { return productiveTaskListID; }
            set
            {
                productiveTaskListID = value;
                this.RaisePropertyChanged(nameof(ProductiveTaskListID));
            }
        }

        private int batchCurrentSeq;

        public int BatchCurrentSeq
        {
            get { return batchCurrentSeq; }
            set
            {
                batchCurrentSeq = value;
                this.RaisePropertyChanged(nameof(BatchCurrentSeq));
            }
        }

        private string seq;

        public string Seq
        {
            get { return seq; }
            set
            {
                seq = value;
                this.RaisePropertyChanged(nameof(Seq));
            }
        }

        private string safeCode;

        public string SafeCode
        {
            get { return safeCode; }
            set
            {
                safeCode = value;
                this.RaisePropertyChanged(nameof(SafeCode));
            }
        }

        private string dangerousIngredient;

        public string DangerousIngredient
        {
            get { return dangerousIngredient; }
            set
            {
                dangerousIngredient = value;
                this.RaisePropertyChanged(nameof(DangerousIngredient));
            }
        }

        private string dangerousComment;

        public string DangerousComment
        {
            get { return dangerousComment; }
            set
            {
                dangerousComment = value;
                this.RaisePropertyChanged(nameof(DangerousComment));
            }
        }

        private int printOrder;// 打印顺序值

        public int PrintOrder
        {
            get { return printOrder; }
            set
            {
                printOrder = value;
                this.RaisePropertyChanged(nameof(PrintOrder));
            }
        }

        private int seq2678;

        public int Seq2678
        {
            get { return seq2678; }
            set
            {
                seq2678 = value;
                this.RaisePropertyChanged(nameof(Seq2678));
            }
        }

        private string twoDimensionCode1;

        public string TwoDimensionCode1
        {
            get { return twoDimensionCode1; }
            set
            {
                twoDimensionCode1 = value;
                this.RaisePropertyChanged(nameof(TwoDimensionCode1));
            }
        }

        private string twoDimensionCode2;

        public string TwoDimensionCode2
        {
            get { return twoDimensionCode2; }
            set
            {
                twoDimensionCode2 = value;
                this.RaisePropertyChanged(nameof(TwoDimensionCode2));
            }
        }

        private string twoDimensionCode3;

        public string TwoDimensionCode3
        {
            get { return twoDimensionCode3; }
            set
            {
                twoDimensionCode3 = value;
                this.RaisePropertyChanged(nameof(TwoDimensionCode3));
            }
        }

        private string twoDimensionCode4;

        public string TwoDimensionCode4
        {
            get { return twoDimensionCode4; }
            set
            {
                twoDimensionCode4 = value;
                this.RaisePropertyChanged(nameof(TwoDimensionCode4));
            }
        }

        private int sampleOilPrintCount;

        public int SampleOilPrintCount
        {
            get { return sampleOilPrintCount; }
            set
            {
                sampleOilPrintCount = value;
                this.RaisePropertyChanged(nameof(SampleOilPrintCount));
            }
        }

        private string sampleOilPrintArea;

        public string SampleOilPrintArea
        {
            get { return sampleOilPrintArea; }
            set
            {
                sampleOilPrintArea = value;
                this.RaisePropertyChanged(nameof(SampleOilPrintArea));
            }
        }

        private bool isSampleOilPrint;

        public bool IsSampleOilPrint
        {
            get { return isSampleOilPrint; }
            set
            {
                isSampleOilPrint = value;
                this.RaisePropertyChanged(nameof(IsSampleOilPrint));
            }
        }

        private string sampleOilPrintProductionName;

        public string SampleOilPrintProductionName
        {
            get { return sampleOilPrintProductionName; }
            set
            {
                sampleOilPrintProductionName = value;
                this.RaisePropertyChanged(nameof(SampleOilPrintProductionName));
            }
        }

        private double rowQuantity;

        public double RowQuantity
        {
            get { return rowQuantity; }
            set
            {
                rowQuantity = value;
                this.RaisePropertyChanged(nameof(RowQuantity));
            }
        }

        private string iCMOOrderBillNo;

        public string ICMOOrderBillNo
        {
            get { return iCMOOrderBillNo; }
            set
            {
                iCMOOrderBillNo = value;
                this.RaisePropertyChanged(nameof(ICMOOrderBillNo));
            }
        }

        private string shippingAddress;

        public string ShippingAddress
        {
            get { return shippingAddress; }
            set
            {
                shippingAddress = value;
                this.RaisePropertyChanged(nameof(ShippingAddress));
            }
        }

        private int fCustId;

        public int FCustId
        {
            get { return fCustId; }
            set
            {
                fCustId = value;
                this.RaisePropertyChanged(nameof(FCustId));
            }
        }


        private string gB;

        public string GB
        {
            get { return gB; }
            set
            {
                gB = value;
                this.RaisePropertyChanged(nameof(GB));
            }
        }

        private string qB;

        public string QB
        {
            get { return qB; }
            set
            {
                qB = value;
                this.RaisePropertyChanged(nameof(QB));
            }
        }

        private string gQB;

        public string GQB
        {
            get { return gQB; }
            set
            {
                gQB = value;
                this.RaisePropertyChanged(nameof(GQB));
            }
        }

        private string field01;

        public string Field01
        {
            get { return field01; }
            set
            {
                field01 = value;
                this.RaisePropertyChanged(nameof(Field01));
            }
        }

        private string field02;

        public string Field02
        {
            get { return field02; }
            set
            {
                field02 = value;
                this.RaisePropertyChanged(nameof(Field02));
            }
        }

        private string field03;

        public string Field03
        {
            get { return field03; }
            set
            {
                field03 = value;
                this.RaisePropertyChanged(nameof(Field03));
            }
        }


        private string field04;

        public string Field04
        {
            get { return field04; }
            set
            {
                field04 = value;
                this.RaisePropertyChanged(nameof(Field04));
            }
        }

        private string field05;

        public string Field05
        {
            get { return field05; }
            set
            {
                field05 = value;
                this.RaisePropertyChanged(nameof(Field05));
            }
        }

        private string field06;

        public string Field06
        {
            get { return field06; }
            set
            {
                field06 = value;
                this.RaisePropertyChanged(nameof(Field06));
            }
        }

        private string field07;

        public string Field07
        {
            get { return field07; }
            set
            {
                field07 = value;
                this.RaisePropertyChanged(nameof(Field07));
            }
        }

        private string field08;

        public string Field08
        {
            get { return field08; }
            set
            {
                field08 = value;
                this.RaisePropertyChanged(nameof(Field08));
            }
        }

        private string field09;

        public string Field09
        {
            get { return field09; }
            set
            {
                field09 = value;
                this.RaisePropertyChanged(nameof(Field09));
            }
        }

        private string field10;

        public string Field10
        {
            get { return field10; }
            set
            {
                field10 = value;
                this.RaisePropertyChanged(nameof(Field10));
            }
        }

        private bool noPrintVocName;

        public bool NoPrintVocName
        {
            get { return noPrintVocName; }
            set
            {
                noPrintVocName = value;
                this.RaisePropertyChanged(nameof(NoPrintVocName));
            }
        }


        private string vOCName;

        public string VOCName
        {
            get { return vOCName; }
            set
            {
                vOCName = value;
                this.RaisePropertyChanged(nameof(VOCName));
            }
        }

        //public int UserID { get; set; } // 用户ID

        //public DateTime CreateTime { get; set; } // 添加时间

        //public int ProductiveTaskListID { get; set; } // 生产任务清单ID

        //public int BatchCurrentSeq { get; set; } // 每个批次打印的最大序列号

        //public string Seq { get; set; } // 打印固定序列号

        //public string SafeCode { get; set; } // 安全编号,获取打印才使用，根据物料ID获取的数据

        //public string DangerousIngredient { get; set; } // 危险成分,获取打印才使用，根据物料ID获取的数据

        //public string DangerousComment { get; set; } // 危险性说明,获取打印才使用，根据物料ID获取的数据

    }
}
