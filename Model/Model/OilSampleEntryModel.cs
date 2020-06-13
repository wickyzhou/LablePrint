using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class OilSampleEntryModel:NotificationObject
    {  
        public double Id { get; set; }

        public int EntryId { get; set; }

        public string ProductionName { get; set; }

        public string ProductionModel { get; set; }

        public string RoughWeight { get; set; }

        public float WeightPerBucket { get; set; }

        public float TotalWeight { get; set; }

        public string ExpirationMonth { get; set; }

        public string CheckNo { get; set; }

        public string ProductionDate { get; set; }

        public string BatchNo { get; set; }

        public float PrintedWeight { get; set; }

        private int printCount;

        public int PrintCount   
        {
            get { return printCount; }
            set
            {
                printCount = value;
                this.RaisePropertyChanged(nameof(PrintCount));
            }
        }

    }
}
