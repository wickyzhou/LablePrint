using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
   public class ConsignmentBillService
    {
        public IList<ConsignmentBillModel> GetAllConsignmentBills()
        {
            string sql = @"select * from SJConsignmentBill ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql).ToList();
            }
        }

        public IList<ConsignmentBillModel> GetAllConsignmentBills(string filter)
        {
            string sql = @"select * from SJConsignmentBill where 1=1  "+ filter;
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ConsignmentBillModel>(sql).ToList();
            }
        }

        public bool Update(ConsignmentBillModel consignmentBill)
        {
            string sql = @"update SJConsignmentBill set FCurrencyQuatity=@FCurrencyQuatity  where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, consignmentBill) > 0;
            }
        }
    }
}
