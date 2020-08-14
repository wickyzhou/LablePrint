using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ImportTemplateExcelHeaderFieldMappingModel
    {
        public int TableId { get; set; }

        public string TableName { get; set; }

        public int ColumnSeq { get; set; }

        public string ExcelName { get; set; }

        public string FieldName { get; set; }

    }
}
