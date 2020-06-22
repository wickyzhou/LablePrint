using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class BarTenderTemplateModel
    {
        public int TemplateId { get; set; }

        public int TemplatePerPage { get; set; }

        public string TemplateFileName { get; set; }

        public string TemplateFullName { get; set; }

        public string TemplateFolderPath { get; set; }

        public int TemplateTotalPage { get; set; }
    }
}
