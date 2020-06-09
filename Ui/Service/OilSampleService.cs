using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class OilSampleService
    {
        public IList<OilSampleFlowModel> GetOilSampleFlow()
        {
            string sql = @"select * from SROilSampleFlowView; ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSampleFlowModel>(sql).ToList();
            }
        }

        public IList<ExpressPrintModel> GetExpressPrintData(int id)
        {
            string sql = @"select * from SRExpressPrintView where Id=@Id ";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<ExpressPrintModel>(sql,new {Id=id}).ToList();
            }
        }

        public IList<OilSamplePrintModel> GetOilSamplePrintData(int id)
        {
            string sql = @" select *
	                            ,cast(cast(TotalWeight/WeightPerBucket as int) as numeric(8,2)) IntegratedPrintCount
	                            ,case when TotalWeight%WeightPerBucket>0 then 1 else 0 end PlusPrintCount
	                            ,TotalWeight%WeightPerBucket  WeightPerBucket2
                            from SROilSamplePrintView where WeightPerBucket>0 and Id=@Id";
            using (var connection = SqlDb.UpdateConnectionOa)
            {
                return connection.Query<OilSamplePrintModel>(sql, new { Id = id }).ToList();
            }
        }



        //public bool UpdateConsignmentBill(ConsignmentBillModel consignmentBill)
        //{

        //    string sql = @" update SJConsignmentBill set CurrencyQuantity=@CurrencyQuantity,UndoQuantity=@UndoQuantity,TotalQuantity=@TotalQuantity  where InterId=@InterId ; ";
        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        return connection.Execute(sql, consignmentBill) > 0;
        //    }
        //}
    }
}
