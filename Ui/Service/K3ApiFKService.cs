using Dapper;
using K3ApiModel;
using K3ApiModel.PurchaseRequisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class K3ApiFKService
    {
        public IList<PurchaseRequisitionFKModel> GetPurchaseRequisitionICItem()
        {
            string sql = @" select FAuxClassID,FUnitID,F_102,FNumber,FModel,FName,FFixLeadTime,FSecCoefficient,FSecUnitID from t_icitem ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<PurchaseRequisitionFKModel>(sql).ToList();
            }
        }

        public IList<BaseNumberNameModel> GetMeasureUnit()
        {
            string sql = @" select FMeasureUnitID FId,FNumber,FName from t_Measureunit ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BaseNumberNameModel>(sql).ToList();
            }
        }

        public IList<BaseNumberNameModel> GetK3Employee()
        {
            string sql = @" select FItemID FId,FNumber,FName from t_Emp where FDeleted=0 order by FItemID desc ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<BaseNumberNameModel>(sql).ToList();
            }
        }
    }
}
