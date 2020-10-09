
using Dapper;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ui.Helper;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class PrintA4Service
    {
        public List<LabelPrintHistoryModel>  GetHistoryLists(string filter = "")
        {
            string sql = $" select a.*,b.LastPrintTime,b.PrintedCount from SJLabelPrintHistory a left join (select RowHashValue,sum(PrintBucket) PrintedCount,MAX(PrintTime) LastPrintTime from SJLabelPrintA4Log group  by  RowHashValue) b on a.RowHashValue = b.RowHashValue  where 1=1  {filter}";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<LabelPrintHistoryModel>(sql).ToList();
            }
        }

        public DataTable GetExportData1(string dataGridName,int userId,string filter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            DataTable dataTable = SqlHelper.ExecuteDataTable(@" select ColumnFieldName ,ColumnHeaderName from SJDataGridUserCustom where IsDownLoad = 1 and DataGridName = @DataGridName and UserId = @UserId       order by columnorder ;"
                    , new SqlParameter[] { new SqlParameter("@DataGridName", dataGridName), new SqlParameter("@UserId", userId) });
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                stringBuilder.Append($",{dataTable.Rows[i][0]} as [{dataTable.Rows[i][1]}]");
            }

            string fields = stringBuilder.ToString().Substring(1);
            string sql = $"  select {fields} from SJLabelPrintHistory  where 1=1 {filter} ";

            return SqlHelper.ExecuteDataTable(sql,null);
        }
    }
}
