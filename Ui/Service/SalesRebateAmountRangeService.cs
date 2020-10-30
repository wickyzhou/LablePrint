using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SalesRebateAmountRangeService
    {
        public List<SalesRebateAmountRangeModel> GetSalesRebateAmountRangeLists(Guid guid)
        {
            string sql = @" select * from SJSalesRebateAmountRange where Guid=@Guid  order by AmountLower ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateAmountRangeModel>(sql, new { Guid = guid }).ToList();
            }
        }

        public List<SalesRebateAmountRangeModel> GetSalesRebateAmountRangeRecentParameterLists(Guid guid)
        {
            string sql = @" select * from SJSalesRebateRecentParameterSon where Guid=@Guid  order by AmountLower ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateAmountRangeModel>(sql, new { Guid = guid }).ToList();
            }
        }

        public bool RecentSonParameterInsert(SalesRebateAmountRangeModel model)
        {
            string sql = @" insert into SJSalesRebateRecentParameterSon(AmountUpper,AmountLower,SalesRebatePctValue,Guid)
                            values(@AmountUpper,@AmountLower,@SalesRebatePctValue,@Guid) ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }


        public bool RecentSonParameterDelete(int id)
        {
            string sql = @" delete from  SJSalesRebateRecentParameterSon where Id = @Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }

        public bool Delete(Guid guid)
        {
            string sql = @" delete from  SJSalesRebateAmountRange where Guid = @Guid ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Guid = guid }) > 0;
            }
        }

        public bool RecentSonParameterDelete(Guid guid)
        {
            string sql = $" delete from SJSalesRebateRecentParameterSon where Guid=@Guid; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Guid = guid }) > 0;
            }
        }
    }
}
