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
    public class PrintLSService
    {
        public List<LabelPrintLSModel> GetLists(DateTime productionDate,int barcodeTypeId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@BarcodeTypeId", barcodeTypeId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<LabelPrintLSModel>("SJGetLabelPrintLSProc", dp, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }


        public bool GenerationPrintData(DateTime productionDate)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute("SJGetLabelPrintLSBaseDataProc", dp, null, null, CommandType.StoredProcedure)>0;
            }
        }


        public bool ExistsPrintData(DateTime productionDate)
        {
            string sql = $" select top 1  1 as r from SJLabelPrintLS where ProductionDate =@ProductionDate";
            using (var connection = SqlDb.UpdateConnection)
            {
                object s = connection.ExecuteScalar(sql, new { ProductionDate = productionDate });
                return  s == null?false:true ;
            }
        }
    }
}
