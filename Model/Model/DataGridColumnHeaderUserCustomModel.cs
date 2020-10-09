using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DataGridColumnHeaderUserCustomModel:NotificationObject
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime ModifyTime { get; set; }

        public DateTime CreateTime { get; set; }

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



        private bool isDownLoad;

        public bool IsDownLoad
        {
            get { return isDownLoad; }
            set
            {
                isDownLoad = value;
                this.RaisePropertyChanged(nameof(IsDownLoad));
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

        private long timeTicks;

        public long TimeTicks
        {
            get { return timeTicks; }
            set
            {
                timeTicks = value;
                this.RaisePropertyChanged(nameof(TimeTicks));
            }
        }

    }
}
