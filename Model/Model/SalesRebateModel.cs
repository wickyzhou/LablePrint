using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class SalesRebateModel:NotificationObject
    {

        private int userId;

        public int UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                this.RaisePropertyChanged(nameof(UserId));
            }
        }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.RaisePropertyChanged(nameof(UserName));
            }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged(nameof(Id));
            }
        }

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

        public int TaxAmountType;

        public int MinusLastPeriodRebateType;

        public int RebatePctType;

        public int AmountRangeCalculateType;

        public double? RebatePctValue;

        public double CalculateRebateAmout { get; set; }

        public DateTime? CalculateTime { get; set; }

        private Guid sGuid;

        public Guid SGuid
        {
            get { return sGuid; }
            set
            {
                sGuid = value;
                this.RaisePropertyChanged(nameof(SGuid));
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

    }
}
