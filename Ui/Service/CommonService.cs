using Common;
using Dal;
using Dapper;
using Microsoft.ReportingServices.DataProcessing;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
            string sql = @"select * from SJBarTenderPrintConfig where TemplateTypeId=@TemplateTypeId and HostName=@HostName and UserId=@UserId ";
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
                return connection.Query<BarTenderPrintConfigModelXX, BarTenderTemplateModel, BarTenderPrintConfigModelXX>(sql, (c, t) => { c.ExpressTemplateSelectedItem = t; return c; }
              , new { UserId = userId, TemplateTypeId = typeId, HostName = hostName }, splitOn: "TemplatePerPage"
              ).FirstOrDefault();
            }
        }

        public int InsertBarTenderPrintConfigXX(BarTenderPrintConfigModelXX model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@HostName", model.HostName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTypeId", model.TemplateTypeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateTypeName", model.TemplateTypeName, DbType.String, ParameterDirection.Input);
            dp.Add("@PrinterName", model.PrinterName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplatePerPage", model.ExpressTemplateSelectedItem.TemplatePerPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateFullName", model.ExpressTemplateSelectedItem.TemplateFullName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFileName", model.ExpressTemplateSelectedItem.TemplateFileName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFolderPath", model.ExpressTemplateSelectedItem.TemplateFolderPath, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTotalPage", model.ExpressTemplateSelectedItem.TemplateTotalPage, DbType.Int32, ParameterDirection.Input);

            string sql = @" insert into SJBarTenderPrintConfig(UserId,HostName,TemplateTypeId,TemplateTypeName,TemplateFullName,TemplateFileName,TemplatePerPage,PrinterName,TemplateFolderPath,TemplateTotalPage)
                            values(@UserId,@HostName,@TemplateTypeId,@TemplateTypeName,@TemplateFullName,@TemplateFileName,@TemplatePerPage,@PrinterName,@TemplateFolderPath,@TemplateTotalPage);select SCOPE_IDENTITY() as Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToInt32(connection.ExecuteScalar(sql, dp));
            }
        }

        public bool UpdateBarTenderPrintConfigXX(BarTenderPrintConfigModelXX model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", model.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("@PrinterName", model.PrinterName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplatePerPage", model.ExpressTemplateSelectedItem.TemplatePerPage, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TemplateFullName", model.ExpressTemplateSelectedItem.TemplateFullName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFileName", model.ExpressTemplateSelectedItem.TemplateFileName, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateFolderPath", model.ExpressTemplateSelectedItem.TemplateFolderPath, DbType.String, ParameterDirection.Input);
            dp.Add("@TemplateTotalPage", model.ExpressTemplateSelectedItem.TemplateTotalPage, DbType.Int32, ParameterDirection.Input);

            string sql = @" update SJBarTenderPrintConfig set PrinterName=@PrinterName
                            ,TemplateFullName=@TemplateFullName,TemplateFileName=@TemplateFileName,TemplatePerPage=@TemplatePerPage,TemplateFolderPath=@TemplateFolderPath,TemplateTotalPage=@TemplateTotalPage where Id=@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, dp) > 0;
            }
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
            var headers = new DataGridManagementService().GetUserDataGridColumnLists(dataGrid.Name, userId);
            foreach (var item in headers)
            {
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
                dataGrid.Columns.Insert(beginColumn, dataGridTextColumn);
                //bool r = false;
                //string sql = @" select 1 from SJDataGridUserCustom where DataGridName=@DataGridName  and UserId = @UserId ;";

                //using (var connection = SqlDb.UpdateConnection)
                //{
                //    r = connection.Execute(sql, new { DataGridName = dataGrid.Name, UserId = userId }) > 0;
                //}

                //if (r)
                //    GetDataGridColumnHeaderCustom(userId, dataGrid, beginColumn);
                //else
                //    GetDataGridColumnHeaderDefault(dataGrid, beginColumn);
            }

        }

        ///// <summary>
        ///// 获取默认表格配置
        ///// </summary>
        ///// <param name="dataGrid">表格</param>
        ///// <param name="beginColumn">开始列</param>
        //public void GetDataGridColumnHeaderDefault(DataGrid dataGrid, int beginColumn = 0)
        //{

        //    List<DataGridColumnHeaderModel> headers;

        //    string sql = @" select * from SJDataGridColumnHeader where DataGridName=@DataGridName  and ColumnVisibility=1 order by ColumnOrder desc ;";

        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        headers = connection.Query<DataGridColumnHeaderModel>(sql, new { DataGridName = dataGrid.Name }).ToList();
        //    }

        //    DataGridTextColumnInit(dataGrid, headers, beginColumn);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataGrid">界面的控件DataGrid</param>
        ///// <param name="headers">数据列对应的表头名称</param>
        ///// <param name="beginColumn">从第几列开始，动态生成，可以设置【数据列】相对【模板列】前后位置</param>
        //private void DataGridTextColumnInit(DataGrid dataGrid, IList<DataGridColumnHeaderModel> headers, int beginColumn)
        //{
        //    foreach (var item in headers)
        //    {
        //        DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
        //        dataGridTextColumn.Header = item.ColumnHeaderName;
        //        //dataGridTextColumn.HeaderStyle = (Style)Application.Current.Resources["DGColumnHeader"];
        //        dataGridTextColumn.Width = item.ColumnWidthUnitType == '*' ? new DataGridLength(item.ColumnWidth, DataGridLengthUnitType.Star) : new DataGridLength(item.ColumnWidth);

        //        Binding binding = new Binding() { Path = new PropertyPath(item.ColumnFieldName), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay };
        //        if (!string.IsNullOrEmpty(item.BindingStringFormat))
        //            binding.StringFormat = item.BindingStringFormat;

        //        if (!string.IsNullOrEmpty(item.ConverterName))
        //            binding.Converter = Application.Current.Resources[item.ConverterName] as IValueConverter;

        //        dataGridTextColumn.Binding = binding;
        //        dataGrid.Columns.Insert(beginColumn, dataGridTextColumn);
        //    }
        //}


        ///// <summary>
        ///// 获取用户自定义表格配置
        ///// </summary>
        ///// <param name="dataGrid">表格</param>
        ///// <param name="beginColumn">开始列</param>
        //public void GetDataGridColumnHeaderCustom(int userId, DataGrid dataGrid, int beginColumn = 0)
        //{

        //    List<DataGridColumnHeaderUserCustomModel> headers;

        //    string sql = @" select * from SJDataGridUserCustom where DataGridName=@DataGridName  and UserId = @UserId order by   ColumnOrder desc ;";

        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        headers = connection.Query<DataGridColumnHeaderUserCustomModel>(sql, new { DataGridName = dataGrid.Name, UserId = userId }).ToList();
        //    }

        //    DataGridTextColumnInitCustom(dataGrid, headers, beginColumn);
        //}


        //private void DataGridTextColumnInitCustom(DataGrid dataGrid, IList<DataGridColumnHeaderUserCustomModel> headers, int beginColumn)
        //{
        //    foreach (var item in headers)
        //    {
        //        DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
        //        dataGridTextColumn.Header = item.ColumnHeaderName;
        //        //dataGridTextColumn.HeaderStyle = (Style)Application.Current.Resources["DGColumnHeader"];
        //        dataGridTextColumn.Width = item.ColumnWidthUnitType == '*' ? new DataGridLength(item.ColumnWidth, DataGridLengthUnitType.Star) : new DataGridLength(item.ColumnWidth);

        //        Binding binding = new Binding() { Path = new PropertyPath(item.ColumnFieldName), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay };
        //        if (!string.IsNullOrEmpty(item.BindingStringFormat))
        //            binding.StringFormat = item.BindingStringFormat;

        //        if (!string.IsNullOrEmpty(item.ConverterName))
        //            binding.Converter = Application.Current.Resources[item.ConverterName] as IValueConverter;

        //        dataGridTextColumn.Binding = binding;
        //        dataGrid.Columns.Insert(beginColumn, dataGridTextColumn);
        //    }
        //}


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
                else if (type == typeof(int) || type == typeof(double) )
                {
                    if (value != null)
                    {
                        if (name.EndsWith("Begin"))
                            sb.Append($" and {name.Replace("Begin", "")} >= {value} ");
                        else if (name.EndsWith("End"))
                            sb.Append($" and {name.Replace("End", "")} <= {value} ");
                        else
                            sb.Append($" and {name} = {value} ");
                    }
                }
                else if (type == typeof(DateTime))
                {
                    if (name.EndsWith("Begin"))
                        sb.Append($" and {name.Replace("Begin", "")} >= '{value}' ");
                    else if (name.EndsWith("End"))
                        sb.Append($" and {name.Replace("End", "")} <= '{value}' ");
                    else
                        sb.Append($" and {name} = '{value}' ");
                }
            }
            return sb.ToString();
        }

        public void LoadIEnumerableToDatabase<T>(IEnumerable<T> lists,string tableName)
        {
            DataTable dataTable = TypeConvertHelper.ConvertIEnumerableToDataTable(lists,DateTime.Now.Ticks);
            SqlHelper.LoadDataTableToDBModelTable(dataTable, tableName);
        }
    }
}
