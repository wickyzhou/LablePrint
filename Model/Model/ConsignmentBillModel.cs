using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ConsignmentBillModel:NotificationObject
    {

        public int Id { get; set; }

        public int InterId { get; set; }

        public DateTime BillDate { get; set; }

        public string BillNo { get; set; }

        public int CustId { get; set; }

        public string ContractPerson { get; set; }

        public string ContractPhone { get; set; }

        public int DeptId { get; set; }

        public string CustName { get; set; }

        public string DeptName { get; set; }

        public DateTime CreateTime { get; set; }

        private int selectedStatus;

        public int SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                this.RaisePropertyChanged(nameof(SelectedStatus));
            }
        }

        private float totalQuatity;

        public float TotalQuatity
        {
            get { return totalQuatity; }
            set
            {
                totalQuatity = value;
                this.RaisePropertyChanged(nameof(TotalQuatity));
            }
        }

        private float undoQuatity;

        public float UndoQuatity
        {
            get { return undoQuatity; }
            set
            {
                undoQuatity = value;
                this.RaisePropertyChanged(nameof(UndoQuatity));
            }
        }


        private float currencyQuatity;

        public float CurrencyQuatity
        {
            get { return currencyQuatity; }
            set
            {
                currencyQuatity = value;
                this.RaisePropertyChanged(nameof(CurrencyQuatity));
            }
        }

        public int UserId { get; set; }

    }
}
