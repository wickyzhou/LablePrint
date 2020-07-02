using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialPlanSeorderModel:NotificationObject
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

        public DateTime FDate { get; set; }

        public int FInterID { get; set; }

        public string FBillNo { get; set; }

        public int FEntryID { get; set; }

        public string FName { get; set; }

        public decimal FQty { get; set; }
    }
}
