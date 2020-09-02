using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Model
{
    public class LabelPrintHistoryModel : NotificationObject
    {

        public Byte[] RowHashValue { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    isChecked = value;
                    this.RaisePropertyChanged(nameof(IsChecked));
                }
            }
        }

        public int ID { get; set; } //自增ID

        public int ProductiveTaskListID { get; set; } //生产任务清单ID

        public string WorkNo { get; set; } //工单号

        public string BatchNo { get; set; }    //生产批号

        public string ProductionModel { get; set; }  //产品型号

        public string ProductionName { get; set; }    //产品名称
            
        public DateTime? ProductionDate { get; set; } //生产日期

        public DateTime? ExpirationDate { get; set; } //有效期至

        public string ExpirationMonth { get; set; }  //有效月份

        public string OrgID { get; set; }  //客户编号

        public string Label { get; set; }  //标签型号

        public string OrgCode { get; set; }    //客户料号

        public string OrgBillNo { get; set; }    //客户订单号

        public string Package { get; set; }   //包装桶

        public string RoughWeight { get; set; }    //毛重

        public string NetWeight { get; set; }  //净重

        public string CheckNo { get; set; }    //检验号

        public string SpecialRequest { get; set; }   //备注

        public string CaseName { get; set; } //案子名称

        public DateTime? ModifyTime { get; set; }    //修改日期

        public string TwoDimensionCode { get; set; }   //二维码

        //public int PrintCount { get; set; }    //打印张数
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



        public int BucketCount { get; set; } // 桶数

        //public int Selected { get; set; } //选中
        private int selected;

        public int Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.RaisePropertyChanged(nameof(Selected));
            }
        }



        //public string Seq { get; set; } // 重打序号
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


        public int BatchTotal { get; set; } // 批次打印总数

        public int BatchReprintCount { get; set; } // 批次重打数量

        public int BatchCurrentSeq { get; set; } // 批次当前序号

        public int WorkTotal { get; set; } // 工单打印总数

        public int WorkPrintCount { get; set; } // 工单打印次数

        public int WorkReprintCount { get; set; } // 工单重打数量

        public DateTime? LastPrintTime { get; set; } //最后打印时间

        public string SafeCode { get; set; } // 安全编号

        public decimal SpecificationValue { get; set; } // 规格用来做查询条件

        public decimal SpecificationValueBegin { get; set; } // 获取方案明细值规格开始值

        public decimal SpecificationValueEnd { get; set; } // 获取方案明细值规格结束值


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

    }
}
