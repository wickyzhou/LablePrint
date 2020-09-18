using Dal;
using Dapper;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Ui.MVVM.Common;

namespace Ui.Service
{
    public class ItemProfitAccountingService
    {
        public List<ComboBoxSearchModel> GetSettleMonthLists()
        {
            string sql = @" select  MonthSeq as Id,MonthValue as SearchText from SJMonthTable; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ComboBoxSearchModel>(sql).ToList();
            }
        }


        public List<ItemProfitAccountingModel> GetItemProfitAccountingLists(string filter = "")
        {
            string sql = $" select  * from SRItemProfitAccountingSummaryView where 1=1 {filter} ; ";
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                return connection.Query<ItemProfitAccountingModel>(sql).ToList();
            }
        }


        public List<ItemProfitAccountingMonthlyModel> GetItemProfitAccountingMonthlyLists(string filter = "")
        {
            string sql = $" select  * from SRItemProfitAccountingMonthly where 1=1  {filter}; ";
            using (var connection = SqlDb.UpdateConnectionSR)
            {
                return connection.Query<ItemProfitAccountingMonthlyModel>(sql).ToList();
            }
        }

        public DataTable GetOaItemCostDataTable(int monthId)
        {
            return SqlHelper.ExecuteDataTableOa(@" select * from SROaItemCostView  where MonthId=@MonthId; ", new SqlParameter[] { new SqlParameter("@MonthId", monthId) });
        }


        public void ImportDataTableToDatabaseTableSR(DataTable dataTable,string tableName)
        {
            SqlHelper.ExecuteNonQuerySR(" truncate table "+ tableName,null);
            SqlHelper.LoadDataTableToDBModelTableSR(dataTable, tableName);     
        }

        public bool AccountItemProfit(int monthId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@MonthId", monthId, DbType.Int32, ParameterDirection.Input);

            using (var connection = SqlDb.UpdateConnectionSR)
            {
                return connection.Execute("SRItemProfitAccountingProcedure", dp, null, null, CommandType.StoredProcedure)>0;
            }
        }
    }
}
