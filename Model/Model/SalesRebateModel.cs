using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class SalesRebateModel:NotificationObject
    {
        public int Id { get; set; }

        private int materialId;

        public int MaterialId
        {
            get { return materialId; }
            set
            {
                materialId = value;
                this.RaisePropertyChanged(nameof(MaterialId));
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


        private int taxAmountType;

        public int TaxAmountType
        {
            get { return taxAmountType; }
            set
            {
                taxAmountType = value;
                this.RaisePropertyChanged(nameof(TaxAmountType));
                this.RaisePropertyChanged(nameof(IsPassed));
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
                this.RaisePropertyChanged(nameof(IsPassed));
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
                this.RaisePropertyChanged(nameof(IsPassed));
            }
        }



        private double rebatePctValue;

        public double RebatePctValue
        {
            get { return rebatePctValue; }
            set
            {
                rebatePctValue = value;
                this.RaisePropertyChanged(nameof(RebatePctValue));
            }
        }


        public double ComputeRebatePctValue { get; set; }

        public double ComputeRebateAmout { get; set; }

        public double SalesRebateAmoutResult { get; set; }

        public Guid Guid { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.RaisePropertyChanged(nameof(IsChecked));
            }
        }



        private SalesRebateModel salesRebateSelectedItem;

        public SalesRebateModel SalesRebateSelectedItem
        {
            get { return salesRebateSelectedItem; }
            set
            {
                salesRebateSelectedItem = value;
                this.RaisePropertyChanged(nameof(SalesRebateSelectedItem));
            }
        }

        private string materialName;

        public string MaterialName
        {
            get { return materialName; }
            set
            {
                materialName = value;
                this.RaisePropertyChanged(nameof(MaterialName));
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

        private double orgTotalAmount;

        public double OrgTotalAmount
        {
            get { return orgTotalAmount; }
            set
            {
                orgTotalAmount = value;
                this.RaisePropertyChanged(nameof(OrgTotalAmount));
            }
        }

        private bool deleted;

        public bool Deleted
        {
            get { return deleted; }
            set
            {
                deleted = value;
                this.RaisePropertyChanged(nameof(Deleted));
            }
        }

        private string k3BillNo;

        public string K3BillNo
        {
            get { return k3BillNo; }
            set
            {
                k3BillNo = value;
                this.RaisePropertyChanged(nameof(K3BillNo));
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

        private DateTime modifyTime;

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set
            {
                modifyTime = value;
                this.RaisePropertyChanged(nameof(ModifyTime));
            }
        }

        private bool isPassed;

        public bool IsPassed
        {
            get 
            {
                if ( TaxAmountType>0 && minusLastPeriodRebateType>0 && ((RebatePctType==1 && RebatePctValue>0 ) || RebatePctType == 2))
                    return true;
                return false;
            }
            set
            {
               
            }
        }


        public string UserName { get; set; }


    }
}
