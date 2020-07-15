using Dapper;
using ImportVerificationModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class MaterialPlanInventoryService
    {
        public IList<MaterialPlanSeorderModel> GetMaterialPlanSeorderLists(DateTime beginDate,DateTime endDate)
        {
            string sql = @"  select * from SJYuanCaiLiaoJiSuan where FDate >= @BeginDate and FDate <= @EndDate order by FInterID desc,FEntryID ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialPlanSeorderModel>(sql,new {BeginDate=beginDate,EndDate=endDate }).ToList();
            }
        }

        public IList<MaterialDemandModel> GetMaterialDemandLists()
        {
            string sql = @"  select * from SJMaterialDemand ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialDemandModel>(sql).ToList();
            }
        }

        public IList<MaterialBomModel> GetMaterialBomLists(string itemIds)
        {
            string sql = @"  select* from SJMaterialBomView where ItemId in( "+ itemIds + ") ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialBomModel>(sql).ToList();
            }
        }

        public IList<int> GetMaterialFItemIds(string itemNames)
        {
            string sql = @" select fitemId from 
                            (select ROW_NUMBER()over(partition by FName order by case when FNumber like 'YL.SC%' then 0 else 1 end) rnk,FItemID  from t_ICItem where FName in("+itemNames+@")) as t
                            where rnk=1 ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<int>(sql).ToList();
            }
        }


        public bool ImportExcel()
        {
            return true;
        }

        public IList<PurchaseRequisitionImportVerificationModel> GetCheckedPurchaseRequisitionMaterialLists()
        {
            string sql = @" ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<PurchaseRequisitionImportVerificationModel>(sql).ToList();
            }
        }

        

        //public bool DeleteOilSampleFlowLog(decimal id)
        //{
        //    string sql = @" delete from SROilSampleFlowPrintLog where Id=@Id ; ";
        //    using (var connection = SqlDb.UpdateConnectionOa)
        //    {
        //        return connection.Execute(sql, new { Id = id }) > 0;
        //    }
        //}
    }
}
