using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class DataGridManagementService
    {
        public CommonService _commonService { get; set; } = new CommonService();

        public List<DataGridColumnHeaderModel> GetDataGridLists(string filter = "")
        {
            string sql = $" select  * from SJDataGridColumnHeader where 1=1  {filter} ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<DataGridColumnHeaderModel>(sql).ToList();
            }
        }

        public List<DataGridColumnHeaderUserCustomModel> GetUserDataGridColumnLists(string dataGridName, int userId,bool visibility)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@DataGridName", dataGridName, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@Visibility", visibility, DbType.Boolean, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<DataGridColumnHeaderUserCustomModel>("SJGetUserDataGridColumnHeaderConfiguration", dp, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }

        public bool SyncUserDataGridColumnConfiguration(string dataGridName, int userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@DataGridName", dataGridName, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJSyncUserDataGridColumnHeaderConfiguration", dp, null, null, CommandType.StoredProcedure) > 0;
            }
        }


        public bool Insert(DataGridColumnHeaderModel model)
        {
            string sql = @" insert into SJDataGridColumnHeader(DataGridCode,DataGridName,TableName,ColumnOrder,ColumnFieldName,ColumnHeaderName,ColumnVisibility,ColumnWidth,Note,ColumnWidthUnitType,ConverterName,BindingStringFormat,MainMenuId) values(@DataGridCode,@DataGridName,@TableName,@ColumnOrder,@ColumnFieldName,@ColumnHeaderName,@ColumnVisibility,@ColumnWidth,@Note,@ColumnWidthUnitType,@ConverterName,@BindingStringFormat,@MainMenuId)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool BatchUpdate()
        {
            string sql = @" update a set TableName=b.TableName,ColumnOrder=b.ColumnOrder,	ColumnFieldName=b.ColumnFieldName,	ColumnHeaderName=b.ColumnHeaderName,	
				                            ColumnVisibility=b.ColumnVisibility,ColumnWidth = b.ColumnWidth,ModifyTime=getdate(),Note=b.Note,	
				                            ColumnWidthUnitType=b.ColumnWidthUnitType,ConverterName=b.ConverterName,BindingStringFormat=b.BindingStringFormat,MainMenuId=b.MainMenuId,IsFrozenColumn = b.IsFrozenColumn
                from  SJDataGridColumnHeader a join 
                (select *, rank() over( order by TimeTicks desc) rnk from SJDataGridColumnHeaderTemplate ) b
                on a.Id =b.id and b.rnk = 1 and b.UserId=-1 ;";

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }


        /// <summary>
        /// 界面调整列宽和顺序
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="userId"></param>
        public void SaveColumnConfigurationInUserInterface(DataGrid grid, int userId)
        {
            string frozenString = string.Empty;
            // 同步用户自定义后台参数 初始化加载就已经同步了字段
           // SyncUserDataGridColumnConfiguration(grid.Name, userId);

            // 将界面参数更新到后台
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in grid.Columns)
            {
                var column = item as DataGridTextColumn;
                int order = column.DisplayIndex;
                double width = Math.Round(column.ActualWidth, 2);
                string fieldName = (column.Binding as Binding).Path.Path;
                //if (grid.FrozenColumnCount == column.DisplayIndex)
                //    frozenString = " , IsFrozenColumn = 1 ";
                //{ frozenString}

                stringBuilder.Append($" update SJDataGridUserCustom set ColumnWidth ={width} ,ColumnWidthUnitType='',ColumnOrder = {order}  " +
                    $" where DataGridName = '{grid.Name}' and UserId = {userId} and ColumnFieldName ='{fieldName}' ;");
            }

            _commonService.ExecuteSqlAsync(stringBuilder.ToString());
        }


        /// <summary>
        /// 主要调整是否下载 可见性 冻结列等
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="userId"></param>
        public bool SaveColumnConfigurationInManagementView(string dataGridName,int userId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@DataGridName", dataGridName, DbType.String, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJSaveUserDataGridColumnHeaderConfiguration", dp, null, null, CommandType.StoredProcedure) > 0;
            }
        }

        //public  void SaveColumnConfigurationInManagementView(IEnumerable<DataGridColumnHeaderUserCustomModel> lists)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();

        //    foreach (var item in lists)
        //    {
        //        stringBuilder.Append($" update SJDataGridUserCustom set IsDownLoad ={item.IsDownLoad} ,ColumnVisibility={item.ColumnVisibility}" +
        //            $"  ,IsFrozenColumn = {item.IsFrozenColumn}, BindingStringFormat = {item.BindingStringFormat},ColumnOrder = {item.ColumnOrder} " +
        //            $" where DataGridName = '{item.DataGridName}' and UserId = {item.UserId} and ColumnFieldName ='{item.ColumnFieldName}' ;");
        //    }
        //    _commonService.ExecuteSqlAsync(stringBuilder.ToString());

        //}

    }
}
