using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DataGridColumnHeaderModel : NotificationObject
    {

        public int Id { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; } = DateTime.Now;

        public long TimeTicks { get; set; } = DateTime.Now.Millisecond;

        private string dataGridCode;

        public string DataGridCode
        {
            get { return dataGridCode; }
            set
            {
                dataGridCode = value;
                this.RaisePropertyChanged(nameof(DataGridCode));
            }
        }

        private string dataGridName;

        public string DataGridName
        {
            get { return dataGridName; }
            set
            {
                dataGridName = value;
                this.RaisePropertyChanged(nameof(DataGridName));
            }
        }


        private string tableName;

        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value;
                this.RaisePropertyChanged(nameof(TableName));
            }
        }

        private int columnOrder;

        public int ColumnOrder
        {
            get { return columnOrder; }
            set
            {
                columnOrder = value;
                this.RaisePropertyChanged(nameof(ColumnOrder));
            }
        }

        private string columnFieldName;

        public string ColumnFieldName
        {
            get { return columnFieldName; }
            set
            {
                columnFieldName = value;
                this.RaisePropertyChanged(nameof(ColumnFieldName));
            }
        }

        private string columnHeaderName;

        public string ColumnHeaderName
        {
            get { return columnHeaderName; }
            set
            {
                columnHeaderName = value;
                this.RaisePropertyChanged(nameof(ColumnHeaderName));
            }
        }

        private bool columnVisibility;

        public bool ColumnVisibility
        {
            get { return columnVisibility; }
            set
            {
                columnVisibility = value;
                this.RaisePropertyChanged(nameof(ColumnVisibility));
            }
        }


        private double columnWidth;

        public double ColumnWidth
        {
            get { return columnWidth; }
            set
            {
                columnWidth = value;
                this.RaisePropertyChanged(nameof(ColumnWidth));
            }
        }

        private char? columnWidthUnitType;

        public char? ColumnWidthUnitType
        {
            get { return columnWidthUnitType; }
            set
            {
                columnWidthUnitType = value;
                this.RaisePropertyChanged(nameof(ColumnWidthUnitType));
            }
        }


        private string note;

        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                this.RaisePropertyChanged(nameof(Note));
            }
        }

        private string bindingStringFormat;

        public string BindingStringFormat
        {
            get { return bindingStringFormat; }
            set
            {
                bindingStringFormat = value;
                this.RaisePropertyChanged(nameof(BindingStringFormat));
            }
        }

        private string converterName;

        public string ConverterName
        {
            get { return converterName; }
            set
            {
                converterName = value;
                this.RaisePropertyChanged(nameof(ConverterName));
            }
        }

        private int mainMenuId;

        public int MainMenuId
        {
            get { return mainMenuId; }
            set
            {
                mainMenuId = value;
                this.RaisePropertyChanged(nameof(MainMenuId));
            }
        }

        private bool isFrozenColumn;

        public bool IsFrozenColumn
        {
            get { return isFrozenColumn; }
            set
            {
                isFrozenColumn = value;
                this.RaisePropertyChanged(nameof(IsFrozenColumn));
            }
        }
    }
}
