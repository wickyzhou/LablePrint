using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ConsignmentBillEntryModel:NotificationObject
    {   
        
        public int Id { get; set; }

        public int InterId { get; set; }

        public int EntryId { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int CaseId { get; set; }

        public string CaseName { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        private float eTotalQuantity;

        public float ETotalQuantity
        {
            get { return eTotalQuantity; }
            set
            {
                eTotalQuantity = value;
                this.RaisePropertyChanged(nameof(ETotalQuantity));
            }
        }

        private float eCurrencyQuantity;

        public float ECurrencyQuantity
        {
            get { return eCurrencyQuantity; }
            set
            {
                eCurrencyQuantity = value;
                this.RaisePropertyChanged(nameof(ECurrencyQuantity));
            }
        }


        private float eUndoQuantity;

        public float EUndoQuantity
        {
            get { return eUndoQuantity; }
            set
            {
                eUndoQuantity = value;
                this.RaisePropertyChanged(nameof(EUndoQuantity));
            }
        }


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

        public bool IsSystem { get; set; }

    }
}
