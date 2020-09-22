using Dapper;
using Model;
using System.Collections.Generic;
using System.Linq;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class DataGridManagementService
    {
        public List<DataGridColumnHeaderModel> GetDataGridLists(string filter = "")
        {
            string sql = $" select  * from SJDataGridColumnHeader where 1=1  {filter} ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<DataGridColumnHeaderModel>(sql).ToList();
            }
        }

        public bool Insert(DataGridColumnHeaderModel model)
        {
            string sql = @" insert into SJDataGridColumnHeader(DataGridCode,DataGridName,TableName,ColumnOrder,ColumnFieldName,ColumnHeaderName,ColumnVisibility,ColumnWidth,Note,ColumnWidthUnitType,ConverterName,BindingStringFormat,MainMenuId) values(@DataGridCode,@DataGridName,@TableName,@ColumnOrder,@ColumnFieldName,@ColumnHeaderName,@ColumnVisibility,@ColumnWidth,@Note,@ColumnWidthUnitType,@ConverterName,@BindingStringFormat,@MainMenuId)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model)>0;
            }
        }
    }
}
