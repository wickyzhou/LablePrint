using Dapper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

        public List<DataGridColumnHeaderUserCustomModel> GetUserDataGridColumnLists(string datagridName,int userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@DatagridName", datagridName, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<DataGridColumnHeaderUserCustomModel>("SJGetUserDataGridColumnHeaderConfiguration", dp,null,true,null,CommandType.StoredProcedure).ToList();
            }
        }

        public bool SyncUserDataGridColumnConfiguration(string datagridName, int userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@DatagridName", datagridName, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJSyncUserDataGridColumnHeaderConfiguration", dp, null, null, CommandType.StoredProcedure)>0;
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

        public bool BatchUpdate()
        {
            string sql = @" update a set TableName=b.TableName,ColumnOrder=b.ColumnOrder,	ColumnFieldName=b.ColumnFieldName,	ColumnHeaderName=b.ColumnHeaderName,	
				                            ColumnVisibility=b.ColumnVisibility,ColumnWidth = b.ColumnWidth,ModifyTime=getdate(),Note=b.Note,	
				                            ColumnWidthUnitType=b.ColumnWidthUnitType,ConverterName=b.ConverterName,BindingStringFormat=b.BindingStringFormat,MainMenuId=b.MainMenuId
                from  SJDataGridColumnHeader a join 
                (select *, rank() over( order by TimeTicks desc) rnk from SJDataGridColumnHeaderTemplate ) b
                on a.Id =b.id and b.rnk = 1 and b.UserId=-1 ;";
            
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }
    }
}
