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

        public bool Update(SalesRebateModel salesRebateModel)
        {
            string sql = @" update SJSalesRebate 
	                        set CaseId=@CaseId,OrgId=@OrgId,RebateClass=@RebateClass,RebatePctValue=@RebatePctValue,OrgCode=@OrgCode
	                            RebatePctType=@RebatePctType,TaxAmountType=@TaxAmountType
                            where Id = @Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
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
            string sql = $" delete from  SJSalesRebate  where Guid in ({guids}); delete from SJSalesRebateAmountRange where Guid in ({guids}); ";
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
            string sql = $" select  * from SJSalesRebateView where 1=1 {filter} {userString} {isDeletedString} order by Id desc";
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


        public void CalculateSalesRebateAmount(DateTime beginDate,DateTime endDate)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@BeginDate", beginDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@EndDate", endDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                 connection.ExecuteScalar("SJCalculateSalesRebateAmountProc", dp, null, null, CommandType.StoredProcedure);
            }
        }


        public bool LoadBatchParamterToDBTemplate(SalesRebateModel model,Guid guid)
        {
            model.Guid = guid;
            string sql = @" truncate table SJSalesRebateBatchGenerationTemplate;
                            insert into SJSalesRebateBatchGenerationTemplate(OrgId,RebateClass,RebatePctValue,RebatePctType,TaxAmountType,MinusLastPeriodRebateType,SettleDateBegin,SettleDateEnd,Guid) 
                            values(@OrgId,@RebateClass,@RebatePctValue,@RebatePctType,@TaxAmountType,@MinusLastPeriodRebateType,@SettleDateBegin,@SettleDateEnd,@Guid);";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool LoadAmountRangeListsToDBTemplate(IList<SalesRebateAmountRangeModel> lists, Guid guid)
        {
            string sql = @" truncate table SJSalesRebateAmountRangeBatchGenerationTemplate ;  insert into SJSalesRebateAmountRangeBatchGenerationTemplate(AmountLower,AmountUpper,SalesRebatePctValue,Guid) values ";
            foreach (var item in lists)
            {
                sql += $" ( {item.AmountLower},{item.AmountUpper},{item.SalesRebatePctValue},'{guid}' ),"; 
            }
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql.TrimEnd(',')) > 0;
            }
        }

        public void BatchGenerationSalesRebateEntry(int userId)
        {
            using (var connection = SqlDb.UpdateConnection)
            {
                connection.ExecuteScalar("SJBatchGenerationSalesRebateEntryProc", new { UserId= userId }, null, null, CommandType.StoredProcedure);
            }
        }

        
    }
}
