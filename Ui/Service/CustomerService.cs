using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class CustomerService
    {
        public CustomerModel GetOrganizationById(int id)
        {
            string sql = @"select FItemID as Id,FName as FullName,FShortName as ShortName from t_Organization where FDeleted=0 and FItemID = @FItemID";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<CustomerModel>(sql, new { FItemID = id }).FirstOrDefault();
            }
        }
    }
}
