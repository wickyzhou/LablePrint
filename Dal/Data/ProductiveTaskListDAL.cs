using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dal
{
    public class ProductiveTaskListDAL
    {
        public List<ProductiveTaskListModel> GetAllProductiveTaskList(DateTime productionDate, string type)
        {
            DataTable data= SqlHelper.ExecuteDataTable(" SELECT * FROM SJICMOList WHERE FProductionDate=@ProductionDate AND FType=@Type  ORDER BY FBatchNo,FOrgID"
                , new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate), new SqlParameter("@Type", type) });
            return SqlHelper.DataTableToModelList<ProductiveTaskListModel>(data);
        }

        public List<ProductiveTaskListModel> GetAllProductiveTaskListByDate(DateTime productionDate)
        {
            DataTable data = SqlHelper.ExecuteDataTable(" SELECT * FROM SJICMOList WHERE FProductionDate=@ProductionDate AND FType<>'其他' ORDER BY FBatchNo,FOrgID"
                , new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate) });
            return SqlHelper.DataTableToModelList<ProductiveTaskListModel>(data);
        }

        public object SyncProductiveTaskList(DateTime productionDate)
        {
            return SqlHelper.ExecuteScalarProcedure("SJGenICMOListVOC", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate) });
        }

        public int ModifyProductiveTaskList(ProductiveTaskListModel model)
        {
            return 1;//  SqlHelper.ExecuteNonQuery("SJGenICMOList", new SqlParameter[] { new SqlParameter("@ProductionDate", model.FProductionDate) });
        }

        public List<ProductionClassModel> GetAllType()
        {
            DataTable data = SqlHelper.ExecuteDataTable("SELECT * FROM SJProductionType ", null);
            return SqlHelper.DataTableToModelList<ProductionClassModel>(data);
        }

        public void ImportDataTableSync(DataTable dataTable)
        {
            DateTime productionDate=Convert.ToDateTime(dataTable.Rows[0]["FProductionDate"].ToString()).Date;
            string type = dataTable.Rows[0]["FType"].ToString();
            //SqlHelper.ExecuteNonQuery(" DELETE FROM SJICMOList WHERE FProductionDate=@ProductionDate AND FType=@FType", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate), new SqlParameter("@FType",type) });

            //导入数据到临时表SJICMOListImport
            SqlHelper.LoadDataTableToDBModelTable(dataTable, "SJICMOListImport");

            //同步SJICMOListImport数据到SJICMOList
            SqlHelper.ExecuteNonQuery(@" UPDATE A SET A.FitemName=B.FitemName,A.FBatchNo=B.FBatchNo,A.FQuantity=B.FQuantity,A.FHasSmallMaterial=B.FHasSmallMaterial
                                                        , A.FPackage = B.FPackage, A.FBucketName = B.FBucketName, A.FOrgID = B.FOrgID, A.FLabel = B.FLabel, A.FBillNo = B.FBillNo, A.FNote = B.FNote,A.FAuditTip='已审核'
                                         FROM SJICMOList A JOIN SJICMOListImport B   ON A.ID = B.ID ; TRUNCATE TABLE SJICMOListImport;", null);

            ////生成打印数据
            //AuditProductiveTaskList(productionDate);
        }

        public object AuditProductiveTaskList(DateTime productionDate)
        {
            return SqlHelper.ExecuteScalarProcedure("SJGenPrintDataVOC", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate) });
        }

        public void ClearIncrement()
        {
            SqlHelper.ExecuteNonQuery(@" TRUNCATE TABLE SJWorkNoHashHistory; TRUNCATE TABLE SJSplitWorkNoResultTableLast;", null);
        }

        public object ModifyBillDateMonthly(string batches, DateTime productionDate, DateTime newDate, int userId)
        {
            return SqlHelper.ExecuteScalarProcedure("SJModifyBillDateMonthly",
                new SqlParameter[] { new SqlParameter("@Batches", batches),
                   new SqlParameter("@ProductionDate", productionDate),
                   new SqlParameter("@NewDate", newDate),
                   new SqlParameter("@UserId", userId) });
        }

        public string VerifyICMOOrder(DateTime productionDate)
        {
            return  Convert.ToString(SqlHelper.ExecuteScalarProcedure("SJICMOOrderVerificationProc", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate) }));
        }
        
    }
}
