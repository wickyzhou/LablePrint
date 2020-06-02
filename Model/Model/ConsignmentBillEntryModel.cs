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

        public string CaseName { get; set; }

        public float ETotalQuatity { get; set; }


        private float eCurrencyQuatity;

        public float ECurrencyQuatity
        {
            get { return eCurrencyQuatity; }
            set
            {
                eCurrencyQuatity = value;
                this.RaisePropertyChanged(nameof(ECurrencyQuatity));
            }
        }



        public float EUndoQuatity { get; set; }


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


    }
}
