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


        public List<ItemProfitAccountingModel> GetItemProfitAccountingLists()
        {
            string sql = @" select  * from SRItemProfitAccounting; ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ItemProfitAccountingModel>(sql).ToList();
            }
        }


        public List<ItemProfitAccountingMonthlyModel> GetItemProfitAccountingMonthlyLists()
        {
            string sql = @" select  * from SRItemProfitAccountingMonthly; ";
            using (var connection = SqlDb.UpdateConnection)
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
    }
}
