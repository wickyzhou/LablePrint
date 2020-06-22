using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ProductiveTaskWorkService
    {
        public IList<ProductiveTaskWorkModel> GetMutiOrgNoteWorkDetail(DateTime productionDate)
        {
            string sql = @" select *  from ICMO_OrderList
                            where (PATINDEX('%/[0-9][0-9][0-9][0-9] %',FRequest1)>0 or PATINDEX('%/[0-9][0-9][0-9][0-9] %',FRequest2)>0  or PATINDEX('%/[0-9][0-9][0-9][0-9] %',FRequest3)>0 )
                            and  FICMONo in(select FWorkNo from SJICMOList where FProductionDate=@FProductionDate and BrandName='华为' )  ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ProductiveTaskWorkModel>(sql,new { FProductionDate = productionDate }).ToList();
            }
        }


        public bool UpdateProductiveTaskWork(ProductiveTaskWorkModel model)
        {
            string sql = @" update ICMO_OrderList set FRequest1 = @FRequest1,FRequest2 = @FRequest2,FRequest3 = @FRequest3   where FICMONo=@FICMONo";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, model) > 0;
            }
        }

        public string GetProductiveTaskListFNote(DateTime productionDate,string fWorkNo,string fOrgId)
        {
            string sql = @" select FNote from SJICMOList where FProductionDate=@FProductionDate and FWorkNo = @FWorkNo and FOrgID=@FOrgID; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString( connection.ExecuteScalar<ProductiveTaskWorkModel>(sql, new { FProductionDate = productionDate, FWorkNo= fWorkNo, FOrgID= fOrgId }));
            }
        }



        
    }
}
