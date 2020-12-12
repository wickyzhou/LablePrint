using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DataGridColumnHeaderQueryParameterModel: NotificationObject
    {
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

    }
}
