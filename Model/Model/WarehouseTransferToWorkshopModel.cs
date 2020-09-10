using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WarehouseTransferToWorkshopModel:NotificationObject
    {
        public int Id { get; set; }
        public int BatchTypeId { get; set; }
        public string BatchTypeCode { get; set; }
        public string BatchTypeName { get; set; }

        public int FItemId { get; set; }
        public string FItemName { get; set; }
        public string FNumber { get; set; }


            

        public string Spec { get; set; }

        public int FStockID { get; set; }
        public double FQtyMust { get; set; }
        public double WorkshopInventory { get; set; }

        public double QtyTransfering { get; set; }


        public DateTime FPlanCommitDateBegin { get; set; }
        public DateTime FPlanCommitDateEnd { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? TransferTime { get; set; }
        public bool Deleted { get; set; }


        private string fBatchNoAndActualQty;

        public string FBatchNoAndActualQty
        {
            get { return fBatchNoAndActualQty; }
            set
            {
                fBatchNoAndActualQty = value;
                this.RaisePropertyChanged(nameof(FBatchNoAndActualQty));
            }
        }
        private double qtyTransfered;

        public double QtyTransfered
        {
            get { return qtyTransfered; }
            set
            {
                qtyTransfered = value;
                this.RaisePropertyChanged(nameof(QtyTransfered));
            }
        }


        private string transferedBillNo;

        public string TransferedBillNo
        {
            get { return transferedBillNo; }
            set
            {
                transferedBillNo = value;
                this.RaisePropertyChanged(nameof(TransferedBillNo));
            }
        }

        private bool isChecked = false;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.RaisePropertyChanged(nameof(IsChecked));
            }
        }


    }
}
