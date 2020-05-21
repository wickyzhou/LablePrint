using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ConsignmentBillModel:NotificationObject
    {
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


        public int Id { get; set; }

        public int FInterID { get; set; }

        public DateTime FDate { get; set; }

        public string FBillNo { get; set; }

        public int FCustID { get; set; }

        public string ContractPerson { get; set; }

        public string ContractPhone { get; set; }

        public int FDeptID { get; set; }

        public string CustName { get; set; }

        public string DeptName { get; set; }

        public string RestStatus { get; set; }
        
        public DateTime CreateTime { get; set; }
                    
    }
}
