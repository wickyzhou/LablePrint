using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace Dal
{
    public class LabelPrintDAL
    {


        /// <summary>
        /// 主页面 Datagrid数据
        /// </summary>
        public List<LabelPrintHistoryModel> GetLabelPrintHistoryDataByDate(DateTime date, int userId)
        {
            //A.ID=C.LabelPrintHistoryID

            //DataTable data = SqlHelper.ExecuteDataTableProcedure(@"SJGetPrintData"
            //                , new SqlParameter[] { new SqlParameter("@ProductionDate", date), new SqlParameter("@UserID", userId) });

            DataTable data = SqlHelper.ExecuteDataTable(@"SELECT A.ModifyTime,A.ID,A.ProductiveTaskListID,A.WorkNo,A.BatchNo,A.ProductionModel,A.ProductionName,A.ProductionDate,A.ExpirationDate,A.ExpirationMonth
                                                                ,A.OrgID,A.Label,A.OrgCode,A.OrgBillNo,A.Package
                                                                ,A.RoughWeight,A.NetWeight,A.CheckNo,A.PrintCount,A.TwoDimensionCode,A.SpecialRequest,A.BucketCount,A.CaseName,A.RowHashValue,A.Seq
                                                                ,CAST(CASE WHEN C.ID IS NULL  THEN 0 ELSE 1 END AS BIT) IsChecked
                                                                ,CASE WHEN C.ID IS NULL THEN 0 ELSE 1 END Selected
                                                               , isnull(B.BatchTotal, 0) BatchTotal,isnull(B.BatchReprintCount, 0) BatchReprintCount,isnull(B.BatchCurrentSeq, 0) BatchCurrentSeq
																,isnull(M.WorkTotal, 0)  WorkTotal,isnull(M.WorkPrintCount, 0)    WorkPrintCount ,isnull(M.WorkReprintCount, 0)    WorkReprintCount ,M.LastPrintTime,A.SafeCode
                                                              FROM SJLabelPrintHistory A
                                                           LEFT JOIN SJPrintLogBatchView B ON A.BatchNo = B.BatchNo
                                                           LEFT JOIN SJPrintLogBatchWorkView M on A.BatchNo = m.BatchNo  and A.RowHashValue = m.RowHashValue
                                                           LEFT JOIN(SELECT* FROM SJLabelPrintResult WHERE UserID= @UserID AND PrintStatus = '未打印') C ON A.ProductiveTaskListID = C.ProductiveTaskListID
                                                           WHERE A.ProductionDate = @ProductionDate "
                           , new SqlParameter[] { new SqlParameter("@ProductionDate", date), new SqlParameter("@UserID", userId) });

            return SqlHelper.DataTableToModelList<LabelPrintHistoryModel>(data);
        }


        /// <summary>
        /// 生产任务清单数据插入到打印历史表
        /// </summary>
        public int GenPrintNewDataByDate(DateTime date)
        {
            return SqlHelper.ExecuteNonQueryProcedure("SJGenPrintData", new SqlParameter[] { new SqlParameter("@ProductionDate", date) });
        }

        /// <summary>
        /// 添加打印数据到 新增数据表
        /// </summary>
        public int AddCurrentPrintData(string ids, int userID,DateTime productionDate)
        {
            return SqlHelper.ExecuteNonQueryProcedure("SJAddUserCurrentPrintData", new SqlParameter[] { new SqlParameter("@IDS", ids), new SqlParameter("@UserID", userID), new SqlParameter("@ProductionDate", productionDate) });
        }

        // 打印完成后记录日志
        public int ModifyHistoryAndCurrentData(string currentIDS, string printTime)
        {
            //string sql = " UPDATE SJLabelPrintHistory SET SelectCount+=1,SelectTotalCount+=BucketCount,PrintTime=@PrintTime WHERE ID IN(" + historyIDS + ");"

            string sql = " INSERT INTO SJLabelPrintLog(RowHashValue,PrintBucket,PrintTime,PrintUserID,BatchNo,IsReprint,LabelPrintResultID) SELECT RowHashValue,PrintCount,@PrintTime,UserID,BatchNo,case when len(seq)>0 then 1 else 0 end,ID  FROM SJLabelPrintResult WHERE ID IN(" + currentIDS + ");"
                        + " UPDATE SJLabelPrintResult SET PrintStatus='已打印' WHERE ID IN(" + currentIDS + ")";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@PrintTime", printTime) });
        }


        /// 获取未打印的历史记录ID，避免多次添加
        public List<int> GetNotPrintedUniqueKey(int userID,DateTime date)
        {
            List<int> ls = new List<int>();
            DataTable data = SqlHelper.ExecuteDataTable("  SELECT  ProductiveTaskListID FROM SJLabelPrintResult WHERE PrintStatus='未打印' AND UserID=@UserID AND ProductionDate=@ProductionDate"
                                                            , new SqlParameter[] { new SqlParameter("@UserID", userID), new SqlParameter("@ProductionDate", date) });
            foreach (DataRow row in data.Rows)
            {
                ls.Add((int)row["ProductiveTaskListID"]);
            }
            return ls;
        }


        /// 获取本人当天生成的所有数据ID，用来对批添加排重
        public List<int> GetUserUniqueKey(int userID,DateTime date)
        {
            List<int> ls = new List<int>();
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT   ProductiveTaskListID FROM SJLabelPrintResult WHERE  UserID=@UserID and ProductionDate=@ProductionDate; "
                                                        , new SqlParameter[] { new SqlParameter("@UserID", userID), new SqlParameter("@ProductionDate", date) }                    
                                                        );
            foreach (DataRow row in data.Rows)
            {
                ls.Add((int)row["ProductiveTaskListID"]);
            }
            return ls;
        }





        /// <summary>
        /// 获取打印模板字段设置值
        /// </summary>
        public IEnumerable<PrintTemplateRefModel> GetPrintTemplateRef()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT ID,ModuleName,ModuleTable,TemplateFieldName ,TemplateFieldDesc,Example   FROM SJPrintTemplateRef ", null);
            return SqlHelper.DataTableToModelList<PrintTemplateRefModel>(data);
        }


        /// 更新标签历史数据
        public int ModifyLastPrintBucketByID(int id, int count, string seq)
        {
            return SqlHelper.ExecuteNonQuery(" UPDATE SJLabelPrintHistory SET PrintCount=@BucketCount,Seq=@Seq WHERE ID=@ID"
                , new SqlParameter[] { new SqlParameter("@BucketCount", count), new SqlParameter("@ID", id), new SqlParameter("@Seq", seq) });
        }

        public int AddQuerySchemaEntry(QuerySchemaEntryModel model)
        {
            return SqlHelper.ExecuteNonQuery(@" insert into SJUserQuerySchemaEntry (SchemaId,OrgId,Label,BatchNo,ProductionModel,IsConditionOut,SafeCode) values(@SchemaId,@OrgId,@Label,@BatchNo,@ProductionModel,@IsConditionOut,@SafeCode);",
                      new SqlParameter[] { new SqlParameter("@SchemaId", model.SchemaId) ,
                                              new SqlParameter("@OrgId", model.OrgId) ,
                                              new SqlParameter("@Label", model.Label) ,
                                              new SqlParameter("@BatchNo", model.BatchNo) ,
                                              new SqlParameter("@ProductionModel", model.ProductionModel),
                                              new SqlParameter("@IsConditionOut", model.IsConditionOut),
                                              new SqlParameter("@SafeCode", model.SafeCode)
                      });
        }

        public IEnumerable<QuerySchemaDynamicBtnModel> GetSchemaDynamicBtnByUserId(int userId)
        {

            DataTable data = SqlHelper.ExecuteDataTable(@" select a.Id,UserId,SchemaSeq,SchemaName,Content,MarginLeft,MarginTop,MarginRight,MarginBottom,TemplateFullName  "
                                                        + " from SJUserQuerySchema a  join  SJUserQuerySchemaSeq b on a.SchemaSeq=b.Id where a.UserId=@UserId",
                new SqlParameter[] { new SqlParameter("@UserId", userId) }); ;
            return SqlHelper.DataTableToModelList<QuerySchemaDynamicBtnModel>(data);

        }


        //特殊与通用调整修改后，后台重新生成打印数据
        public void ReGenPrintData()
        {
            SqlHelper.ExecuteScalarProcedure("SJGenPrintData", null);
        }

        public int GetBatchPrintTotal(string batchNo)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(" Select SUM(PRINTBUCKET) AS PrintTotalLog FROM dbo.SJLabelPrintLog WHERE IsReprint=0 and BatchNo=@BatchNo GROUP BY  BatchNo"
                  , new SqlParameter[] { new SqlParameter("@BatchNo", batchNo) }));
        }



        #region 打印记录页面相关

        // 按日期一次性获取打印数据
        public IEnumerable<LabelPrintCurrencyModel> GetCurrencyPrintingDataByDate(DateTime date)
        {
            DataTable data = SqlHelper.ExecuteDataTable("  SELECT * FROM SJLabelPrintResult where ProductionDate=@ProductionDate ;", new SqlParameter[] { new SqlParameter("@ProductionDate ", date) });
            return SqlHelper.DataTableToModelList<LabelPrintCurrencyModel>(data);
        }



        // 查看该用户的所有打印结果记录
        public IEnumerable<LabelPrintCurrencyModel> GetPrintResultRecordByUserID(int userID, string pringStatus)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM SJLabelPrintResult  WHERE UserID=@UserID;", new SqlParameter[] { new SqlParameter("@UserID ", userID) });
            return SqlHelper.DataTableToModelList<LabelPrintCurrencyModel>(data);
        }

        // 查看该用户某个打印状态记录
        public IEnumerable<LabelPrintCurrencyModel> GetPrintResultRecord(DateTime date, int userID, string pringStatus)
        {
            ObservableCollection<LabelPrintCurrencyModel> list = new ObservableCollection<LabelPrintCurrencyModel>();

            DataTable data = SqlHelper.ExecuteDataTable(" SELECT A.*,B.F_107 SafeCode ,B.F_108 DangerousIngredient, B.F_109 DangerousComment FROM SJLabelPrintResult A left join t_ICItemCustom B on A.FItemID=B.FItemID WHERE UserID=@UserID AND PrintStatus =@PrintStatus  AND ProductionDate=@ProductionDate;"
                                                        , new SqlParameter[] { new SqlParameter("@ProductionDate ", date), new SqlParameter("@UserID ", userID), new SqlParameter("@PrintStatus", pringStatus) });
            return SqlHelper.DataTableToModelList<LabelPrintCurrencyModel>(data);
        }

        //获取用户模板查询明细
        public IEnumerable<QuerySchemaEntryModel> GetQuerySchemaEntryBySchemaId(int schemaId)
        {
            DataTable data = SqlHelper.ExecuteDataTable("  SELECT * FROM SJUserQuerySchemaEntry where SchemaId=@SchemaId ;", new SqlParameter[] { new SqlParameter("@SchemaId ", schemaId) });
            return SqlHelper.DataTableToModelList<QuerySchemaEntryModel>(data);
        }


        // 批量插入到数据库
        public void BatchInsertSchema(DataTable dt, string destinationTableName)
        {
            SqlHelper.LoadDataTableToDBModelTable(dt, destinationTableName);
        }


        public int SavePrintSchemaParameter(PrintSchemaParameterModel model)
        {
            return SqlHelper.ExecuteNonQuery(" insert into SJPrintSchemaParameterModel(UserId,SchemaId,TemplateFullName,TemplateFileName,Orientation,PrinterName,FolderPath) values(@UserId,@SchemaId,@TemplateFullName,@TemplateFileName,@Orientation,@PrinterName,@FolderPath) "
                , new SqlParameter[] {  new SqlParameter("@UserId", model.UserId),
                                        new SqlParameter("@SchemaId", model.SchemaId),
                                        new SqlParameter("@TemplateFullName", model.TemplateFullName),
                                        new SqlParameter("@TemplateFileName", model.TemplateFileName),
                                        new SqlParameter("@Orientation", model.Orientation),
                                        new SqlParameter("@PrinterName", model.PrinterName),
                                         new SqlParameter("@FolderPath", model.FolderPath)
                }

             );
        }


        public List<PrintSchemaParameterModel> GetPrintSchemaParameter(int userId, int schemaId)
        {
            DataTable data = SqlHelper.ExecuteDataTable("  SELECT * FROM SJPrintSchemaParameterModel where SchemaId=@SchemaId  and UserId=@UserId;"
                    , new SqlParameter[] { new SqlParameter("@SchemaId", schemaId), new SqlParameter("@UserId", userId) });
            return SqlHelper.DataTableToModelList<PrintSchemaParameterModel>(data);
        }

        public int DeletePrintLog(string ids)
        {
            string sql = " delete from SJLabelPrintLog where LabelPrintResultID in ( " + @ids + ")";
            return SqlHelper.ExecuteNonQuery(sql,null);
        }


        /// <summary>
        /// 删除当前打印数据
        /// </summary>
        public int DeleteCurrentPrintData(int id)
        {
            return SqlHelper.ExecuteNonQuery(" DELETE FROM SJLabelPrintResult WHERE ID =@ID;"
                                       , new SqlParameter[] { new SqlParameter("@ID", id) });
        }

        public int BatchDeleteCurrentPrintData(DateTime date, int userId)
        {
            return SqlHelper.ExecuteNonQuery(" DELETE FROM SJLabelPrintResult WHERE ProductionDate= @ProductionDate AND UserID=@UserID AND PrintStatus='未打印';"
                , new SqlParameter[] { new SqlParameter("@ProductionDate", date), new SqlParameter("@UserID", userId) });
        }

        public int BatchDeleteCurrentPrintData(string ids)
        {
            string sql = "  DELETE FROM SJLabelPrintResult WHERE ID in ("+ids+");";
            return SqlHelper.ExecuteNonQuery(sql, null);
        }

        public void ModifyCurrency(int id, int printOrder)
        {
            SqlHelper.ExecuteNonQuery(" update SJLabelPrintResult set PrintOrder= @PrintOrder where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", id), new SqlParameter("@PrintOrder", printOrder) });
        }

        #endregion

        #region 通用调整界面

        public IEnumerable<LabelPrintCommonAdjustmentModel> GetAllLabelPrintCommonAdjustment()
        {

            DataTable data = SqlHelper.ExecuteDataTable(@" SELECT * FROM SJPrintCustLabelCommonAdjustment ORDER BY ID DESC ; ");
            return SqlHelper.DataTableToModelList<LabelPrintCommonAdjustmentModel>(data);
        }

        public int DeleteCommonAdjustment(int id)
        {
            return SqlHelper.ExecuteNonQuery(" DELETE FROM SJPrintCustLabelCommonAdjustment WHERE ID=@ID", new SqlParameter[] { new SqlParameter("@ID", id) });
        }

        public object AddCommonAdjustment(LabelPrintCommonAdjustmentModel model)
        {
            return SqlHelper.ExecuteScalar(@" INSERT INTO  dbo.SJPrintCustLabelCommonAdjustment(OrgID,Label,ProductionName,ExpirationMonth,ExpirationDay,IdentityType,IdentityTypeDesc,NetWeight) 
                                                 VALUES( @OrgID,@Label,@ProductionName,@ExpirationMonth,@ExpirationDay,@IdentityType,@IdentityTypeDesc,@NetWeight );  
                                                SELECT @@IDENTITY AS R"
                     , new SqlParameter[] {
                        new SqlParameter("@OrgID", model.OrgID),
                        new SqlParameter("@Label", model.Label),
                        new SqlParameter("@ProductionName", SqlHelper.ConvertNullableStringToDbValue(model.ProductionName)),
                        new SqlParameter("@ExpirationMonth",SqlHelper.ConvertNullableIntToDbValue(model.ExpirationMonth)),
                        new SqlParameter("@ExpirationDay",SqlHelper.ConvertNullableIntToDbValue(model.ExpirationDay)),
                        new SqlParameter("@IdentityType", model.IdentityType),
                        new SqlParameter("@IdentityTypeDesc", model.IdentityTypeDesc),
                        new SqlParameter("@NetWeight", model.NetWeight)
                 });
        }

        public int UpdateCommonAdjustment(LabelPrintCommonAdjustmentModel model)
        {
            return SqlHelper.ExecuteNonQuery(@" Update  dbo.SJPrintCustLabelCommonAdjustment "
                                               + " SET OrgID=@OrgID, "
                                               + "    Label=@Label, "
                                               + "    ProductionName =@ProductionName, "
                                               + "    ExpirationMonth =@ExpirationMonth, "
                                               + "    ExpirationDay =@ExpirationDay, "
                                               + "    IdentityType =@IdentityType, "
                                               + "    IdentityTypeDesc =@IdentityTypeDesc ,"
                                               + "    NetWeight =@NetWeight "
                                               + " WHERE ID=@ID"
               , new SqlParameter[] {
                        new SqlParameter("@ID", model.ID),
                        new SqlParameter("@OrgID", model.OrgID),
                        new SqlParameter("@Label", model.Label),
                        new SqlParameter("@ProductionName", SqlHelper.ConvertNullableStringToDbValue(model.ProductionName)),
                        new SqlParameter("@ExpirationMonth",SqlHelper.ConvertNullableIntToDbValue(model.ExpirationMonth)),
                        new SqlParameter("@ExpirationDay",SqlHelper.ConvertNullableIntToDbValue(model.ExpirationDay)),
                        new SqlParameter("@IdentityType", model.IdentityType),
                        new SqlParameter("@IdentityTypeDesc", model.IdentityTypeDesc),
                        new SqlParameter("@NetWeight", model.NetWeight)
           });
        }

        //public LabelPrintCommonAdjustmentModel ToModel(DataRow row)
        //{

        //    return new LabelPrintCommonAdjustmentModel
        //    {
        //        ID = (int)row["ID"],
        //        OrgID = (string)row["OrgID"],
        //        Label = (string)row["Label"],
        //        ProductionName = (string)row["ProductionName"],
        //        ExpirationMonth = SqlHelper.SetString(row["ExpirationMonth"]),
        //        ExpirationDay = SqlHelper.SetInt(row["ExpirationDay"]),
        //        CbIdentTypeModel = new CbIdentTypeModel { IdentityType =(int)row["IdentityType"] , IdentityTypeDesc = (string)row["IdentityTypeDesc"] }  
        //    };

        //}

        #endregion


        #region 特殊要求界面
        public IEnumerable<LabelPrintSpecialRequestModel> GetAllSpecialRequest()
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select * from  SJPrintCustLabelSpecialRequest	", null);
            return SqlHelper.DataTableToModelList<LabelPrintSpecialRequestModel>(data);
        }

        public int AddEnumModel(EnumModel model)
        {
            return SqlHelper.ExecuteNonQuery(" insert into  SJEnumTable(GroupSeq,GroupName,ItemSeq,ItemValue,ParentGroupSeq) values(@GroupSeq,@GroupName,@ItemSeq,@ItemValue,@ParentGroupSeq)"
                , new SqlParameter[] { new SqlParameter("@GroupSeq",model.GroupSeq),
                                        new SqlParameter("@GroupName",model.GroupName),
                                        new SqlParameter("@ItemSeq",model.ItemSeq),
                                        new SqlParameter("@ItemValue",model.ItemValue),
                                        new SqlParameter("@ParentGroupSeq",model.ParentGroupSeq) });
        }

        public int UpdateLabelPrintSpecialRequestModel(int id, string value, string label)
        {
            return SqlHelper.ExecuteNonQuery(" update SJPrintCustLabelSpecialRequest  set RequestValue=@RequestValue,Label=@Label where id=@id ;"
                , new SqlParameter[] { new SqlParameter("@id",id),
                                        new SqlParameter("@RequestValue",SqlHelper.ConvertNullableStringToDbValue(value)),
                                        new SqlParameter("@Label",SqlHelper.ConvertNullableStringToDbValue(label))

                });
        }

        public object AddSpecialRequest(LabelPrintSpecialRequestModel model)
        {
            return SqlHelper.ExecuteScalar(@" insert into  SJPrintCustLabelSpecialRequest(OrgID,Label,RequestSeq,RequestName,RequestValue,IdentityType,IdentityTypeDesc) 
                                              values(@OrgID,@Label,@RequestSeq,@RequestName,@RequestValue,@IdentityType,@IdentityTypeDesc); 
                                                SELECT @@IDENTITY AS R;"
                    , new SqlParameter[] {
                        new SqlParameter("@OrgID", model.OrgID),
                        new SqlParameter("@Label", model.Label),
                        new SqlParameter("@RequestSeq",model.RequestSeq ),
                        new SqlParameter("@RequestName",model.RequestName),
                        new SqlParameter("@RequestValue",SqlHelper.ConvertNullableStringToDbValue(model.RequestValue)),
                        new SqlParameter("@IdentityType", model.IdentityType),
                        new SqlParameter("@IdentityTypeDesc", model.IdentityTypeDesc),
                        //new SqlParameter("@IsFixedValue", model.IsFixedValue)
                });
        }

        public int DeleteSpecialRequest(int id)
        {
            return SqlHelper.ExecuteNonQuery(" DELETE FROM SJPrintCustLabelSpecialRequest WHERE ID=@ID", new SqlParameter[] { new SqlParameter("@ID", id) });
        }
        #endregion

        #region 查询方案
        public IEnumerable<QuerySchemaModel> GetAllSchemaName(int userId)
        {
            DataTable data = SqlHelper.ExecuteDataTable("  SELECT * FROM SJUserQuerySchema where UserId=@UserId", new SqlParameter[] { new SqlParameter("@UserId", userId) });
            return SqlHelper.DataTableToModelList<QuerySchemaModel>(data);
        }

        public object AddQuerySchemaName(QuerySchemaModel model)
        {
            return SqlHelper.ExecuteScalar(@" insert into  SJUserQuerySchema(UserId,SchemaSeq,SchemaName) 
                                              values(@UserId,@SchemaSeq,@SchemaName);
                                                SELECT @@IDENTITY AS R;",
                                              new SqlParameter[] {     new SqlParameter("UserId", model.UserId),
                        new SqlParameter("@SchemaSeq", model.SchemaSeq),
                        new SqlParameter("@SchemaName", model.SchemaName )});
        }

        public int UpdateQuerySchemaName(int id, string name)
        {
            return SqlHelper.ExecuteNonQuery(@" update SJUserQuerySchema set SchemaName=@SchemaName where Id=@Id ;",
                                             new SqlParameter[] {     new SqlParameter("SchemaName", name),
                        new SqlParameter("@Id", id )});
        }

        public int DeleteQuerySchema(int id)
        {
            return SqlHelper.ExecuteNonQuery(@" delete from SJUserQuerySchemaEntry where SchemaId=@Id  ; delete from SJUserQuerySchema where Id=@Id ;",
                       new SqlParameter[] { new SqlParameter("@Id", id) });
        }

        public IEnumerable<QuerySchemaModel> GetMySchema(int userId)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select * from  SJUserQuerySchema where UserId=@UserId;	", new SqlParameter[] { new SqlParameter("@UserId", userId) });
            return SqlHelper.DataTableToModelList<QuerySchemaModel>(data);
        }

        public IEnumerable<QuerySchemaConfigModel> GetSchemaEntryByUserId(int userId)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select a.UserId,a.Id BtnN,a.SchemaSeq,a.SchemaName,b.Id,b.OrgId,b.Label,b.BatchNo,b.ProductionModel,b.IsConditionOut,c.ContentTrans,b.SafeCode,b.CreateTime from SJUserQuerySchema a  join  SJUserQuerySchemaEntry b on a.Id=b.SchemaId join SJUserQuerySchemaSeq c on c.Id=a.SchemaSeq   where a.UserId=@UserId;",
                new SqlParameter[] { new SqlParameter("@UserId", userId) });
            return SqlHelper.DataTableToModelList<QuerySchemaConfigModel>(data);
        }

        public IEnumerable<QuerySchemaConfigModel> GetSchemaEntryByUserIdAndType(int userId, bool isConditionOut)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" select a.UserId,a.Id BtnN,a.SchemaSeq,a.SchemaName,b.Id,b.OrgId,b.Label,b.BatchNo,b.ProductionModel,b.IsConditionOut,c.ContentTrans from SJUserQuerySchema a  join  SJUserQuerySchemaEntry b on a.Id=b.SchemaId join SJUserQuerySchemaSeq c on c.Id=a.SchemaSeq   where a.UserId=@UserId    and IsConditionOut = @IsConditionOut ;",
                new SqlParameter[] { new SqlParameter("@UserId", userId), new SqlParameter("@IsConditionOut", isConditionOut) });
            return SqlHelper.DataTableToModelList<QuerySchemaConfigModel>(data);
        }

        public int DeleteQuerySchemaEntry(int entryId)
        {
            return SqlHelper.ExecuteNonQuery(@" delete from SJUserQuerySchemaEntry where Id=@Id ;",
                       new SqlParameter[] { new SqlParameter("@Id", entryId) });
        }

        #endregion
    }
}
