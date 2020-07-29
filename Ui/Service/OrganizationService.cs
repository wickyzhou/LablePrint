using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class OrganizationService
    {
        public IList<OrganizationModel> GetOrganizationLists()
        {
            string sql = @"select FItemID as Id,FName as FullName,FShortName as ShortName from t_Organization where FDeleted=0 ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<OrganizationModel>(sql).ToList();
            }
        }
    }
}
