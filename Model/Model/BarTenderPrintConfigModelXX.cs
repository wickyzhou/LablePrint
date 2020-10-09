using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BarTenderPrintConfigModelXX:NotificationObject
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged(nameof(Id));
            }
        }

        public int UserId { get; set; }

        public string HostName { get; set; }

        public int TemplateTypeId { get; set; }

        public string TemplateTypeName { get; set; }

        public string PrinterName { get; set; }

        private BarTenderTemplateModel templateSelectedItem;

        public BarTenderTemplateModel TemplateSelectedItem
        {
            get { return templateSelectedItem; }
            set
            {
                templateSelectedItem = value;
                this.RaisePropertyChanged(nameof(TemplateSelectedItem));
            }
        }

        private DateTime? modifyTime;

        public DateTime? ModifyTime
        {
            get { return modifyTime; }
            set
            {
                modifyTime = value;
                this.RaisePropertyChanged(nameof(ModifyTime));
            }
        }

    }
}
