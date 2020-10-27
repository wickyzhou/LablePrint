using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaterialTimelyInventoryModel:NotificationObject
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public string MaterialNumber { get; set; }

        public int StockId { get; set; }

        public string StockNumber { get; set; }

        public string StockName { get; set; }

        public string BatchNo { get; set; }

        public double TotalWeight { get; set; }

        public DateTime ProductionDate { get; set; }

        private double? transferingWeight;

        public double? TransferingWeight
        {
            get { return transferingWeight; }
            set
            {
                transferingWeight = value;
                this.RaisePropertyChanged(nameof(TransferingWeight));
            }
        }

        private double? transferedWeight;

        public double? TransferedWeight
        {
            get { return transferedWeight; }
            set
            {
                transferedWeight = value;
                this.RaisePropertyChanged(nameof(TransferedWeight));
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


        private DateTime? transferedTime;

        public DateTime? TransferedTime
        {
            get { return transferedTime; }
            set
            {
                transferedTime = value;
                this.RaisePropertyChanged(nameof(TransferedTime));
            }
        }

        private string spec;

        public string Spec
        {
            get { return spec; }
            set
            {
                spec = value;
                this.RaisePropertyChanged(nameof(Spec));
            }
        }
    }
}
