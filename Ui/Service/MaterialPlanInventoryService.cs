using Dal;
using Dapper;
using ImportVerificationModel;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class MaterialPlanInventoryService
    {
        public List<MaterialPlanSeOrderFullModel> GetMaterialPlanSeorderFullLists(DateTime beginDate,DateTime endDate)
        {
            string sql = @"  select * from SJYuanCaiLiaoJiSuanView where FDate >= @BeginDate and FDate <= @EndDate ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialPlanSeOrderFullModel>(sql,new {BeginDate=beginDate,EndDate=endDate }).ToList();
            }
        }

        public List<MaterialBomModel> GetMaterialBomLists(string fNumbers)
        {
            string sql = @"  select * from SJMaterialBomView where FNumber in( " + fNumbers + ") ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialBomModel>(sql).ToList();
            }
        }

        public DataTable GetMaterialBomLists()
        {
            return SqlHelper.ExecuteDataTable(@" select * from SJMaterialBomView ;", null);
        }

        public List<int> GetMaterialFItemIds(string itemNames)
        {
            string sql = @" select fitemId from 
                            (select ROW_NUMBER()over(partition by FName order by case when FNumber like 'YL.SC%' then 0 else 1 end) rnk,FItemID  from t_ICItem where FName in("+itemNames+@")) as t
                            where rnk=1 ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<int>(sql).ToList();
            }
        }


        public bool DeleteMaterialPlanInventory()
        {
            string sql = @" truncate table SJMaterialPlanInventory;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql) > 0;
            }
        }

        public IEnumerable<MaterialPlanInventoryModel> GetMaterialPlanInventoryLists(string ids)
        {

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Ids", ids, DbType.String, ParameterDirection.Input);
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MaterialPlanInventoryModel>("SJCalculateMaterialPlanInventoryProc", dp, null,true, null, CommandType.StoredProcedure);
            }
        }

        //// 40004 -- 生产Bom  0 -- 未禁用  1072 -- 使用中
        //public List<ExpandBomModel> GetScBom()
        //{
        //    string sql = @"  select a.FItemID PitemId,b.FItemID CitemId,a.FQty PQty,b.FQty CQty,FBOMNumber
        //                    from ICBOM  a join ICBomChild b on a.FInterID=b.FInterID  where  a.FHeadSelfZ0134=40004  and a.FForbid=0  and FUseStatus=1072 ";
        //    using (var connection = SqlDb.UpdateConnection)
        //    {
        //        return connection.Query<ExpandBomModel>(sql).ToList();
        //    }
        //}
    }
}
