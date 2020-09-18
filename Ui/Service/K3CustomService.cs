using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class K3CustomService
    {
        public IList<CaseModel> GetCaseLists()
        {
            string sql = @" select FItemID Id,F_101 BrandName,FName CaseName from t_Item_3014 ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<CaseModel>(sql).ToList();
            }
        }
    }
}
