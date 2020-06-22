using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OilSampleEntryModel : NotificationObject
    {
        public decimal Id { get; set; }
        public decimal FormmainId { get; set; }

        public int EntryId { get; set; }


        private string productionName;

        public string ProductionName
        {
            get { return productionName; }
            set
            {
                productionName = value;
                this.RaisePropertyChanged(nameof(ProductionName));
            }
        }


        private string productionModel;

        public string ProductionModel
        {
            get { return productionModel; }
            set
            {
                productionModel = value;
                this.RaisePropertyChanged(nameof(ProductionModel));
            }
        }




        public string RoughWeight { get; set; }


        private float weightPerBucket;

        public float WeightPerBucket
        {
            get { return weightPerBucket; }
            set
            {
                weightPerBucket = value;
                this.RaisePropertyChanged(nameof(WeightPerBucket));
                this.RaisePropertyChanged(nameof(PrintTotalCount));
            }
        }


        private float totalWeight;

        public float TotalWeight
        {
            get { return totalWeight; }
            set
            {
                totalWeight = value;
                this.RaisePropertyChanged(nameof(TotalWeight));
                this.RaisePropertyChanged(nameof(PrintTotalCount));
            }
        }

        public string ExpirationMonth { get; set; }

        public string CheckNo { get; set; }

        public DateTime ProductionDate { get; set; }

        public string BatchNo { get; set; }

        private int printedCount;

        public int PrintedCount
        {
            get { return printedCount; }
            set
            {
                printedCount = value;
                this.RaisePropertyChanged(nameof(PrintedCount));
            }
        }

        private int printTotalCount;

        public int PrintTotalCount
        {
            get { return TotalWeight % WeightPerBucket == 0 ? (int)(TotalWeight / WeightPerBucket) : (int)(TotalWeight / WeightPerBucket + 1); }
            set
            {
                printTotalCount = value;
                this.RaisePropertyChanged(nameof(PrintTotalCount));
            }
        }

        private int currencyPrintCount;

        public int CurrencyPrintCount
        {
            get { return currencyPrintCount; }
            set
            {
                currencyPrintCount = value;
                this.RaisePropertyChanged(nameof(CurrencyPrintCount));
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

        public string Title { get; set; }

    }
}
