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
    public class WarehouseMaterialTransferAndSafeInventoryService
    {

        public bool Update(int id,string batchQty)
        {
            string sql = @" update SJWarehouseTransferToWorkshop set FBatchNoAndActualQty = @FBatchNoAndActualQty where Id =@Id ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id, FBatchNoAndActualQty = batchQty }) > 0;
            }
        }


        public List<InventoryBatchNoModel> GetInventoryBatchNoLists(int materialId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@MaterialId", materialId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<InventoryBatchNoModel>("SJGetMaterialStockBatchInventoryQty", dp,null,true,null,CommandType.StoredProcedure).ToList();
            }
        }

        public List<WarehouseTransferToWorkshopModel> GetWarehouseTransferToWorkshopLists(string filter = "")
        {
            string sql = $" select * from SJWarehouseTransferToWorkshop where   CreateTime <GETDATE() {filter};  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<WarehouseTransferToWorkshopModel>(sql).ToList();
            }
        }



        public object GenerationNewData(DateTime beginDate,DateTime endDate)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add("@BeginDate", beginDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@EndDate", endDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
               return connection.ExecuteScalar("SJGenWarehouseTransferToWorkshopNewDataProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

    }
}
