using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LabelPrintSpecialRequestModel:NotificationObject
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

        private int requestSeq;

        public int RequestSeq
        {
            get { return requestSeq; }
            set
            {
                requestSeq = value;
                this.RaisePropertyChanged(nameof(RequestSeq));
            }
        }

        private string requestName;

        public string RequestName
        {
            get { return requestName; }
            set
            {
                requestName = value;
                this.RaisePropertyChanged(nameof(RequestName));
            }
        }

        private string requestValue;

        public string RequestValue
        {
            get { return requestValue; }
            set
            {
                requestValue = value;
                this.RaisePropertyChanged(nameof(RequestValue));
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


        //private bool isFixedValue;

        //public bool IsFixedValue
        //{
        //    get { return isFixedValue; }
        //    set
        //    {
        //        isFixedValue = value;
        //        this.RaisePropertyChanged(nameof(IsFixedValue));
        //    }
        //}

    }
}
