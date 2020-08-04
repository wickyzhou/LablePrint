using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DataGridColumnHeaderModel
    {
        public int Id { get; set; }

        public string DataGridCode { get; set; }

        public string DataGridName { get; set; }

        public string TableName { get; set; }

        public int ColumnOrder { get; set; }

        public string ColumnFieldName { get; set; }

        public string ColumnHeaderName { get; set; }

        public bool ColumnVisibility { get; set; }

        public int ColumnWidth { get; set; }

        public string ColumnWidthUnitType { get; set; }

        public DateTime CreateTime { get; set; }

        public string Note { get; set; }

        public string BindingStringFormat { get; set; }
    }
}
