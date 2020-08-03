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


    }
}
