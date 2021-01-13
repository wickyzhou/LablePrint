using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace Model
{
    public class LabelPrintHistoryModel : AGenericFieldModel
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

        public string OrgID { get; set; }  //客户编号

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

        private DateTime? productionDate;

        public DateTime? ProductionDate
        {
            get { return productionDate; }
            set
            {
                productionDate = value;
                this.RaisePropertyChanged(nameof(ProductionDate));
            }
        }

        private DateTime? expirationDate;

        public DateTime? ExpirationDate
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


        private string orgBillNo;

        public string OrgBillNo
        {
            get { return orgBillNo; }
            set
            {
                orgBillNo = value;
                this.RaisePropertyChanged(nameof(OrgBillNo));
            }
        }


        private string package;

        public string Package
        {
            get { return package; }
            set
            {
                package = value;
                this.RaisePropertyChanged(nameof(Package));
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


        private DateTime? modifyTime;

        public DateTime? ModifyTime
        {
            get { return modifyTime; }
            set
            {
                modifyTime = value;
                this.RaisePropertyChanged(nameof(ModifyTime));
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

        private int printedCount;

        public int PrintedCount
        {
            get { return printedCount; }
            set
            {
                printedCount = value;
                this.RaisePropertyChanged(nameof(PrintedCount));
            }
        }


        public int BucketCount { get; set; } 

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

        private int batchTotal;

        public int BatchTotal
        {
            get { return batchTotal; }
            set
            {
                batchTotal = value;
                this.RaisePropertyChanged(nameof(BatchTotal));
            }
        }


        private int batchReprintCount;

        public int BatchReprintCount
        {
            get { return batchReprintCount; }
            set
            {
                batchReprintCount = value;
                this.RaisePropertyChanged(nameof(BatchReprintCount));
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


        private int workTotal;

        public int WorkTotal
        {
            get { return workTotal; }
            set
            {
                workTotal = value;
                this.RaisePropertyChanged(nameof(WorkTotal));
            }
        }


        private int workPrintCount;

        public int WorkPrintCount
        {
            get { return workPrintCount; }
            set
            {
                workPrintCount = value;
                this.RaisePropertyChanged(nameof(WorkPrintCount));
            }
        }


        private int workReprintCount;

        public int WorkReprintCount
        {
            get { return workReprintCount; }
            set
            {
                workReprintCount = value;
                this.RaisePropertyChanged(nameof(WorkReprintCount));
            }
        }


        private DateTime? lastPrintTime;

        public DateTime? LastPrintTime
        {
            get { return lastPrintTime; }
            set
            {
                lastPrintTime = value;
                this.RaisePropertyChanged(nameof(LastPrintTime));
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


        private decimal specificationValue;

        public decimal SpecificationValue
        {
            get { return specificationValue; }
            set
            {
                specificationValue = value;
                this.RaisePropertyChanged(nameof(SpecificationValue));
            }
        }


        private decimal specificationValueBegin;

        public decimal SpecificationValueBegin
        {
            get { return specificationValueBegin; }
            set
            {
                specificationValueBegin = value;
                this.RaisePropertyChanged(nameof(SpecificationValueBegin));
            }
        }


        private decimal specificationValueEnd;

        public decimal SpecificationValueEnd
        {
            get { return specificationValueEnd; }
            set
            {
                specificationValueEnd = value;
                this.RaisePropertyChanged(nameof(SpecificationValueEnd));
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

        private bool isPassed;

        public bool IsPassed
        {
            get { return isPassed; }
            set
            {
                isPassed = value;
                this.RaisePropertyChanged(nameof(IsPassed));
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


        private DateTime? sampleOilPrintTime;

        public DateTime? SampleOilPrintTime
        {
            get { return sampleOilPrintTime; }
            set
            {
                sampleOilPrintTime = value;
                this.RaisePropertyChanged(nameof(SampleOilPrintTime));
            }
        }


        private int sampleOilPrintedCount;

        public int SampleOilPrintedCount
        {
            get { return sampleOilPrintedCount; }
            set
            {
                sampleOilPrintedCount = value;
                this.RaisePropertyChanged(nameof(SampleOilPrintedCount));
            }
        }




        private bool sampleOilIsChecked;

        public bool SampleOilIsChecked
        {
            get { return sampleOilIsChecked; }
            set
            {
                sampleOilIsChecked = value;
                this.RaisePropertyChanged(nameof(SampleOilIsChecked));
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

    }
}
