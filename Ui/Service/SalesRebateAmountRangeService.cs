using Dapper;
using Model;
using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Linq;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SalesRebateAmountRangeService
    {
        public List<SalesRebateRecentParameterSonModel> GetSalesRebateAmountRangeRecentParameterLists(Guid guid)
        {
            string sql = @" select * from SJSalesRebateRecentParameterSon where Guid=@Guid  order by AmountLower ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateRecentParameterSonModel>(sql, new { Guid = guid }).ToList();
            }
        }

        public Guid RecentSonParameterInsert(SalesRebateRecentParameterSonModel model, Guid mainGuid)
        {
            if (mainGuid == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {

                string sql = @"declare @Guid1 uniqueidentifier = newid() ; insert into SJSalesRebateRecentParameterSon(AmountUpper,AmountLower,SalesRebatePctValue,Guid)
                            values(@AmountUpper,@AmountLower,@SalesRebatePctValue,@Guid1); select @Guid1 as r ";
                using (var connection = SqlDb.UpdateConnection)
                {
                    return Guid.Parse(Convert.ToString(connection.ExecuteScalar(sql, model)));
                }
            }
            else
            {
                string sql = $"insert into SJSalesRebateRecentParameterSon(AmountUpper,AmountLower,SalesRebatePctValue,Guid) values(@AmountUpper,@AmountLower,@SalesRebatePctValue,'{mainGuid}');";
                using (var connection = SqlDb.UpdateConnection)
                {
                    connection.Execute(sql, model);
                    return mainGuid;
                }
            }

        }


        public Guid RecentSonParameterClear()
        {
            string sql = @" declare @Guid uniqueidentifier = newid();;
                            select @Guid as R;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Guid.Parse(Convert.ToString(connection.ExecuteScalar(sql))) ;
            }
        }


        //public bool RecentSonParameterDelete(Guid guid)
        //{
        //    string sql = $" delete from SJSalesRebateRecentParameterSon where Guid=@Guid; ";
        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        return connection.Execute(sql, new { Guid = guid }) > 0;
        //    }
        //}
    }
}
