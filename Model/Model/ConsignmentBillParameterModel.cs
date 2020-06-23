using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class ConsignmentBillParameterModel:NotificationObject
    {
        private DateTime paramBeginDate;

        public DateTime ParamBeginDate
        {
            get { return paramBeginDate; }
            set
            {
                paramBeginDate = value;
                this.RaisePropertyChanged(nameof(ParamBeginDate));
            }
        }

        private DateTime paramEndDate;

        public DateTime ParamEndDate
        {
            get { return paramEndDate; }
            set
            {
                paramEndDate = value;
                this.RaisePropertyChanged(nameof(ParamEndDate));
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

        //private EnumModel paramBillType;

        //public EnumModel ParamBillType
        //{
        //    get { return paramBillType; }
        //    set
        //    {
        //        paramBillType = value;
        //        this.RaisePropertyChanged(nameof(ParamBillType));
        //    }
        //}


        private int paramBillType;

        public int ParamBillType
        {
            get { return paramBillType; }
            set
            {
                paramBillType = value;
                this.RaisePropertyChanged(nameof(ParamBillType));
            }
        }



        private int paramBillBeginSeq;

        public int ParamBillBeginSeq
        {
            get { return paramBillBeginSeq; }
            set
            {
                paramBillBeginSeq = value;
                this.RaisePropertyChanged(nameof(ParamBillBeginSeq));
            }
        }

        private int paramBillEndSeq;

        public int ParamBillEndSeq
        {
            get { return paramBillEndSeq; }
            set
            {
                paramBillEndSeq = value;
                this.RaisePropertyChanged(nameof(ParamBillEndSeq));
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
