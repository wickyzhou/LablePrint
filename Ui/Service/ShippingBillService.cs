using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
   public class ShippingBillService
    {
        public IList<ShippingBillModel> GetAllShippingBills()
        {
            string sql = @"select * from SJShippingBill ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ShippingBillModel>(sql).ToList();
            }
        }
    }
}
