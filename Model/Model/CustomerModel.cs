using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CustomerModel :NotificationObject
    {


        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.RaisePropertyChanged(nameof(Selected));
            }
        }


        public int Id { get; set; }

        public int OrgId { get; set; }

        public string CustCode { get; set; }

        public string CustName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactTelephone { get; set; }

        public string ContactAddress { get; set; }

        public string FNumber { get; set; }

        public string FName { get; set; }


    }
}
