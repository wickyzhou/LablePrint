using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class ConsignmentBillParameterModel:NotificationObject
    {
        private DateTime paramDate;

        public DateTime ParamDate
        {
            get { return paramDate; }
            set
            {
                paramDate = value;
                this.RaisePropertyChanged(nameof(ParamDate));
            }
        }

        private string paramBillNo;

        public string ParamBillNo
        {
            get { return paramBillNo; }
            set
            {
                paramBillNo = value;
                this.RaisePropertyChanged(nameof(ParamBillNo));
            }
        }

        private string paramDeptName;

        public string ParamDeptName
        {
            get { return paramDeptName; }
            set
            {
                paramDeptName = value;
                this.RaisePropertyChanged(nameof(ParamDeptName));
            }
        }

        private string paramCustName;

        public string ParamCustName
        {
            get { return paramCustName; }
            set
            {
                paramCustName = value;
                this.RaisePropertyChanged(nameof(ParamCustName));
            }
        }

        private string paramMaterialName;

        public string ParamMaterialName
        {
            get { return paramMaterialName; }
            set
            {
                paramMaterialName = value;
                this.RaisePropertyChanged(nameof(ParamMaterialName));
            }
        }

        private float paramRestQuatity;

        public float ParamRestQuatity
        {
            get { return paramRestQuatity; }
            set
            {
                paramRestQuatity = value;
                this.RaisePropertyChanged(nameof(ParamRestQuatity));
            }
        }

        private bool isSeleted;

        public bool IsSelected
        {
            get { return isSeleted; }
            set
            {
                isSeleted = value;
                this.RaisePropertyChanged(nameof(IsSelected));
            }
        }


        //public DateTime ParamDate { get; set; }

        //public string ParamBillNo { get; set; }

        //public string ParamDeptName { get; set; }

        //public string ParamCustName { get; set; }

        //public string ParamMaterialName { get; set; }

        //public float ParamRestQuatity { get; set; }

    }
}
