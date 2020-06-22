using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BarTenderPrintConfigModelXX:NotificationObject
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string HostName { get; set; }

        public int TemplateTypeId { get; set; }

        public string TemplateTypeName { get; set; }

        public string PrinterName { get; set; }

        private BarTenderTemplateModel expressTemplateSelectedItem;

        public BarTenderTemplateModel ExpressTemplateSelectedItem
        {
            get { return expressTemplateSelectedItem; }
            set
            {
                expressTemplateSelectedItem = value;
                this.RaisePropertyChanged(nameof(ExpressTemplateSelectedItem));
            }
        }

        public int TemplateTotalPage { get; set; }

    }
}
