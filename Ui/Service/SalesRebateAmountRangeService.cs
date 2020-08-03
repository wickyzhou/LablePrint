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
            string sql = @" select *,(select ItemValue from SJEnumTable where GroupSeq=999 and ItemSeq=SJSalesRebateAmountRange.IsValid)  IsValidName
                            from SJSalesRebateAmountRange where Guid=@Guid and IsValid=1 order by AmountLower ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateAmountRangeModel>(sql,new { Guid=guid }).ToList();
            }
        }

        public List<SalesRebateAmountRangeModel> GetSalesRebateAmountRangeHistoryLists(Guid guid)
        {
            string sql = @" select *,(select ItemValue from SJEnumTable where GroupSeq=999 and ItemSeq=SJSalesRebateAmountRange.IsValid)  IsValidName 
                            from SJSalesRebateAmountRange where Guid=@Guid  order by AmountLower ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SalesRebateAmountRangeModel>(sql, new { Guid = guid }).ToList();
            }
        }

        //public List<SalesRebateAmountRangeModel> CopySalesRebateAmountRangeLists(Guid oldGuid,Guid newGuid)
        //{
        //    string sql = @" insert into SJSalesRebateAmountRange(Guid,EffectiveDate,ExpirationDate,IsValid,CreateTime,AmountLower,AmountUpper,SalesRebatePctValue) 
        //                    select @NewGuid,EffectiveDate,ExpirationDate,IsValid,CreateTime,AmountLower,AmountUpper,SalesRebatePctValue from SJSalesRebateAmountRange where Guid=@OldGuid and IsValid=1 ; ";
        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        return connection.Query<SalesRebateAmountRangeModel>(sql, new { OldGuid= oldGuid, NewGuid = newGuid }).ToList();
        //    }
        //}


        public bool Insert(SalesRebateAmountRangeModel model)
        {
            string sql = @" insert into SJSalesRebateAmountRange(AmountUpper,AmountLower,SalesRebatePctValue,EffectiveDate,ExpirationDate,IsValid,Guid)
                            values(@AmountUpper,@AmountLower,@SalesRebatePctValue,@EffectiveDate,@ExpirationDate,@IsValid,@Guid) ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public bool Update(SalesRebateAmountRangeModel model)
        {
            string sql = @" update SJSalesRebateAmountRange 
                                    set AmountUpper=@AmountUpper,AmountLower=@AmountLower,SalesRebatePctValue=@SalesRebatePctValue,
                                         EffectiveDate=@EffectiveDate,ExpirationDate=@ExpirationDate,IsValid=@IsValid 
                            where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }
    }
}
