using Dapper;
using K3ApiModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class SrK3Service
    {
        public IEnumerable<SrOrganizationModel> GetSrOrganizationLists()
        {
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                string sql = @" select a.FItemID as OrgId,a.FNumber as OrgNumber,a.FShortName OrgShortName,a.FName OrgName,
                                        b.FItemID as EmpId,b.FNumber as EmpNumber,b.FName as EmpName
                                from t_Organization a left
                                join t_Emp b on a.F_102 = b.FItemID
                                where FStatus = 1072";
                return connection.Query<SrOrganizationModel>(sql);
            }
        }

        public IEnumerable<SrMaterialModel> GetSrMaterialLists()
        {
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                string sql = @"  select FItemID as MaterialId, FNumber as MaterialNumber, FName as MaterialName from t_ICItem where FDeleted = 0 ";
                return connection.Query<SrMaterialModel>(sql);
            }
        }

        public IEnumerable<SrPriceModel>  GetSrPriceControlLists()
        {
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                string sql = @"  SELECT FRelatedID OrgId,FItemID MaterialId,FPrice Price FROM vw_ICPrcPly_CtoI ";
                return connection.Query<SrPriceModel>(sql);
            }
        }

        public IEnumerable<BaseNumberNameModel> GetSpecLists()
        {
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                string sql = @"  select FItemID,FNumber,FName from t_Item where FItemClassID = 3003 ";
                return connection.Query<BaseNumberNameModel>(sql);
            }
        }
    }
}
