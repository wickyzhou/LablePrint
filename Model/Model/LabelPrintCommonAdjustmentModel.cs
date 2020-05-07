using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class LabelPrintCommonAdjustmentModel : NotificationObject
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


        private int? expirationMonth;

        public int? ExpirationMonth
        {
            get { return expirationMonth; }
            set
            {
                expirationMonth = value;
                this.RaisePropertyChanged(nameof(ExpirationMonth));
            }
        }


        private int? expirationDay;

        public int? ExpirationDay
        {
            get { return expirationDay; }
            set
            {
                expirationDay = value;
                this.RaisePropertyChanged(nameof(ExpirationDay));
            }
        }


        private int identityType;

        public int IdentityType
        {
            get { return identityType; }
            set
            {
                identityType = value;
                this.RaisePropertyChanged(nameof(IdentityType));
            }
        }

        private string identityTypeDesc;

        public string IdentityTypeDesc
        {
            get { return identityTypeDesc; }
            set
            {
                identityTypeDesc = value;
                this.RaisePropertyChanged(nameof(IdentityTypeDesc));
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



        // public string OrgID { get; set; }

        // public string Label { get; set; }

        // public string ProductionName { get; set; }

        // public string ExpirationMonth { get; set; }

        // public int? ExpirationDay { get; set; }

        // public int IdentityType { get; set; }

        // public string IdentityTypeDesc { get; set; }
    }
}

