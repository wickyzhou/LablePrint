using System;

namespace Model
{
    public class SalesRebateRecentParameterMainModel : NotificationObject
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        private int caseId;

        public int CaseId
        {
            get { return caseId; }
            set
            {
                caseId = value;
                this.RaisePropertyChanged(nameof(CaseId));
            }
        }

        private int orgId;

        public int OrgId
        {
            get { return orgId; }
            set
            {
                orgId = value;
                this.RaisePropertyChanged(nameof(OrgId));
            }
        }

        private int rebateClass;

        public int RebateClass
        {
            get { return rebateClass; }
            set
            {
                rebateClass = value;
                this.RaisePropertyChanged(nameof(RebateClass));
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

        private int taxAmountType;

        public int TaxAmountType
        {
            get { return taxAmountType; }
            set
            {
                taxAmountType = value;
                this.RaisePropertyChanged(nameof(TaxAmountType));
            }
        }

        private int minusLastPeriodRebateType;

        public int MinusLastPeriodRebateType
        {
            get { return minusLastPeriodRebateType; }
            set
            {
                minusLastPeriodRebateType = value;
                this.RaisePropertyChanged(nameof(MinusLastPeriodRebateType));
            }
        }

        private int rebatePctType;

        public int RebatePctType
        {
            get { return rebatePctType; }
            set
            {
                rebatePctType = value;
                this.RaisePropertyChanged(nameof(RebatePctType));
            }
        }

        private double? rebatePctValue;

        public double? RebatePctValue
        {
            get { return rebatePctValue; }
            set
            {
                rebatePctValue = value;
                this.RaisePropertyChanged(nameof(RebatePctValue));
            }
        }

        private int amountRangeCalculateType;

        public int AmountRangeCalculateType
        {
            get { return amountRangeCalculateType; }
            set
            {
                amountRangeCalculateType = value;
                this.RaisePropertyChanged(nameof(AmountRangeCalculateType));
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

        private string orgName;

        public string OrgName
        {
            get { return orgName; }
            set
            {
                orgName = value;
                this.RaisePropertyChanged(nameof(OrgName));
            }
        }

        private string rebateClassName;

        public string RebateClassName
        {
            get { return rebateClassName; }
            set
            {
                rebateClassName = value;
                this.RaisePropertyChanged(nameof(RebateClassName));
            }
        }

        private string rebatePctTypeName;

        public string RebatePctTypeName
        {
            get { return rebatePctTypeName; }
            set
            {
                rebatePctTypeName = value;
                this.RaisePropertyChanged(nameof(RebatePctTypeName));
            }
        }

        private string taxAmountTypeName;

        public string TaxAmountTypeName
        {
            get { return taxAmountTypeName; }
            set
            {
                taxAmountTypeName = value;
                this.RaisePropertyChanged(nameof(TaxAmountTypeName));
            }
        }

        private string minusLastPeriodRebateTypeName;

        public string MinusLastPeriodRebateTypeName
        {
            get { return minusLastPeriodRebateTypeName; }
            set
            {
                minusLastPeriodRebateTypeName = value;
                this.RaisePropertyChanged(nameof(MinusLastPeriodRebateTypeName));
            }
        }

        private string amountRangeCalculateTypeName;

        public string AmountRangeCalculateTypeName
        {
            get { return amountRangeCalculateTypeName; }
            set
            {
                amountRangeCalculateTypeName = value;
                this.RaisePropertyChanged(nameof(AmountRangeCalculateTypeName));
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

        private DateTime settleDateBegin;

        public DateTime SettleDateBegin
        {
            get { return settleDateBegin; }
            set
            {
                settleDateBegin = value;
                this.RaisePropertyChanged(nameof(SettleDateBegin));
            }
        }

        private DateTime settleDateEnd;

        public DateTime SettleDateEnd
        {
            get { return settleDateEnd; }
            set
            {
                settleDateEnd = value;
                this.RaisePropertyChanged(nameof(SettleDateEnd));
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

        private double? taxAmountIncludeDiscount;

        public double? TaxAmountIncludeDiscount
        {
            get { return taxAmountIncludeDiscount; }
            set
            {
                taxAmountIncludeDiscount = value;
                this.RaisePropertyChanged(nameof(TaxAmountIncludeDiscount));
            }
        }

        private double? taxAmountNoDiscount;

        public double? TaxAmountNoDiscount
        {
            get { return taxAmountNoDiscount; }
            set
            {
                taxAmountNoDiscount = value;
                this.RaisePropertyChanged(nameof(TaxAmountNoDiscount));
            }
        }


        private double? noTaxAmountIncludeDiscount;

        public double? NoTaxAmountIncludeDiscount
        {
            get { return noTaxAmountIncludeDiscount; }
            set
            {
                noTaxAmountIncludeDiscount = value;
                this.RaisePropertyChanged(nameof(NoTaxAmountIncludeDiscount));
            }
        }

        private double? noTaxAmountNoDiscount;

        public double? NoTaxAmountNoDiscount
        {
            get { return noTaxAmountNoDiscount; }
            set
            {
                noTaxAmountNoDiscount = value;
                this.RaisePropertyChanged(nameof(NoTaxAmountNoDiscount));
            }
        }

        private Guid pGuid;

        public Guid PGuid
        {
            get { return pGuid; }
            set
            {
                pGuid = value;
                this.RaisePropertyChanged(nameof(PGuid));
            }
        }

        private DateTime? calculateTime;

        public DateTime? CalculateTime
        {
            get { return calculateTime; }
            set
            {
                calculateTime = value;
                this.RaisePropertyChanged(nameof(CalculateTime));
            }
        }


    }
}
