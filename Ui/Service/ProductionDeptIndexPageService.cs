using Dal;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ProductionDeptIndexPageService
    {
        public bool GenWorkNoEarningRatio(DateTime date)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@FirstDayOfMonth", date, DbType.Date, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJWorkNoEarningRatioProc", dp, null, null, CommandType.StoredProcedure)>0;
            }
        }


        public DataTable GetWorkNoEarningRatio(DateTime date)
        {
            return SqlHelper.ExecuteDataTable(" select *from SJEarningRatioView where 日期 = @RiQi  ", new SqlParameter[] { new SqlParameter("@RiQi", date) });
        }

        public int SyncBucketInfo()
        {
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToInt32( connection.ExecuteScalar("SJSyncBucketInfo", null, null, null, CommandType.StoredProcedure));
            }
        }
    }
}
