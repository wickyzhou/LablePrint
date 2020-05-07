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
