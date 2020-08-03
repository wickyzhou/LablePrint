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
            string sql = @" insert into SJSalesRebate(MaterialId,CaseId,OrgId,RebateClass,RebatePctValue,RebatePctType,TaxAmountType,Guid)
                            values(@MaterialId,@CaseId,@OrgId,@RebateClass,@RebatePctValue,@RebatePctType,@TaxAmountType,@Guid) ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        public bool Update(SalesRebateModel salesRebateModel)
        {
            string sql = @" update SJSalesRebate 
	                        set MaterialId=@MaterialId,CaseId=@CaseId,OrgId=@OrgId,RebateClass=@RebateClass,RebatePctValue=@RebatePctValue,
	                            RebatePctType=@RebatePctType,TaxAmountType=@TaxAmountType
                            where Id = @Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
            }
        }

        public bool Delete(SalesRebateModel salesRebateModel)
        {
            string sql = @" delete from SJSalesRebate where Id = @Id; delete from SJSalesRebateAmountRange where Guid=@Guid ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, salesRebateModel) > 0;
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


        public List<SalesRebateModel> GetSalesRebateLists()
        {
            string sql = @" select  * from SJSalesRebateView order by Id desc; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateModel>(sql).ToList();
            }
        }

        public List<SalesRebateModel> GetSalesRebateLists(string filter)
        {
            string sql = @" select  * from SJSalesRebateView " + filter +" order by Id desc";
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
    }
}
