using Dapper;
using Model;
using QueryParameterModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ui.Helper;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SalesRebateService
    {

        /// <summary>
        /// 修改最近使用参数值
        /// </summary>
        /// <param name="salesRebateModel"></param>
        /// <returns></returns>
        public bool RecentParameterUpdate(SalesRebateRecentParameterMainModel salesRebateModel)
        {
            string sql = @" update SJSalesRebateRecentParameterMain set RebatePctValue = @RebatePctValue, RebatePctType = @RebatePctType
                                ,TaxAmountType = @TaxAmountType, MinusLastPeriodRebateType = @MinusLastPeriodRebateType, ModifyTime = getdate()
                                , AmountRangeCalculateType = @AmountRangeCalculateType,PGuid= @PGuid
                            where Id = @Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        /// <summary>
        /// Api插入数据后，更新返利结果表
        /// </summary>
        /// <param name="k3BillNo"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orgId"></param>
        /// <param name="rebateClass"></param>
        /// <returns></returns>
        public bool UpdateK3BillNo(int id,string k3BillNo, DateTime k3BillDate)
        {
            string sql = @" update SJSalesRebateSummary set K3BillNo=@K3BillNo, K3BillDate = @K3BillDate   where Id =@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { K3BillNo = k3BillNo, K3BillDate = k3BillDate, Id = id }) > 0;
            }
        }

        /// <summary>
        /// 清空界面参数
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public bool ClearRecentMainParameter(string guids)
        {
            string sql = $" update SJSalesRebateRecentParameterMain set RebatePctValue = null,RebatePctType = -1,TaxAmountType = -1" +
                $",MinusLastPeriodRebateType = -1,AmountRangeCalculateType = -1,ModifyTime = GETDATE(),PGuid = newid() where Guid in({guids});";

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 删除未生成单据的行数据 
        /// /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public bool SummaryDelete(int id,Guid guid)
        {
            string sql = $" delete from SJSalesRebateSummary where Id = @Id; delete from SJSalesRebate where SGuid= @SGuid ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,new { Id = id, SGuid=guid }) > 0;
            }
        }



        /// <summary>
        /// 获取计算结果
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SalesRebateSummaryModel> GetSalesRebateSummaryLists(int userId, string filter = "")
        {
            string userString = userId == -1 ? "" : " and UserId = @UserId ";
            //string isDeletedString = isDeleted ? "" : " and Deleted = 0 ";
            string sql = $" select  * from SJSalesRebateSummaryView where 1=1 {filter} {userString} order by Id desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateSummaryModel>(sql).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SalesRebateModel> GetSalesRebateListsByGuid(Guid guid)
        {
            string sql = $" select  * from SJSalesRebateView where SGuid = '{guid}' order by Id desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql).ToList();
            }
        }

        /// <summary>
        /// 关闭参数界面自动计算
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CalculateAmount(SalesRebateBatchParameterModel model,int userId,bool isconfig)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SettleDateBegin", model.SettleDateBegin, DbType.Date, ParameterDirection.Input);
            dp.Add("@SettleDateEnd", model.SettleDateEnd, DbType.Date, ParameterDirection.Input);
            dp.Add("@OrgId", model.OrganizationSearchedItem.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("@RebateClass", model.RebateClassSeletedItem.ItemSeq, DbType.Int32, ParameterDirection.Input);
            dp.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@IsConfig", isconfig, DbType.Boolean, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                connection.Execute("SJSalesRebateCalculateAmountProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 获取待配置的参数数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SalesRebateRecentParameterMainModel> GetSalesRebateOrgRecentParameterLists(string filter)
        {

            string sql = $" select  * from SJSalesRebateRecentParameterMainView where 1=1 {filter} order by IsPassed desc,ModifyTime desc ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateRecentParameterMainModel>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取已经插入到K3的单据上的参数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SalesRebateRecentParameterMainModel> GetSalesRebateK3RecordParameterLists(string filter)
        {

            string sql = $"   select  * from SJSalesRebateView   where SGuid in(select guid from  SJSalesRebateSummary where  K3BillNo is not NULL) {filter} order by CalculateTime desc  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateRecentParameterMainModel>(sql).ToList();
            }
        }

        /// <summary>
        /// 新开页面根据类别计算当前参数
        /// </summary>
        /// <param name="model"></param>
        public void InsertCurrentOrgRebateClassParameter(SalesRebateBatchParameterModel model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SettleDateBegin", model.SettleDateBegin, DbType.Date, ParameterDirection.Input);
            dp.Add("@SettleDateEnd", model.SettleDateEnd, DbType.Date, ParameterDirection.Input);
            dp.Add("@OrgId", model.OrganizationSearchedItem.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("@RebateClass", model.RebateClassSeletedItem.ItemSeq, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                connection.Execute("SJSalesRebateInsertCurrentOrgRebateClassParameterProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 将后选的行参数 复制到先选的行参数
        /// </summary>
        /// <param name="firstId">先选的行Id</param>
        /// <param name="lastId">后选的行Id</param>
        /// <param name="firstGuid">先选的行GUID</param>
        /// <param name="lastGuid">后选的行GUID</param>
        /// <param name="typeId">1：行间复制  2：历史记录复制</param>
        public void SalesRebateParameterCopy(int firstId, int lastId, Guid firstGuid,Guid lastGuid,int typeId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@FirstId", firstId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@LastId", lastId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@FirstGuid", firstGuid, DbType.Guid, ParameterDirection.Input);
            dp.Add("@LastGuid", lastGuid, DbType.Guid, ParameterDirection.Input);
            dp.Add("@TypeId", typeId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                connection.Execute("SJSalesRebateParameterCopyProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 获取某客户最后的返利日期
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="rebateClass"></param>
        /// <returns></returns>
        public DateTime GetMaxSettleDateByOrgId(int orgId, int rebateClass)
        {
            string sql = $" select max(SettleDateEnd) from SJSalesRebateSummary where OrgId = @OrgId  and K3BillNo is not null ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToDateTime(connection.ExecuteScalar(sql, new { OrgId = orgId, RebateClass = rebateClass }));
            }
        }

        /// <summary>
        /// 选定客户在该区间是否有数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IfExistsOrgICSaleBill(SalesRebateBatchParameterModel model)
        {
            string sql = @" select top 1 1 as r from ICSale a join ICSaleEntry b on a.FInterID = b.FInterID  
                            where a.FDate >= @SettleDateBegin and a.FDate <= @SettleDateEnd  AND a.FTranType = 80 AND a.FCancellation = 0  and FCustID = @OrgId and b.FItemID<>14331";

            using (var connection = SqlDb.UpdateConnection)
            {
                return !string.IsNullOrEmpty(Convert.ToString(connection.ExecuteScalar(sql,new { model.SettleDateBegin, model.SettleDateEnd, OrgId = model.OrganizationSearchedItem.Id}))) ;
            }
        }

        /// <summary>
        /// 获取查询报表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<SalesRebateOrgCaseReportModel> GetCaseAmountReport(SalesRebateReportQueryParameterModel model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BillDate1", model.BillDate1, DbType.Date, ParameterDirection.Input);
            dp.Add("@BillDate2", model.BillDate2, DbType.Date, ParameterDirection.Input);
            dp.Add("@OrgName", model.OrgName??"", DbType.String, ParameterDirection.Input);
            dp.Add("@CaseName", model.CaseName ?? "", DbType.String, ParameterDirection.Input);
            dp.Add("@BrandName", model.BrandName ?? "", DbType.String, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
              return  connection.Query<SalesRebateOrgCaseReportModel>("SJSalesRebateShowOrgCaseAmountProc", dp, null, true,null, CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// 获取导出的数据
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable GetExportDataProc(string procName, SalesRebateReportQueryParameterModel model)
        {
            return SqlHelper.ExecuteDataTableProcedure(procName, new SqlParameter[] 
            {   
                new SqlParameter("@BillDate1", model.BillDate1), 
                new SqlParameter("@BillDate2", model.BillDate2), 
                new SqlParameter("@OrgName", model.OrgName?? "") ,
                new SqlParameter("@CaseName", model.CaseName?? "") ,
                new SqlParameter("@BrandName", model.BrandName?? ""),

            });
        }
    }
}
