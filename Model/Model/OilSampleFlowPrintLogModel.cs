using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OilSampleFlowPrintLogModel:NotificationObject
    {
        public int Id { get; set; }
        public decimal FormmainId { get; set; }
        public decimal FormsonId { get; set; }
        public string Title { get; set; }
        public int TypeId { get; set; }
        public string TypeDesc { get; set; }


        public DateTime CreateTime { get; set; }
        public string BatchNo { get; set; }
        public int PrintCount { get; set; }

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


        public int EntryId { get; set; }


    }
}
