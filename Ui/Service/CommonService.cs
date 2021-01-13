using Common;
using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Ui.Extension;
using Ui.Helper;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class CommonService
    {
        public IList<EnumModel> GetEnumLists()
        {
            string sql = @"select * from SJEnumTable ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<EnumModel>(sql).ToList();
            }
        }

        public IList<EnumModel> GetEnumLists(int groupSeq)
        {
            string sql = @"select * from SJEnumTable where GroupSeq=@GroupSeq";
            //System.Threading.Thread.Sleep(3000);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<EnumModel>(sql, new { GroupSeq = groupSeq }).ToList();
            }
        }

        public IList<HostConfigModel> GetHostConfig(int typeId, string host)
        {
            string sql = @"select Id,TypeId,TypeDesciption,Host,Value HostValue,UserId from SJHostConfig where TypeId=@TypeId and Host=@Host ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<HostConfigModel>(sql, new { TypeId = typeId, Host = host }).ToList();
            }
        }

        public HostConfigModel GetHostConfig(int typeId, string host, int userId)
        {
            string sql = @"select Id,TypeId,TypeDesciption,Host,Value HostValue,UserId from SJHostConfig where TypeId=@TypeId and Host=@Host and UserId=@UserId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<HostConfigModel>(sql, new { TypeId = typeId, Host = host, UserId = userId }).FirstOrDefault();
            }
        }

        public HostConfigModel GetHostConfig(HostConfigModel model)
        {
            string sql = @"select Id,TypeId,TypeDesciption,Host,Value HostValue,UserId from SJHostConfig where TypeId=@TypeId and Host=@Host ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<HostConfigModel>(sql, model).FirstOrDefault();
            }
        }

        public bool SaveHostConfig(HostConfigModel model)
        {
            string sql = @" 
                            if(exists(select 1 from SJHostConfig where Host=@Host and UserId=@UserId and TypeId=@TypeId))
                                 update SJHostConfig set Value=@HostValue where Id=@Id;
                            else
                                insert into SJHostConfig(TypeId,TypeDesciption,Host,Value,UserId) values(@TypeId,@TypeDesciption,@Host,@HostValue,@UserId)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool InsertHostConfig(HostConfigModel model)
        {
            string sql = @" insert into SJHostConfig(TypeId,TypeDesciption,Host,Value,UserId) values(@TypeId,@TypeDesciption,@Host,@HostValue,@UserId)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool UpdateHostConfig(HostConfigModel model)
        {
            string sql = @" update SJHostConfig set Value=@Value where Id=@Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public List<string> GetComputerPrinters()
        {
            List<string> printer = new List<string>();

            foreach (string sPrint in PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                printer.Add(sPrint);
            }
            return printer;
        }

        #region BarTender报表
        public BarTenderPrintConfigModel GetBarTenderPrintConfig(int userId, int typeId, string hostName)
        {
            string sql = @" select * from SJBarTenderPrintConfig where TemplateTypeId=@TemplateTypeId and HostName=@HostName and UserId=@UserId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BarTenderPrintConfigModel>(sql, new { UserId = userId, TemplateTypeId = typeId, HostName = hostName }).FirstOrDefault();
            }
        }

        public int InsertBarTenderPrintConfig(BarTenderPrintConfigModel model)
        {
            string sql = @" insert into SJBarTenderPrintConfig(UserId,HostName,TemplateTypeId,TemplateTypeName,TemplateFullName,TemplateFileName,TemplatePerPage,PrinterName,TemplateFolderPath,TemplateTotalPage)
                            values(@UserId,@HostName,@TemplateTypeId,@TemplateTypeName,@TemplateFullName,@TemplateFileName,@TemplatePerPage,@PrinterName,@TemplateFolderPath,@TemplateTotalPage);select SCOPE_IDENTITY() as Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToInt32(connection.ExecuteScalar(sql, model));
            }
        }

        public bool UpdateBarTenderPrintConfig(BarTenderPrintConfigModel model)
        {
            string sql = @" update SJBarTenderPrintConfig set TemplateTypeId=@TemplateTypeId,TemplateTypeName=@TemplateTypeName,TemplateFullName=@TemplateFullName,
                            TemplateFileName=@TemplateFileName,TemplatePerPage=@TemplatePerPage,PrinterName=@PrinterName,TemplateTotalPage=@TemplateTotalPage where Id=@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public BarTenderPrintConfigModelXX GetBarTenderPrintConfigXX(int userId, int typeId, string hostName)
        {
            string sql = @"select * from SJBarTenderPrintConfig where TemplateTypeId=@TemplateTypeId and HostName=@HostName and UserId=@UserId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BarTenderPrintConfigModelXX, BarTenderTemplateModel, BarTenderPrintConfigModelXX>(sql, (c, t) => { c.TemplateSelectedItem = t; return c; }
              , new { UserId = userId, TemplateTypeId = typeId, HostName = hostName }, splitOn: "TemplatePerPage"
              ).FirstOrDefault();
            }
        }

        public int SaveBarTenderPrintConfigXX(BarTenderPrintConfigModelXX model)
        {
            if (model.Id==0)
                return InsertBarTenderPrintConfigXX(model);
             UpdateBarTenderPrintConfigXX(model);
            return model.Id;

        }

        public int InsertBarTenderPrintConfigXX(BarTenderPrintConfigModelXX model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@HostName", model.HostName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTypeId", model.TemplateTypeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateTypeName", model.TemplateTypeName, DbType.String, ParameterDirection.Input);
            dp.Add("@PrinterName", model.PrinterName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplatePerPage", model.TemplateSelectedItem.TemplatePerPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateFullName", model.TemplateSelectedItem.TemplateFullName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFileName", model.TemplateSelectedItem.TemplateFileName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFolderPath", model.TemplateSelectedItem.TemplateFolderPath, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTotalPage", model.TemplateSelectedItem.TemplateTotalPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateDisplayName", model.TemplateSelectedItem.TemplateDisplayName, DbType.String, ParameterDirection.Input);

            string sql = @" insert into SJBarTenderPrintConfig(UserId,HostName,TemplateTypeId,TemplateTypeName,TemplateFullName,TemplateFileName,TemplatePerPage,PrinterName,TemplateFolderPath,TemplateTotalPage,TemplateDisplayName)
                            values(@UserId,@HostName,@TemplateTypeId,@TemplateTypeName,@TemplateFullName,@TemplateFileName,@TemplatePerPage,@PrinterName,@TemplateFolderPath,@TemplateTotalPage,@TemplateDisplayName); 
                            select SCOPE_IDENTITY() as Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToInt32(connection.ExecuteScalar(sql, dp));
            }
        }

        public bool UpdateBarTenderPrintConfigXX(BarTenderPrintConfigModelXX model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@PrinterName", model.PrinterName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplatePerPage", model.TemplateSelectedItem.TemplatePerPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateFullName", model.TemplateSelectedItem.TemplateFullName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFileName", model.TemplateSelectedItem.TemplateFileName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFolderPath", model.TemplateSelectedItem.TemplateFolderPath, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTotalPage", model.TemplateSelectedItem.TemplateTotalPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateDisplayName", model.TemplateSelectedItem.TemplateDisplayName, DbType.String, ParameterDirection.Input);
            dp.Add("@HostName", model.HostName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTypeId", model.TemplateTypeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);

            string sql = @" update SJBarTenderPrintConfig set PrinterName=@PrinterName
                                ,TemplateFullName=@TemplateFullName,TemplateFileName=@TemplateFileName,TemplatePerPage=@TemplatePerPage
                                ,TemplateFolderPath=@TemplateFolderPath,TemplateTotalPage=@TemplateTotalPage,TemplateDisplayName=@TemplateDisplayName,ModifyTime =getdate()
                            where UserId=@UserId and	HostName =@HostName and 	TemplateTypeId = @TemplateTypeId ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, dp) > 0;
            }
        }




        public List<BarTenderTemplateModel> GetTenderPrintTemplates(string folderPath)
        {
            List<BarTenderTemplateModel> lists = new List<BarTenderTemplateModel>();
            if (Directory.Exists(folderPath))
            {
                foreach (var item in Directory.GetFiles(folderPath, "*.btw"))
                {
                    var s = Path.GetFileName(item).Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Count() == 2 && int.TryParse(s[0], out int p))
                    {

                        lists.Add(new BarTenderTemplateModel { TemplateTotalPage = p, TemplatePerPage = int.Parse(s[1].Substring(0, 1)), TemplateFileName = Path.GetFileName(s[1]), TemplateFullName = item, TemplateFolderPath = folderPath });
                    }
                    else
                        lists.Add(new BarTenderTemplateModel { TemplateTotalPage = 1, TemplatePerPage = 1, TemplateFileName = Path.GetFileName(item), TemplateFullName = item, TemplateFolderPath = folderPath });
                }
            }
            return lists;
        }




        #endregion


        #region 水晶报表打印相关
        public CrystalPrintConfigModel GetCrystalPrintConfig(int userId, int typeId, string hostName)
        {
            string sql = @"select * from SJCrystalPrintConfig where TypeId=@TypeId and HostName=@HostName and UserId=@UserId ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<CrystalPrintConfigModel>(sql, new { UserId = userId, TypeId = typeId, HostName = hostName }).FirstOrDefault();
            }
        }

        public List<PageSizeModel> GetPrinterPageSizes(string printerName)
        {
            List<PageSizeModel> lists = new List<PageSizeModel>();
            PageSettings settings = new PageSettings(new PrinterSettings { PrinterName = printerName });
            foreach (PaperSize item in settings.PrinterSettings.PaperSizes)
            {
                lists.Add(new PageSizeModel
                {
                    Height = item.Height,
                    Width = item.Width,
                    RawKind = item.RawKind,
                    Kind = item.Kind,
                    PaperName = item.PaperName
                });
            }

            return lists;
        }

        public bool InsertCrystalPrintConfig(CrystalPrintConfigModel model)
        {
            string sql = @" insert into SJCrystalPrintConfig(TypeId,TypeDesciption,HostName,PrinterName,MarginLeft,MarginTop,MarginRight,MarginBottom,PaperName,UserId)
                            values(@TypeId,@TypeDesciption,@HostName,@PrinterName,@MarginLeft,@MarginTop,@MarginRight,@MarginBottom,@PaperName,@UserId);";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool UpdateCrystalPrintConfig(CrystalPrintConfigModel model)
        {
            string sql = @" update SJCrystalPrintConfig set Orientation=@Orientation,PrinterName=@PrinterName,MarginLeft=@MarginLeft,MarginTop=@MarginTop,MarginRight=@MarginRight,MarginBottom=@MarginBottom,PaperName=@PaperName where Id=@Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }
        #endregion


        public int GetCurrentDateNextSerialNumber(DateTime settleDate, string colName)
        {
            string sql = @" select " + colName + " from SJCurrentDateNextSerialNumber where SettleDate=@SettleDate; update SJCurrentDateNextSerialNumber set  " + colName + "  += 1 where  SettleDate=@SettleDate;";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return Convert.ToInt32(connection.ExecuteScalar(sql, new { SettleDate = settleDate }));
            }
        }

        public int GetUserDataId(UserModel user, int menuId)
        {
            if (user.SuperAdmin)
                return -1;
            return GetPageAdmin(user.ID, menuId) ? -1 : user.ID;
        }

        public bool GetPageAdmin(int userId, int menuId)
        {
            string sql = @" select MainMenuAdmin from SJPageOwner where UserId=@UserId and MainMenuId=@MainMenuId";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToBoolean(connection.ExecuteScalar(sql, new { UserId = userId, MainMenuId = menuId }));
            }
        }

        public void WriteActionLog(ActionOperationLogModel model)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(20000);
                string sql = @" insert into SJActionOperationLog(MainMenuId,UserId,ActionName,ActionDesc,PKId,HostName) values(@MainMenuId,@UserId,@ActionName,@ActionDesc,@PKId,@HostName) ;";
                using (var connection = SqlDb.UpdateConnection)
                {
                    connection.Execute(sql, model);
                }
            });
        }

        public Task WriteActionLogAsync(ActionOperationLogModel model)
        {
            return Task.Factory.StartNew(() =>
             {
                 //Thread.Sleep(20000);
                 string sql = @" insert into SJActionOperationLog(MainMenuId,UserId,ActionName,ActionDesc,PKId,HostName) values(@MainMenuId,@UserId,@ActionName,@ActionDesc,@PKId,@HostName) ;";
                 using (var connection = SqlDb.UpdateConnection)
                 {
                     connection.Execute(sql, model);
                 }
             });
        }


        public void WriteApplicationExceptionLog(string message, string stackTrace)
        {
            Task.Factory.StartNew(() =>
            {
                //Thread.Sleep(20000);
                string sql = @" insert into SJApplicationExceptionLog(ExceptionMessage,StackTrace) values(@Message,@StackTrace) ;";
                using (var connection = SqlDb.UpdateConnection)
                {
                    connection.Execute(sql, new { Message = message, StackTrace = stackTrace });
                }
            });
        }

        #region 自动生成表头数据

        public void GetUserDataGridColumn(int userId, DataGrid dataGrid, int beginColumn = 0)
        {
            var headers = new DataGridManagementService().GetUserDataGridColumnLists(dataGrid.Name, userId, true);
            int frozenColumnCount = 0;
            for (int i = 0; i < headers.Count; i++)
            {
                var item = headers[i];
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = item.ColumnHeaderName;
                //dataGridTextColumn.HeaderStyle = (Style)Application.Current.Resources["DGColumnHeader"];
                dataGridTextColumn.Width = item.ColumnWidthUnitType == '*' ? new DataGridLength(item.ColumnWidth, DataGridLengthUnitType.Star) : new DataGridLength(item.ColumnWidth);

                Binding binding = new Binding() { Path = new PropertyPath(item.ColumnFieldName), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay };
                if (!string.IsNullOrEmpty(item.BindingStringFormat))
                    binding.StringFormat = item.BindingStringFormat;

                if (!string.IsNullOrEmpty(item.ConverterName))
                    binding.Converter = Application.Current.Resources[item.ConverterName] as IValueConverter;

                dataGridTextColumn.Binding = binding;
                dataGrid.Columns.Add(dataGridTextColumn);
                if (item.IsFrozenColumn)
                    frozenColumnCount = i + beginColumn;
            }
            dataGrid.FrozenColumnCount = frozenColumnCount;
        }

        #endregion

        public IEnumerable<ExportViewTypedColumnModel> GetExportViewTypedColumnUniqueValue(int groupId, int checkBoxValue, string orderedFieldName)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@groupId", groupId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@checkBoxValue", checkBoxValue, DbType.Int32, ParameterDirection.Input);
            dp.Add("@orderedFieldName", orderedFieldName, DbType.String, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ExportViewTypedColumnModel>("SJGetExportViewTypedColumn", dp, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public IList<ExportViewTypedColumnModel> GetExportViewTypedColumnWithCheckBox(int groupId)
        {
            string sql = @" select * from SJExportViewTypedColumn where IsValid=1 and ViewGroupId = @ViewGroupId";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ExportViewTypedColumnModel>(sql, new { ViewGroupId = groupId }).ToList();
            }
        }

        /// <summary>
        /// 获取Id和代码用来匹配excel导入的物料代码是否合法
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllItems()
        {
            return SqlHelper.ExecuteDataTable(@" select FItemID,FNumber,FName from t_ICItem where FDeleted=0 ;", null);
        }

        public List<ImportTemplateExcelHeaderFieldMappingModel> GetImportTemplateSJExcelHeaderFieldMappingLists(string tableName)
        {
            string sql = @" select * from SJImportTemplateExcelHeaderFieldMapping where TableName=@TableName";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ImportTemplateExcelHeaderFieldMappingModel>(sql, new { TableName = tableName }).ToList();
            }
        }

        public object ImportExcelToDatabaseTable(string fileName, string tableName)
        {
            var filedMapping = GetImportTemplateSJExcelHeaderFieldMappingLists(tableName);
            DataTable dataTable = new FileHelper().ConvertExcelToDataTable2(fileName, true, filedMapping);
            SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName);
            return dataTable.Rows[1][0];
        }

        public object ImportExcelToDatabaseTableWithSeq(string fileName, string tableName, string connectionName = "")
        {
            var filedMapping = GetImportTemplateSJExcelHeaderFieldMappingLists(tableName);
            DataTable dataTable = new FileHelper().ConvertExcelToDataTableWithSeq(fileName, true, filedMapping);
            if (connectionName == "SR")
                SqlHelper.LoadDataTableToDBModelTableSR(dataTable, tableName);
            else
                SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName);
            return dataTable.Rows[1][0];
        }

        public object ImportExcelToDatabaseTableWithoutSeq(string fileName, string tableName, string connectionName = "")
        {
            var filedMapping = GetImportTemplateSJExcelHeaderFieldMappingLists(tableName);
            DataTable dataTable = new FileHelper().ConvertExcelToDataTableWithoutSeq(fileName, true, filedMapping);

            if (connectionName == "SR")
                SqlHelper.LoadDataTableToDBModelTableSR(dataTable, tableName);
            else
                SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName);

            return dataTable.Rows[1][0];
        }

        public List<BatchTypeModel> GetBatchTypeLists()
        {
            string sql = @" select * from SJBatchTypeName ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BatchTypeModel>(sql).ToList();
            }
        }

        public string GetSqlWhereString<T>(T queryParameter)
        {
            StringBuilder sb = new StringBuilder();
            var t = queryParameter.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                var name = item.Name; // 属性名称
                var value = item.GetValue(queryParameter, null); // 属性值
                var type = value?.GetType() ?? typeof(object);//获得属性的类型
                if (type == typeof(string))
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                        sb.Append($" and {name} like '%{value}%' ");
                }
                else if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(decimal) || type == typeof(double) || type == typeof(float))
                {
                    if (value != null)
                    {
                        if (name.EndsWith("1"))
                            sb.Append($" and {name.Substring(0, name.Length - 1)} >= {value} ");
                        else if (name.EndsWith("2"))
                            sb.Append($" and {name.Substring(0, name.Length - 1)} <= {value} ");
                        else
                            sb.Append($" and {name} = {value} ");
                    }
                }
                else if (type == typeof(DateTime))
                {
                    if (value != null)
                    {
                        if (name.EndsWith("1"))
                            sb.Append($" and {name.Substring(0, name.Length - 1)} >= '{value}' ");
                        else if (name.EndsWith("2"))
                            sb.Append($" and {name.Substring(0, name.Length - 1)} <= '{value}' ");
                        else
                            sb.Append($" and {name} = '{value}' ");

                    }
                   
                }
            }
            return sb.ToString();
        }

        public string GetQueryParameterValueString<T>(T queryParameter)
        {
            StringBuilder sb = new StringBuilder();
            var t = queryParameter.GetType();
            foreach (PropertyInfo item in t.GetProperties())
            {
                var name = item.Name; // 属性名称
                var value = item.GetValue(queryParameter, null); // 属性值
                var type = value?.GetType() ?? typeof(object);//获得属性的类型
                if (value != null)
                {
                    if (type == typeof(DateTime))
                        sb.Append($"_{Convert.ToDateTime(value).Date:yyyy-MM-dd}");
                    else
                        sb.Append($"_{Convert.ToString(value)}");
                }
            }
            return sb.ToString();
        }

        public void LoadIEnumerableToDatabase<T>(IEnumerable<T> lists, string tableName)
        {
            DataTable dataTable = TypeConvertHelper.ConvertIEnumerableToDataTable(lists, DateTime.Now.Ticks);
            SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName);
        }




        public void LoadIEnumerableToDatabase2<T>(IEnumerable<T> lists, string tableName, bool dbFirst)
        {
            DataTable dataTable = lists.ListToDataTable(tableName);
            SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName, dbFirst);
        }

        public void ExecuteSqlAsync(string sql)
        {
            Task.Factory.StartNew(() =>
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@Sql", sql, DbType.String, ParameterDirection.Input);
                using (var connection = SqlDb.UpdateConnection)
                {
                    connection.ExecuteScalar("SJExecSqlReturnExceptionMessage", dp, null, null, System.Data.CommandType.StoredProcedure);
                }
            }
            );
        }

        public object ExecuteSqlAsyncReturns(string sql)
        {
            return Task.Factory.StartNew(() =>
             {
                 DynamicParameters dp = new DynamicParameters();
                 dp.Add("@Sql", sql, DbType.String, ParameterDirection.Input);
                 using (var connection = SqlDb.UpdateConnection)
                 {
                     connection.ExecuteScalar("SJExecSqlReturnExceptionMessage", dp, null, null, System.Data.CommandType.StoredProcedure);
                 }
             }
             );
        }

        public List<DeliveryStockModel> GetDeliveryStock()
        {
            string sql = @" select * from SJDeliveryStock ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<DeliveryStockModel>(sql).ToList();
            }
        }

        public string GetExportViewName(int menu,int viewId)
        {
            string sql = string.Empty;
            switch (viewId)
            {
                case 1: sql = @" select View1 from SJExportView where MainMenuId = @MainMenuId ;";break;
                case 2: sql = @" select View2 from SJExportView where MainMenuId = @MainMenuId ;"; break;
                case 3: sql = @" select View3 from SJExportView where MainMenuId = @MainMenuId ;"; break;
                default:break;
            }
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { MainMenuId  = menu }));
            }
        }

        public DataTable GetExportDataView(string viewName, string filter)
        {
            return SqlHelper.ExecuteDataTable($" select * from {viewName} where 1=1 {filter}");
        }

        public DataTable GetExportDataProcedure(string procName, int userDataId, string orderedColumns, string filter)
        {
            return SqlHelper.ExecuteDataTableProcedure(procName, new SqlParameter[] { new SqlParameter("@UserId", userDataId), new SqlParameter("@OrderColumns", orderedColumns),new SqlParameter("@Filter", filter) });
        }
    }
}
