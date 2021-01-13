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
    public class SemiMaterialsInventoryService
    {
        public List<SemiMaterialsInventoryModel> GetSemiMaterialsLists(DateTime productionDate, string materialName,string materialNumber,string batchQty)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@MaterialName", materialName, DbType.String, ParameterDirection.Input);
            dp.Add("@MaterialNumber", materialNumber, DbType.String, ParameterDirection.Input);
            dp.Add("@BatchQty", batchQty, DbType.String, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<SemiMaterialsInventoryModel>("SJGetSemiMaterialsInventoryProc", dp, null,true,null,CommandType.StoredProcedure).ToList();
            }

        }

        public List<MaterialTimelyInventoryModel> GetTimelyInventoryLists(int materialId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@MaterialId", materialId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@ProductionDate", null, DbType.Date, ParameterDirection.Input);
            dp.Add("@BatchTypeId", null, DbType.Int32, ParameterDirection.Input);
            dp.Add("@ParentStockId", null, DbType.Int32, ParameterDirection.Input);
            dp.Add("@TypeId", 1, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialTimelyInventoryModel>("SJGetMaterialTimelyInventory", dp, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }
    }
}
