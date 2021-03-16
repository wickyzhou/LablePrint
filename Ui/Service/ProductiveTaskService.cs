using Dapper;
using K3ApiModel.SO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ProductiveTaskService
    {
        public IEnumerable<SrSoDataModel> GetSrOrderData(DateTime productionDate)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SrSoDataModel>("SJGetSrOrderDataProc", dp, null, true, null, CommandType.StoredProcedure);
            }
        }


        public void ClearExistsOrderEntryId()
        {
            string sql = " truncate table SJKey ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                 connection.Execute(sql);
            }
        }
    }
}
