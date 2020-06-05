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

        private float totalQuantity;

        public float TotalQuantity
        {
            get { return totalQuantity; }
            set
            {
                totalQuantity = value;
                this.RaisePropertyChanged(nameof(TotalQuantity));
            }
        }

        private float undoQuantity;

        public float UndoQuantity
        {
            get { return undoQuantity; }
            set
            {
                undoQuantity = value;
                this.RaisePropertyChanged(nameof(UndoQuantity));
            }
        }


        private float currencyQuantity;

        public float CurrencyQuantity
        {
            get { return currencyQuantity; }
            set
            {
                currencyQuantity = value;
                this.RaisePropertyChanged(nameof(CurrencyQuantity));
            }
        }

        public int UserId { get; set; }

    }
}
