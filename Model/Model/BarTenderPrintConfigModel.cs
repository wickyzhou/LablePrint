using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BarTenderPrintConfigModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string HostName { get; set; }

        public int TemplateTypeId { get; set; }

        public string TemplateTypeName { get; set; }

        public string PrinterName { get; set; }

        public int TemplatePerPage { get; set; }

        public string TemplateFileName { get; set; }

        public string TemplateFullName { get; set; }

        public string TemplateFolderPath { get; set; }

    }
}
