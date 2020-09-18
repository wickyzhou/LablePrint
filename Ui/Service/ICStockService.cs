using Dapper;
using ImportVerificationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ICStockService
    {
        public List<ICStockBill29ImportVerificationModel> GetICStockBill29ImportVerificationLists()
        {
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ICStockBill29ImportVerificationModel>("SJICStockBill29ImportVerificationProc",null).OrderBy(m=>m.Seq).ToList();
            }
        }


        public List<ICStockBill1ImportVerificationModel> GetICStockBill1ImportVerificationLists()
        {
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ICStockBill1ImportVerificationModel>(" select * from SJICStockBill1ImportVerificationView order by Seq ", null).ToList();
            }
        }
    }

}
