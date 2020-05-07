using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class PrintSchemaParameterModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SchemaId { get; set; }

        public string TemplateFullName { get; set; }

        public string TemplateFileName { get; set; }

        public string Orientation { get; set; }

        public string PrinterName { get; set; }

        public string FolderPath { get; set; }

    }
}
