using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SalesRebateService
    {

        public bool Insert(SalesRebateModel salesRebateModel)
        {
            string sql = @" insert into SJSalesRebate(MaterialId,CaseId,OrgId,RebateClass,RebatePctValue,RebatePctType,TaxAmountType,Guid,MinusLastPeriodRebateType)
                            values(@MaterialId,@CaseId,@OrgId,@RebateClass,@RebatePctValue,@RebatePctType,@TaxAmountType,@Guid,@MinusLastPeriodRebateType) ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        public bool RecentParameterUpdate(SalesRebateModel salesRebateModel)
        {
            string sql = @" update SJSalesRebateRecentParameterMain set RebatePctValue=@RebatePctValue,RebatePctType=@RebatePctType,TaxAmountType=@TaxAmountType,MinusLastPeriodRebateType=@MinusLastPeriodRebateType,ModifyTime=getdate(),UserId=@UserId
                            where Guid = @Guid; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        public bool Update(SalesRebateModel salesRebateModel)
        {
            string sql = @" update SJSalesRebate 
	                        set RebatePctValue=@RebatePctValue,RebatePctType=@RebatePctType,TaxAmountType=@TaxAmountType,MinusLastPeriodRebateType=@MinusLastPeriodRebateType
                            where Id = @Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        public bool UpdateK3BillNo(string k3BillNo,DateTime beginDate,DateTime endDate,int orgId,int rebateClass)
        {
            string sql = @" update SJSalesRebate 
	                        set K3BillNo=@K3BillNo
                            where SettleDateBegin = @SettleDateBegin and SettleDateEnd = @SettleDateEnd and OrgId = @OrgId and RebateClass = @RebateClass ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { K3BillNo = k3BillNo, SettleDateBegin = beginDate, SettleDateEnd = endDate, OrgId = orgId, RebateClass = rebateClass }) > 0;
            }
        }

        public bool RecentMainParameterDelete(int id,Guid guid)
        {
            string sql = $" delete from SJSalesRebateRecentParameterSon where Guid=@Guid; delete from SJSalesRebateRecentParameterMain where Id=@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id, Guid = guid }) > 0;
            }
        }

        public bool BatchDelete(string guids)
        {
            string sql = $" update SJSalesRebate set Deleted = 1 where Guid in ({guids});";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }


        public bool DiskBatchDelete(string guids)
        {
            string sql = $" delete from  SJSalesRebate  where Guid in ({guids}); " +
                         $" delete from SJSalesRebateAmountRange where Guid in ({guids}); ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        public bool Copy(Guid oldGuid,Guid newGuid)
        {
            string sql = @" insert into SJSalesRebateAmountRange(EffectiveDate,ExpirationDate,IsValid,Guid,AmountLower,AmountUpper,SalesRebatePctValue)
                            select EffectiveDate,ExpirationDate,IsValid,@NewGuid,AmountLower,AmountUpper,SalesRebatePctValue
                            from SJSalesRebateAmountRange where Guid=@OldGuid and IsValid=1;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,new { OldGuid = oldGuid , NewGuid = newGuid }) > 0;
            }
        }



        public List<SalesRebateModel> GetSalesRebateLists(int userId, bool isDeleted,string filter="")
        {
            string userString = userId == -1 ? "" : " and UserId = @UserId ";
            string isDeletedString = isDeleted ? "" : " and Deleted = 0 ";
            string sql = $" select  * from SJSalesRebateView where ComputeRebateAmout > 0  {filter} {userString} {isDeletedString} order by Id desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql).ToList();
            }
        }

        public SalesRebateModel GetSalesRebate(Guid guid)
        {
            string sql = @" select  * from SJSalesRebateView where Guid=@Guid";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql,new { Guid = guid }).FirstOrDefault();
            }
        }


        public void ReCalculateSalesRebateAmount(DateTime beginDate,DateTime endDate)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BeginDate", beginDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@EndDate", endDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                 connection.ExecuteScalar("SJCalculateSalesRebateAmountProc", dp, null, null, CommandType.StoredProcedure);
            }
        }


        public bool LoadBatchParamterToDBTemplate(SalesRebateModel model,int UserId)
        {
            string sql = @" truncate table SJSalesRebateBatchGenerationTemplate;truncate table SJSalesRebateAmountRangeBatchGenerationTemplate ; 
                            insert into SJSalesRebateBatchGenerationTemplate(OrgId,RebateClass,RebatePctValue,RebatePctType,TaxAmountType,MinusLastPeriodRebateType,SettleDateBegin,SettleDateEnd,UserId) 
                            values(@OrgId,@RebateClass,@RebatePctValue,@RebatePctType,@TaxAmountType,@MinusLastPeriodRebateType,@SettleDateBegin,@SettleDateEnd,@UserId);";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool LoadAmountRangeListsToDBTemplate(IList<SalesRebateAmountRangeModel> lists)
        {
            string sql = @"  insert into SJSalesRebateAmountRangeBatchGenerationTemplate(AmountLower,AmountUpper,SalesRebatePctValue) values ";
            foreach (var item in lists)
            {
                sql += $" ( {item.AmountLower},{item.AmountUpper},{item.SalesRebatePctValue} ),"; 
            }
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql.TrimEnd(',')) > 0;
            }
        }

        public object BatchGenerationSalesRebateEntry(SalesRebateModel model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SettleDateBegin", model.SettleDateBegin, DbType.Date, ParameterDirection.Input);
            dp.Add("@SettleDateEnd", model.SettleDateEnd, DbType.Date, ParameterDirection.Input);
            dp.Add("@OrgId", model.OrgId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@RebateClass", model.RebateClass, DbType.Int32, ParameterDirection.Input);
            dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
              return  connection.ExecuteScalar("SJBatchGenerationSalesRebateEntryProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        public List<SalesRebateModel> GetSalesRebateOrgRecentParameterLists(SalesRebateModel parameterModel)
        {

            string sql = $" select  * from SJSalesRebateRecentParameterView where deleted = 0 and  OrgId = @OrgId and RebateClass = @RebateClass order by ModifyTime desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql, parameterModel).ToList();
            }
        }

        public List<SalesRebateModel> GetSalesRebateOrgRecentParameterLists(string filter = "")
        {

            string sql = $" select  * from SJSalesRebateRecentParameterView where 1=1 {filter} order by ModifyTime desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql).ToList();
            }
        }

        public List<SalesRebateModel> GetSalesRebateHistoryParameterLists(bool isDeleted, string filter = "")
        {

            string deleted = isDeleted ? "" : " and Deleted = 0 ";
            string sql = $" select  * from SJSalesRebateRecentParameterView where 1=1 {filter}  {deleted} order by ModifyTime desc";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql).ToList();
            }
        }

        public object InsertCurrentOrgRebateClassParameter(SalesRebateModel model)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SettleDateBegin", model.SettleDateBegin, DbType.Date, ParameterDirection.Input);
            dp.Add("@SettleDateEnd", model.SettleDateEnd, DbType.Date, ParameterDirection.Input);
            dp.Add("@OrgId", model.OrgId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@RebateClass", model.RebateClass, DbType.Int32, ParameterDirection.Input);
            dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.ExecuteScalar("SJInsertCurrentOrgRebateClassParameterProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        public bool SalesRebateParameterCopy(int sourceId, int destinationId , Guid sourceGuid) 
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@SourceId", sourceId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@DestinationId", destinationId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@SourceGuid", sourceGuid, DbType.Guid, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJSalesRebateParameterCopyProc", dp, null, null, CommandType.StoredProcedure)>0;
            }
        }
    }
}
