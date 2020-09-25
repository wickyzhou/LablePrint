using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class PrintA4Service
    {
        public List<LabelPrintHistoryModel>  GetHistoryLists(string filter = "")
        {
            string sql = $" select * from SJLabelPrintHistory where 1=1  {filter}";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<LabelPrintHistoryModel>(sql).ToList();
            }
        }
    }
}
