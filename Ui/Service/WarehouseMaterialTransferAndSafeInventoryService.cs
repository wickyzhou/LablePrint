using Dapper;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ui.Helper;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class WarehouseMaterialTransferAndSafeInventoryService
    {

        /// <summary>
        /// 获取原材料的即时库存
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="productionDate"></param>
        /// <returns></returns>
        public List<MaterialTimelyInventoryModel> GetGetMaterialTimelyInventoryLists(int materialId,DateTime productionDate,int stockId,int sourceStockId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@MaterialId", materialId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            dp.Add("@BatchTypeId", stockId, DbType.Int32, ParameterDirection.Input);
            dp.Add("@ParentStockId", sourceStockId, DbType.Int32, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialTimelyInventoryModel>("SJGetMaterialTimelyInventory", dp,null,true,null,CommandType.StoredProcedure).ToList();
            }
        }

        /// <summary>
        /// 查询原材料所需数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<BatchBomRequestSummaryModel> GetBatchBomRequestDetailSummaryLists(string filter = "")
        {
            string sql = $" select * from SJBatchBOMSummaryView where   1 = 1 {filter} order by MaterialId,ProductionDate;  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BatchBomRequestSummaryModel>(sql).ToList();
            }
        }


        /// <summary>
        /// 获取当天生产型号所需的原材料
        /// </summary>
        /// <param name="productionDate"></param>
        /// <returns></returns>
        public void SplitBatchBomRequest(DateTime productionDate)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add("@ProductionDate", productionDate, DbType.Date, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
               connection.Execute("SJSplitBatchBomRequestProc", dp, null, null, CommandType.StoredProcedure);
            }

        }

        /// <summary>
        /// 刷新原材料对应的车间现场库存数量
        /// </summary>
        /// <param name="productionDate"></param>
        /// <returns></returns>
        public void RefreshWorkshopInventoryQty(string filter)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add("@Filter", filter, DbType.String, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
               connection.Execute("SJWuLiaoShouFaRiBaoBiaoProc", dp, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 将选定的及时库存数据，插入到发料调拨表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertDeliverTransfer(MaterialTimelyInventoryModel model)
        {
            string sql = @" insert into SJDeliverTransfer (ProductionDate,MaterialId,MaterialNumber,MaterialName,StockId,StockNumber,StockName,BatchNo,TransferingWeight,BatchTypeId,ParentStockId) values(@ProductionDate,@MaterialId,@MaterialNumber,@MaterialName,@StockId,@StockNumber,@StockName,@BatchNo,@TransferingWeight,@BatchTypeId,@ParentStockId); ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        /// <summary>
        /// 获取发料调拨列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MaterialTimelyInventoryModel> GetDeliverTransferLists(BatchBomRequestSummaryModel model)
        {
            string sql = $" select * from SJDeliverTransfer where  ProductionDate = @ProductionDate and MaterialId = @MaterialId  and  BatchTypeId = @BatchTypeId;  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialTimelyInventoryModel>(sql, model).ToList();
            }
        }

        /// <summary>
        /// 删除发料调拨数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteDeliverTransfer(int id)
        {
            string sql = @" delete from  SJDeliverTransfer where Id =@Id; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id}) > 0;
            }
        }

        
        /// <summary>
        /// 按调拨单号删除
        /// </summary>
        /// <param name="transferBillNo"></param>
        /// <returns></returns>
        public bool DeleteDeliverTransfer(string transferBillNo)
        {
            string sql = $" update SJDeliverTransfer set TransferedWeight= null,TransferedTime = null, TransferedBillNo=null,TransferingWeight=TransferedWeight where TransferedBillNo = @TransferedBillNo; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,new { TransferedBillNo = transferBillNo }) > 0;
            }
        }

        /// <summary>
        /// 查看调拨单在K3里面是否存在
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public bool ExistsK3Bill(string billNo)
        {
            string sql = $" select top 1  1 as r from  ICStockBill  where FBillNo = @FBillNo ; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return !string.IsNullOrEmpty(Convert.ToString(connection.ExecuteScalar(sql,new { FBillNo = billNo})));
            }
        }

        /// <summary>
        /// 调拨单插入成功后，更新单号
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateBillNo(string ids,string billNo)
        {
            string sql = $" update SJDeliverTransfer set  TransferedWeight = TransferingWeight,TransferingWeight=null,TransferedBillNo = '{billNo}', TransferedTime = getdate() where  Id in ({ids}) ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 获取分类导出数据
        /// </summary>
        /// <param name="productionDate"></param>
        /// <param name="orderedColumns"></param>
        /// <returns></returns>
        public DataTable GetSJBatchBomRequestDeliveryExportData(DateTime productionDate,string orderedColumns)
        {
            return SqlHelper.ExecuteDataTableProcedure("SJBatchBomRequestDeliveryExportProc", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate), new SqlParameter("@OrderedColumns", orderedColumns) });
        }

        public List<MaterialTimelyInventoryModel> GetK3InsertData(DateTime productionDate)
        {
            string sql = @" select a.*,b.FNumber ParentStockNumber ,b.FName ParentStockName from SJDeliverTransfer a join t_Stock b on a.ParentStockId = b.FItemID
where ProductionDate = @ProductionDate and TransferingWeight> 0;  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialTimelyInventoryModel>(sql, new { ProductionDate= productionDate }).ToList();
            }
        }


        public DataTable GeSJBatchBOMSummaryExportData(DateTime productionDate)
        {
            return SqlHelper.ExecuteDataTable("select * from SJBatchBOMSummaryExportView where 生产日期 = @ProductionDate", new SqlParameter[] { new SqlParameter("@ProductionDate", productionDate)});
        }


    }
}
