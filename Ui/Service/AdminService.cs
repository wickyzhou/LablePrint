using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class AdminService
    {
        public List<ActionOperationLogModel> GetActionOperationLogLists()
        {
            string sql = @" select * from SJActionOperationLog; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ActionOperationLogModel>(sql).ToList();
            }
        }
    }
}
